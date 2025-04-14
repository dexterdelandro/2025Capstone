using System.Collections;
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject activeObject;  // 需要在 Inspector 中指定的对象
    [SerializeField] private GameObject targetObject;  // 需要在 Inspector 中指定的目标对象
    [SerializeField] private float delayTime = 5f;     // 延迟时间，默认5秒

    private void OnEnable()
    {
        StartCoroutine(SwitchObjects());
    }

    private IEnumerator SwitchObjects()
    {
        yield return new WaitForSeconds(delayTime);

        if (activeObject != null)
        {
            activeObject.SetActive(false);
        }

        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }
}