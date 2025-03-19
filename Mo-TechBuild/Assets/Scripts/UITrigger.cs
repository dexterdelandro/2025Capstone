using UnityEngine;

public class UITrigger : MonoBehaviour
{
    [Header("UI")]
    public Collider triggerZone; // 触发区域
    public GameObject uiPanel;   // 关联的 UI 界面

    [Header("Button")]
    public string openButton = "joystick button 0"; // 打开 UI 的按键
    public string closeButton = "joystick button 8"; // 关闭 UI 的按键
    public KeyCode toggleKey = KeyCode.E; // 额外的键盘按键（E 键）

    private bool isPlayerInside = false; // 是否在触发区域内
    private bool isUIVisible = false; // UI 是否可见

    void Start()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false); // 初始隐藏 UI
        }
    }

    void Update()
    {
        if (isPlayerInside)
        {
            if ((Input.GetKeyDown(openButton) || Input.GetKeyDown(toggleKey)) && !isUIVisible)
            {
                ShowUI();
            }
            else if ((Input.GetKeyDown(closeButton) || Input.GetKeyDown(toggleKey)) && isUIVisible)
            {
                HideUI();
            }
        }
    }

    void ShowUI()
    {
        if (!isUIVisible && uiPanel != null)
        {
            uiPanel.SetActive(true);
            isUIVisible = true;
        }
    }

    void HideUI()
    {
        if (isUIVisible && uiPanel != null)
        {
            uiPanel.SetActive(false);
            isUIVisible = false;
        }
    }

    // 进入触发区域
    private void OnTriggerEnter(Collider other)
    {
        if (triggerZone != null && other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    // 离开触发区域
    private void OnTriggerExit(Collider other)
    {
        if (triggerZone != null && other.CompareTag("Player"))
        {
            isPlayerInside = false;
            HideUI(); // 离开时自动隐藏 UI
        }
    }
}
