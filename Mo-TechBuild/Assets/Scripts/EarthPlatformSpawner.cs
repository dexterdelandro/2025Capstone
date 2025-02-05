using UnityEngine;

public class EarthPlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;  // Ԥ����
    public float maxSpawnDistance = 5f; // ����������
    public float minSpawnDistance = 2f; // ��С���ɾ��룬��������ڽ���
    public float spawnHeightOffset = -2f; // ��ʼ�߶ȣ����£�
    public float riseSpeed = 2f; // �����ٶ�

    private GameObject spawnedPlatform;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // ��������滻Ϊ�ֱ�����
        {
            TrySpawnPlatform();
        }
    }

    void TrySpawnPlatform()
    {
        Vector3 targetPosition;

        // 1. **��ȡ��Ļ���ĵ�����**
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // 2. **���߼�⣬Ѱ�ҵ���**
        if (Physics.Raycast(ray, out RaycastHit hit, maxSpawnDistance))
        {
            targetPosition = hit.point; // ��ȡ���߻��еĵ�
        }
        else
        {
            // ���û�л��е��棬��ʹ�����ǰ�� maxSpawnDistance ��ΪĿ��λ��
            targetPosition = transform.position + transform.forward * maxSpawnDistance;
        }

        // 3. **������С���룬��ֹƽ̨��������ҽ���**
        if (Vector3.Distance(transform.position, targetPosition) < minSpawnDistance)
        {
            targetPosition = transform.position + transform.forward * minSpawnDistance;
        }

        // 4. **����ƽ̨**
        SpawnPlatform(targetPosition);
    }

    void SpawnPlatform(Vector3 position)
    {
        if (spawnedPlatform != null)
        {
            Destroy(spawnedPlatform);
        }

        // �趨��ʼλ�ã�ʹƽ̨�ӵ�������
        Vector3 spawnPosition = position + Vector3.up * spawnHeightOffset;
        spawnedPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        // ����Э����ƽ̨��������
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
