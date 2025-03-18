using UnityEngine;
using FMODUnity;

public class StormAbility : MonoBehaviour
{
    public GameObject stormPrefab; 
    public float stormDuration = 5f; 
    public KeyCode stormKey = KeyCode.Q; 
    public Vector3 spawnOffset = new Vector3(0, 0, 0);
    [SerializeField] private EventReference stormSound;

    void Update()
    {
        if (Input.GetKeyDown(stormKey))
        {
            Vector3 spawnPos = transform.position + spawnOffset;
            GameObject storm = Instantiate(stormPrefab, spawnPos, Quaternion.identity);
            AudioManager.instance.PlayOneShot(stormSound, this.transform.position);

            Destroy(storm, stormDuration);
        }
    }
}
