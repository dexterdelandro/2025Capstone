using System.Collections;
using UnityEngine;

public class Ink : MonoBehaviour
{
    private Canvas popUpCanvas;
    public Transform player;  // 需要拖入 Player 对象
    public Camera mainCamera; // 需要拖入主摄像机
    public Vector3 offset = new Vector3(0, 2f, 0); // UI 偏移量

    void Start()
    {
        popUpCanvas = GetComponentInChildren<Canvas>(true); // true 代表查找被禁用的组件

        if (popUpCanvas == null)
        {
            Debug.LogError("Canvas component not found! Ensure the object has a child Canvas component.");
        }
        else
        {
            ShowPopUp(false); // 初始隐藏
        }
    }

    void Update()
    {
        if (popUpCanvas != null && player != null && mainCamera != null)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(player.position + offset);
            popUpCanvas.transform.position = screenPos;
        }
    }

    public void ShowPopUp(bool state)
    {
        if (popUpCanvas != null)
        {
            popUpCanvas.enabled = state;
        }
    }

    public void DestroySpirit()
    {
        ShowPopUp(false);
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}