using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    [Header("主物体移动参数")]
    public float zDistance = 5f;
    public float yDistance = 3f;
    public float speed = 2f;
    public float pauseTimeAfterZ = 1f;

    [Header("协同物体参数")]
    public Transform secondaryObject; // 协同移动的物体
    public float secondaryYDistance = 2f;
    public float secondarySpeed = 2f;

    [Header("触发目标")]
    public GameObject finalTriggerObject; // 所有移动结束后触发的物体

    private Vector3 startPosition;
    private bool movingZ = true;
    private bool waiting = false;
    private bool movingY = false;

    private Vector3 secondaryStartPos;
    private bool secondaryStarted = false;
    private bool secondaryDone = false;
    private bool mainDone = false;

    void Start()
    {
        startPosition = transform.position;
        if (secondaryObject != null)
        {
            secondaryStartPos = secondaryObject.position;
        }
    }

    void Update()
    {
        if (movingZ)
        {
            Vector3 target = startPosition + new Vector3(0, 0, zDistance);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                movingZ = false;
                waiting = true;
                Invoke(nameof(StartYMovement), pauseTimeAfterZ); // 延迟调用
            }
        }

        if (movingY)
        {
            Vector3 target = startPosition + new Vector3(0, yDistance, zDistance);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                mainDone = true;
                TryTriggerFinal();
            }
        }

        if (secondaryStarted && !secondaryDone && secondaryObject != null)
        {
            Vector3 secondaryTarget = secondaryStartPos + new Vector3(0, secondaryYDistance, 0);
            secondaryObject.position = Vector3.MoveTowards(secondaryObject.position, secondaryTarget, secondarySpeed * Time.deltaTime);

            if (Vector3.Distance(secondaryObject.position, secondaryTarget) < 0.01f)
            {
                secondaryDone = true;
                TryTriggerFinal();
            }
        }
    }

    void StartYMovement()
    {
        waiting = false;
        movingY = true;

        if (secondaryObject != null)
        {
            secondaryStarted = true;
        }
    }

    void TryTriggerFinal()
    {
        if (mainDone && (secondaryObject == null || secondaryDone))
        {
            if (finalTriggerObject != null)
            {
                finalTriggerObject.SetActive(true);
                // 你可以改成触发动画、粒子、调用方法等
            }

            enabled = false; // 停止更新
        }
    }
}
