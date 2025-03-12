using UnityEngine;

public class StormAbility : MonoBehaviour
{
    public GameObject stormPrefab; 
    public float stormDuration = 5f; 
    public KeyCode stormKey = KeyCode.Q; 
    public Vector3 spawnOffset = new Vector3(0, 0, 0); 

    void Update()
    {
        if (Input.GetKeyDown(stormKey))
        {
            Vector3 spawnPos = transform.position + spawnOffset;
            GameObject storm = Instantiate(stormPrefab, spawnPos, Quaternion.identity);

            
            Destroy(storm, stormDuration);
        }
    }
}
