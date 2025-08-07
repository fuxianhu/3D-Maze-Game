using UnityEngine;

public class FullScreenToggle : MonoBehaviour
{
    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) &&
            Input.GetKeyDown(KeyCode.Return))
        {
            ToggleFullScreen();
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