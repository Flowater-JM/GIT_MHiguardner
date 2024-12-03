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
    private float requiredTime = 5f; // �̼� ������ �����ϴ� �� �ʿ��� �ð� (5��)

    private const string Button1Key = "MissionButton1_LastCompleted";
    private const string Button2Key = "MissionButton2_LastCompleted";
    private const string Button3Key = "MissionButton3_LastCompleted";

    void Start()
    {
        // �ʱ� ��ư ���� ����
        missionButton1.interactable = CheckIfButtonCanBeActivated(Button1Key);
        missionButton2.interactable = CheckIfButtonCanBeActivated(Button2Key);
        missionButton3.interactable = CheckIfButtonCanBeActivated(Button3Key);

        GameManager.Instance.UpdateCoinUI();
    }

    void Update()
    {
        // ��ư ���� ������Ʈ
        missionButton1.interactable = isConditionMet1 && CheckIfButtonCanBeActivated(Button1Key);
        missionButton2.interactable = isConditionMet2 && CheckIfButtonCanBeActivated(Button2Key);
        missionButton3.interactable = isConditionMet3 && CheckIfButtonCanBeActivated(Button3Key);

        // �̼� ���� üũ
        CheckMissionCondition1();
        CheckMissionCondition2();
        CheckMissionCondition3();
    }

    // �̼� 1: ��� ���� ����
    void CheckMissionCondition1()
    {
        if (GameManager.Instance.sensorSoilMoisture >= 1f) // ��
        {
            soilCount += Time.deltaTime; // ������ �����ϴ� ���� �ð� ����
            if (soilCount >= requiredTime)
            {
                isConditionMet1 = true; // ���� ����
            }
        }
        else
        {
            soilCount = 0f; // ���� ������ �� �ð� �ʱ�ȭ
            isConditionMet1 = false;
        }
    }

    // �̼� 2: PIR ���� ���� ����
    void CheckMissionCondition2()
    {
        if (GameManager.Instance.sensorPIR == 1) // PIR ������ 1�� ��ȯ�ϸ�
        {
            pirCount++; // ���� Ƚ�� ����
            if (pirCount >= 5) // 5ȸ �̻� �����Ǹ� ���� ����
            {
                isConditionMet2 = true;
            }
        }
    }

    // �̼� 3: ���� ����
    void CheckMissionCondition3()
    {
        if (GameManager.Instance.sensorLight >= 10f) // ��
        {
            lightCount += Time.deltaTime; // ������ �����ϴ� ���� �ð� ����
            if (lightCount >= requiredTime)
            {
                isConditionMet3 = true; // ���� ����
            }
        }
        else
        {
            lightCount = 0f; // ���� ������ �� �ð� �ʱ�ȭ
            isConditionMet3 = false;
        }
    }

    // �̼� �Ϸ� ��ư Ŭ��
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

    // �̼� �Ϸ� ó��
    void CompleteMission(string buttonKey, Button button)
    {
        RewardPlayer(20, 10); // ���� ����
        button.interactable = false; // ��ư ��Ȱ��ȭ
        PlayerPrefs.SetString(buttonKey, DateTime.Now.ToString("yyyy-MM-dd")); // �Ϸ� ��¥ ����
        PlayerPrefs.Save();
    }

    // ��ư Ȱ��ȭ ���� ���� Ȯ��
    bool CheckIfButtonCanBeActivated(string buttonKey)
    {
        if (PlayerPrefs.HasKey(buttonKey))
        {
            string lastCompletedDate = PlayerPrefs.GetString(buttonKey);
            DateTime lastCompleted;
            if (DateTime.TryParse(lastCompletedDate, out lastCompleted))
            {
                // ������ �Ϸ� ��¥�� ���� �����̸� ��ư Ȱ��ȭ
                return lastCompleted.Date < DateTime.Now.Date;
            }
        }
        return true; // ����� �����Ͱ� ������ Ȱ��ȭ
    }

    // �÷��̾� ���� ����
    void RewardPlayer(int coins, int exp)
    {
        GameManager.Instance.playerCoins += coins; // ���� �߰�
        GameManager.Instance.playerExperience += exp;
        GameManager.Instance.SaveGame(); // ����
        GameManager.Instance.UpdateCoinUI(); // UI ������Ʈ
    }
}
