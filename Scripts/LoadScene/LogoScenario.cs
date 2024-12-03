//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class LogoScenario : MonoBehaviour
//{
//	[SerializeField]
//	private	Progress	progress;


//	private void Awake()
//	{
//		SystemSetup();
//	}

//	private void SystemSetup()
//	{
//		// Ȱ��ȭ���� ���� ���¿����� ������ ��� ����
//		Application.runInBackground = true;

//		// �ػ� ���� (5:8, 960x600, ������ ��Ʈ 8)
//		int width	= Screen.width;
//		int height	= (int)(Screen.width * 8f / 5);
//		Screen.SetResolution(width, height, true);

//		// ȭ���� ������ �ʵ��� ����
//		Screen.sleepTimeout = SleepTimeout.NeverSleep;

//		// �ε� �ִϸ��̼� ����, ��� �Ϸ�� OnAfterProgress() �޼ҵ� ����
//		progress.Play(OnAfterProgress);
//	}

//	private void OnAfterProgress()
//	{
//		SceneManager.LoadScene("LoginScene");


//	}
//}
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScenario : MonoBehaviour
{
    [SerializeField]
    private Progress progress;


    private void Awake()
    {
        SystemSetup();
    }

    private void SystemSetup()
    {
        // Ȱ��ȭ���� ���� ���¿����� ������ ��� ����
        Application.runInBackground = true;

        // �ػ� ���� (5:8, 960x600, ������ ��Ʈ 8)
        //int width = Screen.width;
        //int height = (int)(Screen.width * 8f / 5);
        //Screen.SetResolution(width, height, true);

        // ȭ���� ������ �ʵ��� ����
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // �ε� �ִϸ��̼� ����, ��� �Ϸ�� OnAfterProgress() �޼ҵ� ����
        progress.Play(OnAfterProgress);
    }

    private void OnAfterProgress()
    {
        SceneManager.LoadScene("LoginScene");


    }
}


