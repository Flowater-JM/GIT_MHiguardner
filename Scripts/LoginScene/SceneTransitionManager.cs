using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // SceneManager 네임스페이스 추가

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
        Destroy(gameObject); // 씬 전환 매니저 파괴
    }
}
