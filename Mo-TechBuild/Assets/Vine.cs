using Unity.Jobs;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public float maxHeight;

    private bool didCollide = false;

    public float rate;


    public bool doneGrowing = false;

    void Start()
    {

    }

    void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("WaterPS")){
            didCollide = true;
        }
    }

    void Update()
    {
        if(didCollide && !doneGrowing){
            transform.localScale += new Vector3(0.0f, rate, 0.0f)*Time.deltaTime;
            if(transform.localScale.y >= maxHeight){
                doneGrowing = true;
            }
        }
    }
}
