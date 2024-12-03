using UnityEngine;

public class PlantAnimationController : MonoBehaviour
{
    public Animator animator;

    // ���� ���� ����
    public float optimalTemperatureMin = 20f;
    public float optimalTemperatureMax = 25f;
    public float optimalLightLevel = 500f;
    public float optimalHumidity = 50f;
    public float optimalSoilMoisture = 30f;
    public float maxSoilMoisture = 80f;

    void Update()
    {
        // GameManager���� ���� ������ ��������
        float temperature = GameManager.Instance.sensorTemperature;
        float lightLevel = GameManager.Instance.sensorLight;
        float humidity = GameManager.Instance.sensorHumidity;
        float soilMoisture = GameManager.Instance.sensorSoilMoisture;
        bool proximitySensor = GameManager.Instance.sensorPIR > 0; // PIR ���� 0���� ũ�� true�� ó��

        // Happy1: ���� ����
        if (temperature >= optimalTemperatureMin && temperature <= optimalTemperatureMax &&
            lightLevel >= optimalLightLevel &&
            humidity >= optimalHumidity &&
            soilMoisture >= optimalSoilMoisture && soilMoisture <= maxSoilMoisture)
        {
            SetAnimationState("isHappy1", true);
        }
        else
        {
            SetAnimationState("isHappy1", false);
        }

        // Happy3: ���� ���� �۵�
        if (proximitySensor)
        {
            animator.SetTrigger("isHappy3");
        }

        // Sad: ������ ���ų�, ������ ���� ������ �ƴ� ��, ���� �ʹ� ���� ��
        if (lightLevel < optimalLightLevel || humidity < optimalHumidity || soilMoisture > maxSoilMoisture)
        {
            SetAnimationState("isSad", true);
        }
        else
        {
            SetAnimationState("isSad", false);
        }

        // Hot: �µ��� ���ų� ��� ������ ���� ��
        if (temperature > optimalTemperatureMax || soilMoisture < optimalSoilMoisture)
        {
            SetAnimationState("isHot", true);
        }
        else
        {
            SetAnimationState("isHot", false);
        }

        // Cold: �µ��� ���� ��
        if (temperature < optimalTemperatureMin)
        {
            SetAnimationState("isCold", true);
        }
        else
        {
            SetAnimationState("isCold", false);
        }
    }

    // �ִϸ��̼� ���� ���� �Լ�
    void SetAnimationState(string parameter, bool state)
    {
        if (animator.GetBool(parameter) != state)
        {
            animator.SetBool(parameter, state);
        }
    }
}
