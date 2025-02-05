using System.Collections;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    private Canvas popUpCanvas;

    void Start()
    {
        popUpCanvas = GetComponentInChildren<Canvas>();
        if (popUpCanvas == null)
        {
            Debug.LogError("Canvas component not found. Ensure the object has a child with a Canvas component.");
        }
        ShowPopUp(false); // 默认隐藏交互提示
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
