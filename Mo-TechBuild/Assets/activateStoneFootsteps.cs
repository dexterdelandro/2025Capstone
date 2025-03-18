using UnityEngine;
using FMODUnity;

public class activateStoneFootsteps : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RuntimeManager.StudioSystem.setParameterByName("Surface", 1);
        }
    }
}
