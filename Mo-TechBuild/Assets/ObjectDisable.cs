using UnityEngine;

public class ObjectDisable : MonoBehaviour
{
    [Tooltip("ObjectDisable")]
    public GameObject[] objectsToDeactivate;

    private void OnEnable()
    {
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
