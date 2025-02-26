using UnityEngine;

public class Wildfire : MonoBehaviour
{
    public float destoryDelay;

    void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("WaterPS")){
            Destroy(this.gameObject,destoryDelay);
        }
    }
}
