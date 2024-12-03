using UnityEngine;
using TMPro;

public class LogViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText; // TextMeshProUGUI 연결
    private string logMessages = ""; // 로그 메시지를 저장할 변수
    private int maxLines = 20; // 표시할 최대 줄 수

    void Awake()
    {
        // Unity 로그 이벤트 연결
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        // Unity 로그 이벤트 해제
        Application.logMessageReceived -= HandleLog;
    }

    // 로그 메시지를 처리하는 메서드
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // 로그 타입과 메시지 추가
        logMessages += $"[{type}] {logString}\n";

        // 줄 수 제한
        string[] lines = logMessages.Split('\n');
        if (lines.Length > maxLines)
        {
            logMessages = string.Join("\n", lines, lines.Length - maxLines, maxLines);
        }

        // TextMeshProUGUI에 로그 표시
        if (logText != null)
        {
            logText.text = logMessages;
        }
    }
}
