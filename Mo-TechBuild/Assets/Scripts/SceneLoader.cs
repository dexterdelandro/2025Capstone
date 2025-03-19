using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;  // 加载界面
    public Image progressBar;  // 进度条
    public Text progressText;  // 显示加载进度的文本

    private void Update()
    {
        // 检测手柄的 X 按钮是否被按下
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            StartCoroutine(LoadSceneAsync("Level GDC"));
        }
    }

    public void jump()
    {
        StartCoroutine(LoadSceneAsync("Level GDC"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // 显示加载界面
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // 开始异步加载
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // 让加载完成后等待激活

        // 更新加载进度
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // 进度归一化到 0-1

            // 更新进度条
            if (progressBar != null)
            {
                progressBar.fillAmount = progress;
            }

            // 更新进度数字
            if (progressText != null)
            {
                progressText.text = (progress * 100).ToString("F0") + "%"; // 例如 75%
            }

            // 场景加载到 90% 以上才允许进入（防止黑屏）
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}