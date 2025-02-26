using UnityEngine;
using System.Collections;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget; // 传送目标位置
    [SerializeField] private float delay = 1.5f; // 传送延迟时间
    private bool hasTeleported = false; // 是否已经传送过

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTeleported && other.CompareTag("Player")) // 确保只有玩家会触发，并且只执行一次
        {
            hasTeleported = true;
            StartCoroutine(TeleportAfterDelay(other));
        }
    }

    private IEnumerator TeleportAfterDelay(Collider player)
{
    yield return new WaitForSeconds(delay);

    CharacterController controller = player.GetComponent<CharacterController>();
    if (controller != null)
    {
        controller.enabled = false;  // 先禁用 CharacterController
        player.transform.position = teleportTarget.position;
        controller.enabled = true;   // 传送后重新启用
    }
    else
    {
        player.transform.position = teleportTarget.position;
    }
}

}
