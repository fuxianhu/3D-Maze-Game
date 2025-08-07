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
            helpText.SetActive(!helpText.activeSelf); // 切换显示/隐藏
        }
    }

    void ToggleFullScreen()
    {
#if UNITY_EDITOR
    // 编辑器专用：切换 Game 视图最大化
    var assembly = typeof(UnityEditor.EditorWindow).Assembly;
    var type = assembly.GetType("UnityEditor.GameView");
    var gameView = UnityEditor.EditorWindow.GetWindow(type);
    gameView.maximized = !gameView.maximized;
#else
        // 真机逻辑
        Screen.fullScreen = !Screen.fullScreen;
#endif
    }
}