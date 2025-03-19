using UnityEngine;
using FMODUnity;

public class footstepSurfaceAudio : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject.CompareTag("Grass_Audio"))
        {
            RuntimeManager.StudioSystem.setParameterByName("Surface", 0);
        } else if (other.gameObject.CompareTag("Stone_Audio"))
        {            
            RuntimeManager.StudioSystem.setParameterByName("Surface", 1);
        } else if (other.gameObject.CompareTag("Tile_Audio"))
        {
            RuntimeManager.StudioSystem.setParameterByName("Surface", 2);
        }
    }
}
