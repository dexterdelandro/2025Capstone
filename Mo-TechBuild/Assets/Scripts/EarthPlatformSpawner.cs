using UnityEngine;

public class EarthPlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float maxSpawnDistance = 5f;
    public float minSpawnDistance = 2f;
    public float spawnHeightOffset = -2f;
    public float riseSpeed = 2f;

    private GameObject spawnedPlatform;
    private ItemCollector itemCollector;

    void Start()
    {
        itemCollector = FindObjectOfType<ItemCollector>();
        if (itemCollector == null)
        {
            Debug.LogError("EarthPlatformSpawner: ItemCollector script not found in the scene.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (CanUseEarthAbility())
            {
                TrySpawnPlatform();
            }
            else
            {
                Debug.Log("Not enough Spirits collected to use Earth Ability!");
            }
        }
    }

    bool CanUseEarthAbility()
    {
        return itemCollector != null && itemCollector.GetTotalCollectedSpirits() >= itemCollector.requiredSpiritsForEarthAbility;
    }

    void TrySpawnPlatform()
    {
        Vector3 targetPosition;

        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, maxSpawnDistance))
        {
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = transform.position + transform.forward * maxSpawnDistance;
        }

        if (Vector3.Distance(transform.position, targetPosition) < minSpawnDistance)
        {
            targetPosition = transform.position + transform.forward * minSpawnDistance;
        }

        SpawnPlatform(targetPosition);
    }

    void SpawnPlatform(Vector3 position)
    {
        if (spawnedPlatform != null)
        {
            Destroy(spawnedPlatform);
        }

        Vector3 spawnPosition = position + Vector3.up * spawnHeightOffset;
        spawnedPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        StartCoroutine(RaisePlatform(spawnedPlatform.transform, position));
    }

    System.Collections.IEnumerator RaisePlatform(Transform platform, Vector3 targetPosition)
    {
        while (platform.position.y < targetPosition.y)
        {
            platform.position += Vector3.up * riseSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
