using UnityEngine;
using UnityEngine.UI;

public class LiftObjectSmooth : MonoBehaviour
{
    public float liftSpeed = 2.0f;    // 物体上升速度
    public float maxHeight = 10f;     // 目标上升高度
    public GameObject triggerZone;    // 选择的触发区域（在 Inspector 里绑定）
    public GameObject objectToLift;   // 选择要升起的物体（在 Inspector 里绑定）
    public GameObject uiPrompt;       // UI 提示（在 Inspector 里绑定）

    private bool isLifting = false;
    private bool isPlayerInRange = false;
    private bool hasLifted = false;  // 确保只触发一次
    private bool isUIActive = false; // UI 是否激活
    private Vector3 targetPosition;   // 目标位置

    void Start()
    {
        // 初始化目标位置（但不改变 objectToLift 的位置）
        if (objectToLift != null)
        {
            targetPosition = new Vector3(objectToLift.transform.position.x, objectToLift.transform.position.y + maxHeight, objectToLift.transform.position.z);
        }

        // 确保 UI 提示初始时是关闭的
        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false);
        }
    }

    void Update()
    {
        // 当玩家在触发范围内，并且物体还未升高，按 E 触发升高
        if (isPlayerInRange && !hasLifted && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0)))
        {
            isLifting = true;
            hasLifted = true; // 确保只触发一次

            // 显示 UI
            if (uiPrompt != null)
            {
                uiPrompt.SetActive(true);
                isUIActive = true;
            }
        }

        // 按 Space 关闭 UI（仅在 UI 激活时生效）
        if (isUIActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton8)))
        {
            if (uiPrompt != null)
            {
                uiPrompt.SetActive(false);
                isUIActive = false;
            }
        }

        // 平滑升高 objectToLift
        if (isLifting && objectToLift != null)
        {
            objectToLift.transform.position = Vector3.MoveTowards(objectToLift.transform.position, targetPosition, liftSpeed * Time.deltaTime);

            // 当接近目标高度时，停止移动并禁用脚本
            if (Vector3.Distance(objectToLift.transform.position, targetPosition) < 0.01f)
            {
                objectToLift.transform.position = targetPosition; // 确保完全对齐目标位置
                isLifting = false; // 停止移动
                this.enabled = false; // 禁用脚本
            }
        }
    }

    // 当玩家进入触发区域时，允许触发
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && triggerZone != null && other.gameObject == triggerZone)
        {
            isPlayerInRange = true;
            if (uiPrompt != null) uiPrompt.SetActive(true); // 显示提示 UI
        }
    }

    // 当玩家离开触发区域时，禁止触发
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && triggerZone != null && other.gameObject == triggerZone)
        {
            isPlayerInRange = false;
            if (uiPrompt != null) uiPrompt.SetActive(false); // 关闭 UI
        }
    }
}