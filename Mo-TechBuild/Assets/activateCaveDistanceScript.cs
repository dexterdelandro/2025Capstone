using UnityEngine;

public class activateCaveDistanceScript : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("TRIGGERED!!!!");
            GetComponent<caveDistanceParameter>().enabled = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("out");
            GetComponent<caveDistanceParameter>().enabled = false;
        }
    }
}
