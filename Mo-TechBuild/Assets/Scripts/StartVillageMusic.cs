using UnityEngine;

public class StartVillageMusic : MonoBehaviour
{

    private string targetTag = "Player";
    private bool isTriggered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (isTriggered == false)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Music/VillageTransition");
                isTriggered = true;
            }

        }
    }
}
