using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System;
using TMPro;


public class BackendCoupon : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputFieldText;     //입력값
    [SerializeField]
    private Button btnCode;        //확인 버튼
    [SerializeField]
    private TextMeshProUGUI textMessage;        //틀렸을 때 나오는 메시지

    // 씨앗 텍스트와 캐릭터 이미지 GameObject를 드래그로 연결
    [SerializeField]
    private TextMeshProUGUI seedText;         // 씨앗 텍스트 오브젝트
    [SerializeField]
    private Button characterButton;          // 캐릭터 이미지 오브젝트
    [SerializeField]
    private Sprite newCharacterSprite;       // 변경할 새 캐릭터 이미지

    // 쿠폰 패널 오브젝트
    [SerializeField]
    private GameObject couponPanel;          // 쿠폰 입력 패널 오브젝트

    private static BackendCoupon _instance = null;
    public static BackendCoupon Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendCoupon();
            }

            return _instance;
        }
    }

    // 쿠폰 성공 시 실행할 함수
    private void Func1()
    {
        // 씨앗 텍스트를 새싹으로 변경
        if (seedText != null)
        {
            seedText.text = "새싹";
            Debug.Log("Seed text successfully changed to 새싹.");
        }
        else
        {
            Debug.LogWarning("Seed text is not assigned.");
        }

        // 캐릭터 이미지를 변경
        if (characterButton != null && newCharacterSprite != null)
        {
            characterButton.image.sprite = newCharacterSprite;
            Debug.Log("Character image successfully changed to new sprite.");
        }
        else
        {
            Debug.LogWarning("Character button or new character sprite is not assigned.");
        }

        // 쿠폰 패널을 비활성화
        if (couponPanel != null)
        {
            couponPanel.SetActive(false);
            Debug.Log("Coupon panel successfully hidden.");
        }
        else
        {
            Debug.LogWarning("Coupon panel is not assigned.");
        }
    }

    //쿠폰 사용 함수 --> 올바른 쿠폰 입력했을 때 실행시킬 함수 포함
    public void CouponUse(string couponNumber)
    {
        var bro = Backend.Coupon.UseCoupon(couponNumber);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("쿠폰 사용 중 에러가 발생했습니다. : " + bro);
            textMessage.text = "쿠폰 번호가 올바르지 않습니다.";
            btnCode.interactable = true;
            return;
        }

        Debug.Log("쿠폰 사용에 성공했습니다. : " + bro);

        Func1();
    }

    //버튼 함수
    public void OnClickCode()
    {
        //UI 초기화
        textMessage.text = string.Empty;

        btnCode.interactable = false;

        if (inputFieldText.text.Trim() == "")
        {
            textMessage.text = "코드를 입력해주세요";

            inputFieldText.text = string.Empty;
            btnCode.interactable = true;
        }
        else CouponUse(inputFieldText.text);
    }
};

//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;
//using UnityEngine.UI;
//using BackEnd;
//using System;
//using TMPro;


//public class BackendCoupon : MonoBehaviour
//{
//    [SerializeField]
//    private TMP_InputField inputFieldText;     //입력값
//    [SerializeField]
//    private Button btnCode;        //확인 버튼
//    [SerializeField]
//    private TextMeshProUGUI textMessage;        //틀렸을 때 나오는 메시지

//    // 씨앗 텍스트와 캐릭터 이미지 GameObject를 드래그로 연결
//    [SerializeField]
//    private TextMeshProUGUI seedText;         // 씨앗 텍스트 오브젝트
//    [SerializeField]
//    private Button characterButton;          // 캐릭터 이미지 오브젝트
//    [SerializeField]
//    private Sprite newCharacterSprite;       // 변경할 새 캐릭터 이미지

//    // 쿠폰 패널 오브젝트
//    [SerializeField]
//    private GameObject couponPanel;          // 쿠폰 입력 패널 오브젝트

//    // 패널 비활성화 이후 버튼을 다시 활성화하지 않음
//    public void OnClickCode()
//    {
//        // UI 초기화
//        textMessage.text = string.Empty;
//        btnCode.interactable = false;

//        // 입력값이 없을 경우 처리
//        if (string.IsNullOrWhiteSpace(inputFieldText.text))
//        {
//            textMessage.text = "코드를 입력해주세요.";
//            inputFieldText.text = string.Empty;
//            btnCode.interactable = true; // 입력값이 없을 때는 다시 활성화
//            return;
//        }

//        // 입력값이 있으면 아래 로직 실행
//        Debug.Log($"입력된 쿠폰 코드: {inputFieldText.text}");

//        // '새싹' 텍스트로 변경
//        seedText.text = "새싹";

//        // 캐릭터 이미지 변경
//        if (characterButton != null && newCharacterSprite != null)
//        {
//            characterButton.image.sprite = newCharacterSprite;
//            Debug.Log("캐릭터 이미지가 변경되었습니다.");
//        }

//        // 쿠폰 패널 비활성화
//        if (couponPanel != null)
//        {
//            couponPanel.SetActive(false);
//            Debug.Log("쿠폰 입력 패널이 비활성화되었습니다.");
//        }

//        // 버튼 활성화 코드를 제거
//    }
//}
