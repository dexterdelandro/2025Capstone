using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI; // 在 Inspector 中拖入你的 UI 物体
    private bool isPaused = false;

    void Update()
    {
        // 检测 PS 手柄的 Option 键（通常为 JoystickButton9）
        if (Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // 显示 UI 并暂停游戏
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f; // 暂停游戏
        }
        else
        {
            // 关闭 UI 并恢复游戏
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f; // 恢复游戏
        }
    }
}
