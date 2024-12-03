using UnityEngine;

public class PlantAnimationController : MonoBehaviour
{
    public Animator animator;

    // 적정 범위 설정
    public float optimalTemperatureMin = 20f;
    public float optimalTemperatureMax = 25f;
    public float optimalLightLevel = 500f;
    public float optimalHumidity = 50f;
    public float optimalSoilMoisture = 30f;
    public float maxSoilMoisture = 80f;

    void Update()
    {
        // GameManager에서 센서 데이터 가져오기
        float temperature = GameManager.Instance.sensorTemperature;
        float lightLevel = GameManager.Instance.sensorLight;
        float humidity = GameManager.Instance.sensorHumidity;
        float soilMoisture = GameManager.Instance.sensorSoilMoisture;
        bool proximitySensor = GameManager.Instance.sensorPIR > 0; // PIR 값이 0보다 크면 true로 처리

        // Happy1: 적정 조건
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

        // Happy3: 접근 센서 작동
        if (proximitySensor)
        {
            animator.SetTrigger("isHappy3");
        }

        // Sad: 조도가 낮거나, 습도가 적정 범위가 아닐 때, 물이 너무 많을 때
        if (lightLevel < optimalLightLevel || humidity < optimalHumidity || soilMoisture > maxSoilMoisture)
        {
            SetAnimationState("isSad", true);
        }
        else
        {
            SetAnimationState("isSad", false);
        }

        // Hot: 온도가 높거나 토양 습도가 낮을 때
        if (temperature > optimalTemperatureMax || soilMoisture < optimalSoilMoisture)
        {
            SetAnimationState("isHot", true);
        }
        else
        {
            SetAnimationState("isHot", false);
        }

        // Cold: 온도가 낮을 때
        if (temperature < optimalTemperatureMin)
        {
            SetAnimationState("isCold", true);
        }
        else
        {
            SetAnimationState("isCold", false);
        }
    }

    // 애니메이션 상태 설정 함수
    void SetAnimationState(string parameter, bool state)
    {
        if (animator.GetBool(parameter) != state)
        {
            animator.SetBool(parameter, state);
        }
    }
}
