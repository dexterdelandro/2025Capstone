using UnityEngine;
using UnityEngine.UI;  // 引入 UI 相关命名空间

public class LiftObjectSmooth : MonoBehaviour
{
    public float liftSpeed = 2.0f;    // 上升速度
    public float maxHeight = 10f;     // 最大上升高度
    public GameObject triggerZone;    // 绑定的触发区域
    public GameObject uiPrompt;       // 绑定的 UI 提示（手动拖入）

    private bool isLifting = false;
    private bool isPlayerInRange = false;
    private bool hasLifted = false;  // 这个物体是否已经触发过升高
    private bool isUIActive = false; // UI 是否激活

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;

        // 确保 UI 提示初始时是关闭的
        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false);
        }
    }

    void Update()
    {
        // 当玩家在触发范围内，并且物体还未升高，按 Space 触发升高并显示 UI
        if (isPlayerInRange && !hasLifted && Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            isLifting = true;
            hasLifted = true; // 确保只触发一次
            targetPosition = new Vector3(transform.position.x, transform.position.y + maxHeight, transform.position.z);

            // 显示 UI
            if (uiPrompt != null)
            {
                uiPrompt.SetActive(true);
                isUIActive = true;
            }
        }

        // 按 E 关闭 UI
        if (isUIActive && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton8))
        {
            if (uiPrompt != null)
            {
                uiPrompt.SetActive(false);
                isUIActive = false;
            }
        }

        // 平滑升高
        if (isLifting)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, liftSpeed * Time.deltaTime);

            // 当接近目标高度时，禁用脚本
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                this.enabled = false; // 禁用脚本，彻底让升降功能失效
            }
        }
    }
}