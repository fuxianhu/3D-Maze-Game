using UnityEngine;
using TMPro;


public class KeyCheck : MonoBehaviour
{
    public GameObject helpText;

    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) &&
            Input.GetKeyDown(KeyCode.Return))
        {
            ToggleFullScreen();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            helpText.SetActive(!helpText.activeSelf); // �л���ʾ/����
        }
    }

    void ToggleFullScreen()
    {
#if UNITY_EDITOR
    // �༭��ר�ã��л� Game ��ͼ���
    var assembly = typeof(UnityEditor.EditorWindow).Assembly;
    var type = assembly.GetType("UnityEditor.GameView");
    var gameView = UnityEditor.EditorWindow.GetWindow(type);
    gameView.maximized = !gameView.maximized;
#else
        // ����߼�
        Screen.fullScreen = !Screen.fullScreen;
#endif
    }
}