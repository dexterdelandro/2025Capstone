using UnityEngine;
using FMODUnity;

public class StormAbility : MonoBehaviour
{
    public GameObject stormPrefab; 
    public float stormDuration = 5f; 
    public KeyCode stormKey = KeyCode.Q; 
    public Vector3 spawnOffset = new Vector3(0, 0, 0);
     public  ItemCollector itemCollector;

    [SerializeField] private EventReference stormSound;

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Q)|| Input.GetKeyDown(KeyCode.JoystickButton2))&&itemCollector.GetAvailableSpirits() > 2) 
        {

            Vector3 spawnPos = transform.position + spawnOffset;
            GameObject storm = Instantiate(stormPrefab, spawnPos, Quaternion.identity);
            AudioManager.instance.PlayOneShot(stormSound, this.transform.position);

            Destroy(storm, stormDuration);
        }
    }
}
