//using UnityEngine;
//using TMPro;  // TextMesh Pro 네임스페이스
//using BackEnd;  // 뒤끝 SDK 네임스페이스
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.SceneManagement;
//public class LoginSceneManager : MonoBehaviour
//{
//    private static LoginSceneManager _instance;

//    public static LoginSceneManager Instance
//    {
//        get
//        {
//            return _instance;
//        }
//    }

//    // ID와 PW InputField 연결
//    [SerializeField] private TMP_InputField inputFieldID;
//    [SerializeField] private TMP_InputField inputFieldPW;

//    // 버튼 및 텍스트 필드 연결
//    [SerializeField] private Button buttonSignIn;
//    [SerializeField] private TMP_Text signInStatusText;  // 상태 메시지 표시용 텍스트

//    void Awake()
//    {
//        _instance = this;  // 싱글톤 패턴을 사용할 때 인스턴스를 설정
//        var bro = Backend.Initialize(); // 뒤끝 초기화

//        // 뒤끝 초기화에 대한 응답값
//        if (bro.IsSuccess())
//        {
//            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
//        }
//        else
//        {
//            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
//        }

//    }

//    void Start()
//    {
//        // 필드가 null인지 체크하여 디버그 로그로 확인
//        if (buttonSignIn == null) Debug.LogError("buttonSignIn is not assigned in the inspector.");
//        if (inputFieldID == null) Debug.LogError("inputFieldID is not assigned in the inspector.");
//        if (inputFieldPW == null) Debug.LogError("inputFieldPW is not assigned in the inspector.");
//        if (signInStatusText == null) Debug.LogError("signInStatusText is not assigned in the inspector.");

//        // 로그인 버튼 클릭 이벤트 추가
//        if (buttonSignIn != null)
//        {
//            buttonSignIn.onClick.AddListener(OnLoginButtonClicked);
//        }
//        // 로그인 버튼 클릭 이벤트 추가
//        buttonSignIn.onClick.AddListener(OnLoginButtonClicked);
//    }

//    // 로그인 버튼 클릭 시 호출되는 함수
//    public void OnLoginButtonClicked()
//    {
//        string id = inputFieldID.text;  // 입력된 ID
//        string password = inputFieldPW.text;  // 입력된 비밀번호

//        // 입력 값이 유효한지 확인
//        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
//        {
//            signInStatusText.text = "아이디와 비밀번호를 입력하세요.";
//            return;
//        }

//        // 뒤끝 커스텀 로그인 호출
//        Backend.BMember.CustomLogin(id, password, callback =>
//        {
//            if (callback.IsSuccess())
//            {
//                signInStatusText.text = "로그인 성공!";
//                Debug.Log("로그인 성공: " + callback);

//                // 로그인 성공 후, 필요한 작업 수행 (예: 다음 씬으로 이동)
//                GoToNextScene();
//            }
//            else
//            {
//                signInStatusText.text = "로그인 실패: " + callback.GetMessage();
//                Debug.LogError("로그인 실패: " + callback);
//            }
//        });
//    }

//    // 로그인 성공 시 다음 씬으로 이동하는 함수
//    private void GoToNextScene()
//    {
//        SceneManager.LoadScene("SampleScene");
//        Debug.Log("다음 씬으로 이동");
//    }
//}  // 클래스의 마지막 닫는 중괄호
using UnityEngine;
using TMPro;  // TextMesh Pro 네임스페이스
using BackEnd;  // 뒤끝 SDK 네임스페이스
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class LoginSceneManager : MonoBehaviour
{
    private static LoginSceneManager _instance;

    public static LoginSceneManager Instance
    {
        get
        {
            return _instance;
        }
    }

    // ID와 PW InputField 연결
    [SerializeField] private TMP_InputField inputFieldID;
    [SerializeField] private TMP_InputField inputFieldPW;

    // 버튼 및 텍스트 필드 연결
    [SerializeField] private Button buttonSignIn;
    [SerializeField] private TMP_Text signInStatusText;  // 상태 메시지 표시용 텍스트

    void Awake()
    {
        _instance = this;  // 싱글톤 패턴을 사용할 때 인스턴스를 설정
        var bro = Backend.Initialize(); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }

    }

    void Start()
    {
        Backend.Utils.GetGoogleHash();

        //example
        string googlehash = Backend.Utils.GetGoogleHash();

        Debug.Log("구글 해시 키 : " + googlehash);

        // 필드가 null인지 체크하여 디버그 로그로 확인
        if (buttonSignIn == null) Debug.LogError("buttonSignIn is not assigned in the inspector.");
        if (inputFieldID == null) Debug.LogError("inputFieldID is not assigned in the inspector.");
        if (inputFieldPW == null) Debug.LogError("inputFieldPW is not assigned in the inspector.");
        if (signInStatusText == null) Debug.LogError("signInStatusText is not assigned in the inspector.");

        // 로그인 버튼 클릭 이벤트 추가
        if (buttonSignIn != null)
        {
            buttonSignIn.onClick.AddListener(OnLoginButtonClicked);
        }
        // 로그인 버튼 클릭 이벤트 추가
        buttonSignIn.onClick.AddListener(OnLoginButtonClicked);
    }

    // 로그인 버튼 클릭 시 호출되는 함수
    public void OnLoginButtonClicked()
    {
        string id = inputFieldID.text;  // 입력된 ID
        string password = inputFieldPW.text;  // 입력된 비밀번호

        // 입력 값이 유효한지 확인
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            signInStatusText.text = "아이디와 비밀번호를 입력하세요.";
            return;
        }

        // 뒤끝 커스텀 로그인 호출
        Backend.BMember.CustomLogin(id, password, callback =>
        {
            if (callback.IsSuccess())
            {
                signInStatusText.text = "로그인 성공!";
                Debug.Log("로그인 성공: " + callback);
                SceneTransitionManager.LoadSceneWithDelay("MainScene", 0.5f);
            }
            else
            {
                signInStatusText.text = "로그인 실패: " + callback.GetMessage();
                Debug.LogError("로그인 실패: " + callback);
            }
        });
    }

    // 로그인 성공 시 다음 씬으로 이동하는 함수
    private void GoToNextScene()
    {
        SceneManager.LoadScene("MainScene");
        Debug.Log("다음 씬으로 이동");
    }
}  // 클래스의 마지막 닫는 중괄호
