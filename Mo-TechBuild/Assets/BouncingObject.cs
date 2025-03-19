using System.Collections;
using UnityEngine;

public class JumpingObject : MonoBehaviour
{
    public float jumpHeight = 2.0f;  // 跳跃的高度
    public float jumpDuration = 0.5f; // 上升时间
    public float fallDuration = 0.2f; // 下降时间
    public float interval = 1.0f; // 每次跳跃的间隔

    private Vector3 startPosition;
    private bool isJumping = false;

    void Start()
    {
        startPosition = transform.position; // 记录初始位置
        StartCoroutine(JumpLoop());
    }

    IEnumerator JumpLoop()
    {
        while (true)
        {
            if (!isJumping)
            {
                isJumping = true;
                yield return Jump();
                yield return new WaitForSeconds(interval);
                isJumping = false;
            }
            yield return null;
        }
    }

    IEnumerator Jump()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = startPosition + new Vector3(0, jumpHeight, 0);

        // 上升
        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        // 下降
        elapsedTime = 0f;
        while (elapsedTime < fallDuration)
        {
            float t = elapsedTime / fallDuration;
            transform.position = Vector3.Lerp(targetPosition, startPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
    }
}