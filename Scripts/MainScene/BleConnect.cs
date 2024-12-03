using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android; // Android 권한 관리 클래스 추가
using System.Text;
using Newtonsoft.Json.Linq;

public class BleConnect : MonoBehaviour
{
    public string DeviceName = "MLT-BT05";
    public string ServiceUUID = "FFE0";
    public string Characteristic = "FFE1";

    public Text HM10_Status;
    public Text BluetoothStatus;
    public Text debugText; // 디버그 메시지 출력용 UI 텍스트
    public GameObject PanelMiddle;
    public Text TextToSend;
    public Text SensorData;

    public Slider lightSlider;
    public Slider soilMoistureSlider;
    public Slider temperatureSlider;
    public Slider humiditySlider;
    public Slider pirSlider;

    enum States
    {
        None,
        Scan,
        Connect,
        Subscribe,
        Unsubscribe,
        Disconnect,
    }

    private bool _connected = false;
    private float _timeout = 0f;
    private States _state = States.None;
    private string _hm10;


    void Start()
    {
        HM10_Status.text = "";
        RequestPermissions(); // 권한 요청
        StartProcess(); // BLE 초기화 및 프로세스 시작
    }

    void Update()
    {
        if (_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f)
            {
                _timeout = 0f;

                switch (_state)
                {
                    case States.None:
                        ShowDebug("State: None - Idle.");
                        break;

                    case States.Scan:
                        StartScanning();
                        break;

                    case States.Connect:
                        ConnectToDevice();
                        break;

                    case States.Subscribe:
                        SubscribeToNotifications();
                        break;

                    case States.Unsubscribe:
                        UnsubscribeFromDevice();
                        break;

                    case States.Disconnect:
                        DisconnectFromDevice();
                        break;
                }
            }
        }
    }

    // 권한 요청
    void RequestPermissions()
    {

        if (!Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN"))
        {
            Permission.RequestUserPermission("android.permission.BLUETOOTH_SCAN");
        }
        if (!Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT"))
        {
            Permission.RequestUserPermission("android.permission.BLUETOOTH_CONNECT");
        }
        if (!Permission.HasUserAuthorizedPermission("android.permission.ACCESS_FINE_LOCATION"))
        {
            Permission.RequestUserPermission("android.permission.ACCESS_FINE_LOCATION");
        }
    }

    // BLE 초기화
    void StartProcess()
    {
        BluetoothStatus.text = "Initializing...";

        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            SetState(States.Scan, 0.1f);
            BluetoothStatus.text = "Initialized";
            ShowDebug("Bluetooth initialized successfully.");
        },
        (error) =>
        {
            ShowDebug($"Initialization Error: {error}");
            BluetoothStatus.text = $"Error: {error}";
        });
    }

    // BLE 스캔 시작
    void StartScanning()
    {
        BluetoothStatus.text = "Scanning for devices...";
        ShowDebug("Scanning for devices...");

        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
        {
            ShowDebug($"Found Device: Name = {name}, Address = {address}");

            if (name.Contains(DeviceName))
            {
                BluetoothLEHardwareInterface.StopScan();
                _hm10 = address;
                HM10_Status.text = $"Found {DeviceName}";
                ShowDebug($"Found target device: {DeviceName} at address: {address}");
                SetState(States.Connect, 0.5f);
            }
        }, null, false, false);
    }

    // BLE 장치 연결
    void ConnectToDevice()
    {
        HM10_Status.text = "Connecting...";
        ShowDebug($"Connecting to {_hm10}...");

        BluetoothLEHardwareInterface.ConnectToPeripheral(_hm10, null, null, (address, serviceUUID, characteristicUUID) =>
        {
            ShowDebug($"Connected to: Address = {address}, ServiceUUID = {serviceUUID}, CharacteristicUUID = {characteristicUUID}");

            if (IsEqual(serviceUUID, ServiceUUID) && IsEqual(characteristicUUID, Characteristic))
            {
                _connected = true;
                SetState(States.Subscribe, 2f);
                HM10_Status.text = "Connected!";
                ShowDebug("Successfully connected. Ready to subscribe.");
            }
        },
        (disconnectedAddress) =>
        {
            ShowDebug($"Disconnected from: {disconnectedAddress}");
            HM10_Status.text = "Disconnected";
            _connected = false;
            Reset();
        });
    }

    // 데이터 수신 구독
    private string partialData = ""; // 조각난 데이터를 저장할 변수

    void SubscribeToNotifications()
    {
        HM10_Status.text = "Subscribing...";
        ShowDebug("Subscribing to notifications...");

        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(
            _hm10, ServiceUUID, Characteristic, null,
            (address, characteristicUUID, bytes) =>
            {
                string receivedData = Encoding.UTF8.GetString(bytes);
                ShowDebug($"Partial Data Received: {receivedData}");

                // 조각난 데이터 병합
                partialData += receivedData;

                // 구분자를 기준으로 JSON 데이터 완성 확인
                if (partialData.Contains("<start>") && partialData.Contains("<end>"))
                {
                    int startIndex = partialData.IndexOf("<start>") + "<start>".Length;
                    int endIndex = partialData.IndexOf("<end>");
                    string completeJson = partialData.Substring(startIndex, endIndex - startIndex);

                    HM10_Status.text = $"Complete Data: {completeJson}";
                    ShowDebug($"Complete Data: {completeJson}");

                    // JSON 데이터 처리
                    ProcessSensorData(completeJson);

                    // 병합 데이터 초기화
                    partialData = "";
                }
            });

        SetState(States.None, 0f); // 대기 상태로 전환
        PanelMiddle.SetActive(true);
        HM10_Status.text = "Waiting...";
    }


    void ProcessSensorData(string jsonData)
    {
        SensorData.text = jsonData;
        try
        {
            JObject json = JObject.Parse(jsonData);

            if (json["light"] != null)
            {
                float lightValue = (float)json["light"];
                GameManager.Instance.sensorLight = lightValue;
                lightSlider.value = lightValue;
            }
            //else
            //{
            //    ShowDebug($"Complete Data: {GameManager.Instance.sensorLight}");
            //}

            if (json["soil"] != null)
            {
                float soilValue = (float)json["soil"];
                GameManager.Instance.sensorSoilMoisture = soilValue;
                soilMoistureSlider.value = soilValue;
            }
            //else
            //{
            //    ShowDebug($"Complete Data: {GameManager.Instance.sensorSoilMoisture}");
            //}

            if (json["temp"] != null)
            {
                float tempValue = (float)json["temp"];
                GameManager.Instance.sensorTemperature = tempValue;
                temperatureSlider.value = tempValue;
            }
            //else
            //{
            //    ShowDebug($"Complete Data: {GameManager.Instance.sensorTemperature}");
            //}

            if (json["humidity"] != null)
            {
                float humidityValue = (float)json["humidity"];
                GameManager.Instance.sensorHumidity = humidityValue;
                humiditySlider.value = humidityValue;
            }
            //else
            //{
            //    ShowDebug($"Complete Data: {GameManager.Instance.sensorHumidity}");
            //}

            if (json["pir"] != null)
            {
                float pirValue = (float)json["pir"];
                GameManager.Instance.sensorPIR = pirValue;
                pirSlider.value = pirValue;
            }
            //else
            //{
            //    ShowDebug($"Complete Data: {GameManager.Instance.sensorPIR}");
            //}
        }
        catch (System.Exception e)
        {
            ShowDebug($"Error parsing JSON: {e.Message}");
        }
    }

    // 데이터 전송
    public void SendString(string value)
    {
        if (!_connected)
        {
            ShowDebug("Not connected. Cannot send data.");
            return;
        }

        var data = Encoding.UTF8.GetBytes(value);
        BluetoothLEHardwareInterface.WriteCharacteristic(_hm10, ServiceUUID, Characteristic, data, data.Length, false, (characteristicUUID) =>
        {
            ShowDebug($"Data sent: {value}");
        });
    }

    // 장치와의 연결 해제
    void DisconnectFromDevice()
    {
        ShowDebug("Disconnecting...");
        if (_connected)
        {
            BluetoothLEHardwareInterface.DisconnectPeripheral(_hm10, (address) =>
            {
                ShowDebug($"Disconnected from: {address}");
                Reset();
            });
        }
    }

    // 구독 해제
    void UnsubscribeFromDevice()
    {
        ShowDebug("Unsubscribing...");
        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_hm10, ServiceUUID, Characteristic, null);
        SetState(States.Disconnect, 2f);
    }

    // 상태 설정
    void SetState(States newState, float timeout)
    {
        ShowDebug($"State changing to {newState} with timeout {timeout}s");
        _state = newState;
        _timeout = timeout;
    }

    // 리셋
    void Reset()
    {
        _connected = false;
        _timeout = 0f;
        _state = States.None;
        _hm10 = null;
        PanelMiddle.SetActive(false);
        ShowDebug("State reset.");
    }

    // UUID 비교
    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = $"0000{uuid1}-0000-1000-8000-00805F9B34FB";
        if (uuid2.Length == 4)
            uuid2 = $"0000{uuid2}-0000-1000-8000-00805F9B34FB";

        return uuid1.ToUpper().Equals(uuid2.ToUpper());
    }

    // 디버그 메시지 표시
    void ShowDebug(string message)
    {
        if (debugText != null)
        {
            // 기존 텍스트가 너무 길면 오래된 텍스트 제거
            if (debugText.text.Length > 500)
            {
                debugText.text = debugText.text.Substring(debugText.text.Length - 500);
            }

            // 디버그 메시지 추가
            debugText.text += message + "\n";
            Canvas.ForceUpdateCanvases(); // UI 강제 갱신
        }
        else
        {
            Debug.LogWarning("debugText is not assigned in Inspector!");
        }

        Debug.Log(message);
    }
}
