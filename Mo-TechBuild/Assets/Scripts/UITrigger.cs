using UnityEngine;

public class UITrigger : MonoBehaviour
{
    [Header("UI")]
    public Collider triggerZone; // 触发区域
    public GameObject uiPanel;   // 关联的 UI 界面

    [Header("Button")]
    public string openButton = "joystick button 0";
    public string closeButton = "joystick button 8"; 

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
            
            if (Input.GetKeyDown(openButton) && !isUIVisible)
            {
                ShowUI();
            }

            
            if (Input.GetKeyDown(closeButton) && isUIVisible)
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
        if (triggerZone != null && other == triggerZone)
        {
            isPlayerInside = false;
            HideUI(); // 离开时自动隐藏 UI
        }
    }
}