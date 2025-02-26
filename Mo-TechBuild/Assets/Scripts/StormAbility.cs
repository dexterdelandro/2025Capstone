using UnityEngine;
using System.Collections;

public class StormAbility : MonoBehaviour
{
    public GameObject stormPrefab; // The storm prefab to be instantiated
    public ItemCollector itemCollector; // Reference to Spirit management
    public float stormDuration = 5f; // Duration of the storm
    public KeyCode stormKey = KeyCode.Q; // Key to activate the storm
    public Vector3 spawnOffset = new Vector3(0, 0, 0); // Offset for storm spawn position

    private bool isStormActive = false; // Flag to prevent multiple storms at once

    void Update()
    {
        // Check if the ability key is pressed and there are enough Spirits
        if (!isStormActive && itemCollector.GetAvailableSpirits() > 0 && Input.GetKeyDown(stormKey))
        {
            StartCoroutine(ActivateStorm());
        }
    }

    private IEnumerator ActivateStorm()
    {
        isStormActive = true;
        itemCollector.UseSpirit(); // Consume a spirit

        // Instantiate the storm at player's position with offset
        GameObject storm = Instantiate(stormPrefab, transform.position + spawnOffset, Quaternion.identity);

        yield return new WaitForSeconds(stormDuration);

        // Destroy the storm after duration
        Destroy(storm);
        isStormActive = false;
    }
}
