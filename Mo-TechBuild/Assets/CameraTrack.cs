using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    [Header("主物体移动参数")]
    public float zDistance = 5f;
    public float yDistance = 3f;
    public float zSpeed = 2f;  // Z轴移动速度
    public float ySpeed = 4f;  // Y轴移动速度
    public float pauseTimeAfterZ = 1f;

    [Header("协同物体参数")]
    public Transform secondaryObject;
    public float secondaryYDistance = 2f;
    public float secondarySpeed = 4f;

    [Header("最终触发设置")]
    public GameObject finalTriggerObject;
    public float finalDelay = 1f;

    private Vector3 startPosition;
    private bool movingZ = true;
    private bool waiting = false;
    private bool movingY = false;

    private Vector3 secondaryStartPos;
    private bool secondaryStarted = false;
    private bool secondaryDone = false;
    private bool mainDone = false;
    private bool finalTriggered = false;

    void Start()
    {
        startPosition = transform.position;

        if (secondaryObject != null)
        {
            secondaryStartPos = secondaryObject.position;
        }

        if (finalTriggerObject != null)
        {
            finalTriggerObject.SetActive(false); // 确保初始为不激活
        }
    }

    void Update()
    {
        if (movingZ)
        {
            Vector3 target = startPosition + new Vector3(0, 0, zDistance);
            transform.position = Vector3.MoveTowards(transform.position, target, zSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                movingZ = false;
                waiting = true;
                Invoke(nameof(StartYMovement), pauseTimeAfterZ);
            }
        }

        if (movingY)
        {
            Vector3 target = startPosition + new Vector3(0, yDistance, zDistance);
            transform.position = Vector3.MoveTowards(transform.position, target, ySpeed * Time.deltaTime);

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
        if (finalTriggered) return;

        if (mainDone && (secondaryObject == null || secondaryDone))
        {
            finalTriggered = true;
            Invoke(nameof(ActivateFinalObject), finalDelay);
        }
    }

    void ActivateFinalObject()
    {
        if (finalTriggerObject != null)
        {
            finalTriggerObject.SetActive(true);
        }

        enabled = false;
    }
}