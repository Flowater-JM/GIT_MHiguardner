using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // SceneManager ���ӽ����̽� �߰�

public class SceneTransitionManager : MonoBehaviour
{
    public static void LoadSceneWithDelay(string sceneName, float delay)
    {
        var transitionManager = new GameObject("SceneTransitionManager").AddComponent<SceneTransitionManager>();
        transitionManager.StartCoroutine(transitionManager.LoadSceneCoroutine(sceneName, delay));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
        Destroy(gameObject); // �� ��ȯ �Ŵ��� �ı�
    }
}
