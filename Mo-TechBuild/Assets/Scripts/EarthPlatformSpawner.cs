using UnityEngine;

public class EarthPlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;  // 预制体
    public float maxSpawnDistance = 5f; // 生成最大距离
    public float minSpawnDistance = 2f; // 最小生成距离，避免出现在脚下
    public float spawnHeightOffset = -2f; // 初始高度（地下）
    public float riseSpeed = 2f; // 上升速度

    private GameObject spawnedPlatform;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // 这里可以替换为手柄输入
        {
            TrySpawnPlatform();
        }
    }

    void TrySpawnPlatform()
    {
        Vector3 targetPosition;

        // 1. **获取屏幕中心的射线**
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // 2. **射线检测，寻找地面**
        if (Physics.Raycast(ray, out RaycastHit hit, maxSpawnDistance))
        {
            targetPosition = hit.point; // 获取射线击中的点
        }
        else
        {
            // 如果没有击中地面，则使用玩家前方 maxSpawnDistance 作为目标位置
            targetPosition = transform.position + transform.forward * maxSpawnDistance;
        }

        // 3. **限制最小距离，防止平台出现在玩家脚下**
        if (Vector3.Distance(transform.position, targetPosition) < minSpawnDistance)
        {
            targetPosition = transform.position + transform.forward * minSpawnDistance;
        }

        // 4. **生成平台**
        SpawnPlatform(targetPosition);
    }

    void SpawnPlatform(Vector3 position)
    {
        if (spawnedPlatform != null)
        {
            Destroy(spawnedPlatform);
        }

        // 设定初始位置，使平台从地下升起
        Vector3 spawnPosition = position + Vector3.up * spawnHeightOffset;
        spawnedPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        // 启动协程让平台缓缓升起
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
