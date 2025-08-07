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