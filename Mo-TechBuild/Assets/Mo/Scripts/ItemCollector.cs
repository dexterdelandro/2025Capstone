using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public int totalItemsToCollect = 5; // 总共需要收集的物体数量
    private int currentCollectedItems = 0; // 当前已收集的数量
    public Text uiText; // 用于显示收集进度的UI文本
    public GameObject hiddenObject; // 隐藏的物体
    public Transform activationZone; // 特定区域的触发点
    public float activationZoneRadius = 2f; // 特定区域的检测半径
    private bool canActivate = false; // 是否可以激活隐藏物体

    void Start()
    {
        UpdateUI();
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false); // 确保物体初始为隐藏状态
        }
    }

    void Update()
    {
        // 检测玩家是否在特定区域并按下 E 键
        if (canActivate && Input.GetKeyDown(KeyCode.E))
        {
            ActivateHiddenObject();
        }
    }

    // 更新UI显示
    void UpdateUI()
    {
        if (uiText != null)
        {
            uiText.text = "Collected: " + currentCollectedItems + " / " + totalItemsToCollect;
        }
    }

    // 收集物体的方法
    public void CollectItem()
    {
        currentCollectedItems++;
        UpdateUI();

        if (currentCollectedItems >= totalItemsToCollect)
        {
            canActivate = true;
            Debug.Log("All items collected! Go to the activation zone and press 'E'.");
        }
    }

    // 激活隐藏物体
    void ActivateHiddenObject()
    {
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(true); // 显示隐藏物体
            Debug.Log("Hidden object is now visible!");
            ClearProgress();
        }
    }

    // 清空收集进度
    void ClearProgress()
    {
        currentCollectedItems = 0;
        UpdateUI();
        canActivate = false; // 重置激活状态
    }

    // 碰撞触发器，用于检测收集物体
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            CollectItem(); // 调用收集物体的方法
            Destroy(other.gameObject); // 销毁被收集的物体
        }
    }

    // 检测玩家是否在激活区域
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canActivate)
        {
            // 判断玩家是否在特定区域的范围内
            if (Vector3.Distance(other.transform.position, activationZone.position) <= activationZoneRadius)
            {
                Debug.Log("Press 'E' to activate the hidden object.");
            }
        }
    }
}
