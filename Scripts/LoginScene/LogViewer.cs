using UnityEngine;
using TMPro;

public class LogViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText; // TextMeshProUGUI ����
    private string logMessages = ""; // �α� �޽����� ������ ����
    private int maxLines = 20; // ǥ���� �ִ� �� ��

    void Awake()
    {
        // Unity �α� �̺�Ʈ ����
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        // Unity �α� �̺�Ʈ ����
        Application.logMessageReceived -= HandleLog;
    }

    // �α� �޽����� ó���ϴ� �޼���
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // �α� Ÿ�԰� �޽��� �߰�
        logMessages += $"[{type}] {logString}\n";

        // �� �� ����
        string[] lines = logMessages.Split('\n');
        if (lines.Length > maxLines)
        {
            logMessages = string.Join("\n", lines, lines.Length - maxLines, maxLines);
        }

        // TextMeshProUGUI�� �α� ǥ��
        if (logText != null)
        {
            logText.text = logMessages;
        }
    }
}
