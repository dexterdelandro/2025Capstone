using UnityEngine;
using FMODUnity;

public class caveDistanceParameter : MonoBehaviour
{
    public Transform player;
    private float newDist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if(dist > 50f)
        {
            dist = 50f;
        }
        Debug.Log(dist);

        newDist = dist / 50;
        Debug.Log(newDist);
        RuntimeManager.StudioSystem.setParameterByName("CaveDistance", newDist);
    }
}
