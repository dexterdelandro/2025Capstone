using UnityEngine;

public class StormAbility : MonoBehaviour
{
    public GameObject stormPrefab; // �籩Ԥ����
    public float stormDuration = 5f; // �籩����ʱ��
    public KeyCode stormKey = KeyCode.Q; // �����籩�İ���
    public Vector3 spawnOffset = new Vector3(0, 0, 0); // ���ɷ籩��ƫ����

    void Update()
    {
        if (Input.GetKeyDown(stormKey))
        {
            Vector3 spawnPos = transform.position + spawnOffset;
            GameObject storm = Instantiate(stormPrefab, spawnPos, Quaternion.identity);

            // ֱ���� X ������ٷ籩
            Destroy(storm, stormDuration);
        }
    }
}
