using System;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public Button missionButton1;
    public Button missionButton2;
    public Button missionButton3;

    private bool isConditionMet1 = false;
    private bool isConditionMet2 = false;
    private bool isConditionMet3 = false;

    private float soilCount = 0f;
    private float pirCount = 0f;
    private float lightCount = 0f;
    private float requiredTime = 5f; // 미션 조건을 충족하는 데 필요한 시간 (5초)

    private const string Button1Key = "MissionButton1_LastCompleted";
    private const string Button2Key = "MissionButton2_LastCompleted";
    private const string Button3Key = "MissionButton3_LastCompleted";

    void Start()
    {
        // 초기 버튼 상태 설정
        missionButton1.interactable = CheckIfButtonCanBeActivated(Button1Key);
        missionButton2.interactable = CheckIfButtonCanBeActivated(Button2Key);
        missionButton3.interactable = CheckIfButtonCanBeActivated(Button3Key);

        GameManager.Instance.UpdateCoinUI();
    }

    void Update()
    {
        // 버튼 상태 업데이트
        missionButton1.interactable = isConditionMet1 && CheckIfButtonCanBeActivated(Button1Key);
        missionButton2.interactable = isConditionMet2 && CheckIfButtonCanBeActivated(Button2Key);
        missionButton3.interactable = isConditionMet3 && CheckIfButtonCanBeActivated(Button3Key);

        // 미션 조건 체크
        CheckMissionCondition1();
        CheckMissionCondition2();
        CheckMissionCondition3();
    }

    // 미션 1: 토양 수분 조건
    void CheckMissionCondition1()
    {
        if (GameManager.Instance.sensorSoilMoisture >= 1f) // 값
        {
            soilCount += Time.deltaTime; // 조건을 충족하는 동안 시간 누적
            if (soilCount >= requiredTime)
            {
                isConditionMet1 = true; // 조건 충족
            }
        }
        else
        {
            soilCount = 0f; // 조건 미충족 시 시간 초기화
            isConditionMet1 = false;
        }
    }

    // 미션 2: PIR 센서 감지 조건
    void CheckMissionCondition2()
    {
        if (GameManager.Instance.sensorPIR == 1) // PIR 센서가 1을 반환하면
        {
            pirCount++; // 감지 횟수 증가
            if (pirCount >= 5) // 5회 이상 감지되면 조건 충족
            {
                isConditionMet2 = true;
            }
        }
    }

    // 미션 3: 조도 조건
    void CheckMissionCondition3()
    {
        if (GameManager.Instance.sensorLight >= 10f) // 값
        {
            lightCount += Time.deltaTime; // 조건을 충족하는 동안 시간 누적
            if (lightCount >= requiredTime)
            {
                isConditionMet3 = true; // 조건 충족
            }
        }
        else
        {
            lightCount = 0f; // 조건 미충족 시 시간 초기화
            isConditionMet3 = false;
        }
    }

    // 미션 완료 버튼 클릭
    public void OnMissionButtonClick(Button button)
    {
        if (button == missionButton1 && isConditionMet1)
        {
            CompleteMission(Button1Key, button);
        }
        else if (button == missionButton2 && isConditionMet2)
        {
            CompleteMission(Button2Key, button);
        }
        else if (button == missionButton3 && isConditionMet3)
        {
            CompleteMission(Button3Key, button);
        }
    }

    // 미션 완료 처리
    void CompleteMission(string buttonKey, Button button)
    {
        RewardPlayer(20, 10); // 보상 지급
        button.interactable = false; // 버튼 비활성화
        PlayerPrefs.SetString(buttonKey, DateTime.Now.ToString("yyyy-MM-dd")); // 완료 날짜 저장
        PlayerPrefs.Save();
    }

    // 버튼 활성화 가능 여부 확인
    bool CheckIfButtonCanBeActivated(string buttonKey)
    {
        if (PlayerPrefs.HasKey(buttonKey))
        {
            string lastCompletedDate = PlayerPrefs.GetString(buttonKey);
            DateTime lastCompleted;
            if (DateTime.TryParse(lastCompletedDate, out lastCompleted))
            {
                // 마지막 완료 날짜가 오늘 이전이면 버튼 활성화
                return lastCompleted.Date < DateTime.Now.Date;
            }
        }
        return true; // 저장된 데이터가 없으면 활성화
    }

    // 플레이어 보상 지급
    void RewardPlayer(int coins, int exp)
    {
        GameManager.Instance.playerCoins += coins; // 코인 추가
        GameManager.Instance.playerExperience += exp;
        GameManager.Instance.SaveGame(); // 저장
        GameManager.Instance.UpdateCoinUI(); // UI 업데이트
    }
}
