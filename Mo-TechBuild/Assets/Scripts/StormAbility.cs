using UnityEngine;

public class StormAbility : MonoBehaviour
{
    public GameObject stormPrefab; 
    public float stormDuration = 5f; 
    
    public Vector3 spawnOffset = new Vector3(0, 0, 0); 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)|| Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            Vector3 spawnPos = transform.position + spawnOffset;
            GameObject storm = Instantiate(stormPrefab, spawnPos, Quaternion.identity);

            
            Destroy(storm, stormDuration);
        }
    }
}
