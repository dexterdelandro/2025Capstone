using UnityEngine;

public class PopupUITrigger : MonoBehaviour
{
    [Header("TriggerZone")]
    public Collider triggerZone;

    [Header("UI")]
    public GameObject targetObject;

    private void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); // 初始默认隐藏
        }

        if (triggerZone != null)
        {
            triggerZone.isTrigger = true; // 确保是Trigger
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
