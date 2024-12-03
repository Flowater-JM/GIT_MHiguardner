//using UnityEngine;
//using TMPro;  // TextMesh Pro ���ӽ����̽�
//using BackEnd;  // �ڳ� SDK ���ӽ����̽�
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

//    // ID�� PW InputField ����
//    [SerializeField] private TMP_InputField inputFieldID;
//    [SerializeField] private TMP_InputField inputFieldPW;

//    // ��ư �� �ؽ�Ʈ �ʵ� ����
//    [SerializeField] private Button buttonSignIn;
//    [SerializeField] private TMP_Text signInStatusText;  // ���� �޽��� ǥ�ÿ� �ؽ�Ʈ

//    void Awake()
//    {
//        _instance = this;  // �̱��� ������ ����� �� �ν��Ͻ��� ����
//        var bro = Backend.Initialize(); // �ڳ� �ʱ�ȭ

//        // �ڳ� �ʱ�ȭ�� ���� ���䰪
//        if (bro.IsSuccess())
//        {
//            Debug.Log("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
//        }
//        else
//        {
//            Debug.LogError("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻�
//        }

//    }

//    void Start()
//    {
//        // �ʵ尡 null���� üũ�Ͽ� ����� �α׷� Ȯ��
//        if (buttonSignIn == null) Debug.LogError("buttonSignIn is not assigned in the inspector.");
//        if (inputFieldID == null) Debug.LogError("inputFieldID is not assigned in the inspector.");
//        if (inputFieldPW == null) Debug.LogError("inputFieldPW is not assigned in the inspector.");
//        if (signInStatusText == null) Debug.LogError("signInStatusText is not assigned in the inspector.");

//        // �α��� ��ư Ŭ�� �̺�Ʈ �߰�
//        if (buttonSignIn != null)
//        {
//            buttonSignIn.onClick.AddListener(OnLoginButtonClicked);
//        }
//        // �α��� ��ư Ŭ�� �̺�Ʈ �߰�
//        buttonSignIn.onClick.AddListener(OnLoginButtonClicked);
//    }

//    // �α��� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
//    public void OnLoginButtonClicked()
//    {
//        string id = inputFieldID.text;  // �Էµ� ID
//        string password = inputFieldPW.text;  // �Էµ� ��й�ȣ

//        // �Է� ���� ��ȿ���� Ȯ��
//        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
//        {
//            signInStatusText.text = "���̵�� ��й�ȣ�� �Է��ϼ���.";
//            return;
//        }

//        // �ڳ� Ŀ���� �α��� ȣ��
//        Backend.BMember.CustomLogin(id, password, callback =>
//        {
//            if (callback.IsSuccess())
//            {
//                signInStatusText.text = "�α��� ����!";
//                Debug.Log("�α��� ����: " + callback);

//                // �α��� ���� ��, �ʿ��� �۾� ���� (��: ���� ������ �̵�)
//                GoToNextScene();
//            }
//            else
//            {
//                signInStatusText.text = "�α��� ����: " + callback.GetMessage();
//                Debug.LogError("�α��� ����: " + callback);
//            }
//        });
//    }

//    // �α��� ���� �� ���� ������ �̵��ϴ� �Լ�
//    private void GoToNextScene()
//    {
//        SceneManager.LoadScene("SampleScene");
//        Debug.Log("���� ������ �̵�");
//    }
//}  // Ŭ������ ������ �ݴ� �߰�ȣ
using UnityEngine;
using TMPro;  // TextMesh Pro ���ӽ����̽�
using BackEnd;  // �ڳ� SDK ���ӽ����̽�
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

    // ID�� PW InputField ����
    [SerializeField] private TMP_InputField inputFieldID;
    [SerializeField] private TMP_InputField inputFieldPW;

    // ��ư �� �ؽ�Ʈ �ʵ� ����
    [SerializeField] private Button buttonSignIn;
    [SerializeField] private TMP_Text signInStatusText;  // ���� �޽��� ǥ�ÿ� �ؽ�Ʈ

    void Awake()
    {
        _instance = this;  // �̱��� ������ ����� �� �ν��Ͻ��� ����
        var bro = Backend.Initialize(); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻�
        }

    }

    void Start()
    {
        Backend.Utils.GetGoogleHash();

        //example
        string googlehash = Backend.Utils.GetGoogleHash();

        Debug.Log("���� �ؽ� Ű : " + googlehash);

        // �ʵ尡 null���� üũ�Ͽ� ����� �α׷� Ȯ��
        if (buttonSignIn == null) Debug.LogError("buttonSignIn is not assigned in the inspector.");
        if (inputFieldID == null) Debug.LogError("inputFieldID is not assigned in the inspector.");
        if (inputFieldPW == null) Debug.LogError("inputFieldPW is not assigned in the inspector.");
        if (signInStatusText == null) Debug.LogError("signInStatusText is not assigned in the inspector.");

        // �α��� ��ư Ŭ�� �̺�Ʈ �߰�
        if (buttonSignIn != null)
        {
            buttonSignIn.onClick.AddListener(OnLoginButtonClicked);
        }
        // �α��� ��ư Ŭ�� �̺�Ʈ �߰�
        buttonSignIn.onClick.AddListener(OnLoginButtonClicked);
    }

    // �α��� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnLoginButtonClicked()
    {
        string id = inputFieldID.text;  // �Էµ� ID
        string password = inputFieldPW.text;  // �Էµ� ��й�ȣ

        // �Է� ���� ��ȿ���� Ȯ��
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            signInStatusText.text = "���̵�� ��й�ȣ�� �Է��ϼ���.";
            return;
        }

        // �ڳ� Ŀ���� �α��� ȣ��
        Backend.BMember.CustomLogin(id, password, callback =>
        {
            if (callback.IsSuccess())
            {
                signInStatusText.text = "�α��� ����!";
                Debug.Log("�α��� ����: " + callback);
                SceneTransitionManager.LoadSceneWithDelay("MainScene", 0.5f);
            }
            else
            {
                signInStatusText.text = "�α��� ����: " + callback.GetMessage();
                Debug.LogError("�α��� ����: " + callback);
            }
        });
    }

    // �α��� ���� �� ���� ������ �̵��ϴ� �Լ�
    private void GoToNextScene()
    {
        SceneManager.LoadScene("MainScene");
        Debug.Log("���� ������ �̵�");
    }
}  // Ŭ������ ������ �ݴ� �߰�ȣ
