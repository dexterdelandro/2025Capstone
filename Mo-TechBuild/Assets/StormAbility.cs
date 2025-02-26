using UnityEngine;

public class StormAbility : MonoBehaviour
{
    public GameObject stormPrefab; // 风暴预制体
    public float stormDuration = 5f; // 风暴持续时间
    public KeyCode stormKey = KeyCode.Q; // 触发风暴的按键
    public Vector3 spawnOffset = new Vector3(0, 0, 0); // 生成风暴的偏移量

    void Update()
    {
        if (Input.GetKeyDown(stormKey))
        {
            Vector3 spawnPos = transform.position + spawnOffset;
            GameObject storm = Instantiate(stormPrefab, spawnPos, Quaternion.identity);

            // 直接在 X 秒后销毁风暴
            Destroy(storm, stormDuration);
        }
    }
}
