using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public int totalItemsToCollect = 5; // �ܹ���Ҫ�ռ�����������
    private int currentCollectedItems = 0; // ��ǰ���ռ�������
    public Text uiText; // ������ʾ�ռ����ȵ�UI�ı�
    public GameObject hiddenObject; // ���ص�����
    public Transform activationZone; // �ض�����Ĵ�����
    public float activationZoneRadius = 2f; // �ض�����ļ��뾶
    private bool canActivate = false; // �Ƿ���Լ�����������

    public CompanionFollow companion;
    void Start()
    {
        UpdateUI();
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false); // ȷ�������ʼΪ����״̬
        }
    }

    void Update()
    {
        // �������Ƿ����ض����򲢰��� E ��
        if (canActivate && Input.GetKeyDown(KeyCode.E))
        {
            ActivateHiddenObject();
        }
    }

    // ����UI��ʾ
    void UpdateUI()
    {
        if (uiText != null)
        {
            uiText.text = "Collected: " + currentCollectedItems + " / " + totalItemsToCollect;
        }
    }

    // �ռ�����ķ���
    public void CollectItem()
    {
        currentCollectedItems++;
        if(companion!=null)companion.UpdateNumCollectedSprites(currentCollectedItems);
        UpdateUI();

        if (currentCollectedItems >= totalItemsToCollect)
        {
            canActivate = true;
            Debug.Log("All items collected! Go to the activation zone and press 'E'.");
        }
    }

    // ������������
    void ActivateHiddenObject()
    {
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(true); // ��ʾ��������
            Debug.Log("Hidden object is now visible!");
            ClearProgress();
        }
    }

    // ����ռ�����
    void ClearProgress()
    {
        currentCollectedItems = 0;
        if(companion!=null)companion.UpdateNumCollectedSprites(currentCollectedItems);
        UpdateUI();
        canActivate = false; // ���ü���״̬
    }

    // ��ײ�����������ڼ���ռ�����
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            CollectItem(); // �����ռ�����ķ���
            Destroy(other.gameObject); // ���ٱ��ռ�������
        }
    }

    // �������Ƿ��ڼ�������
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canActivate)
        {
            // �ж�����Ƿ����ض�����ķ�Χ��
            if (Vector3.Distance(other.transform.position, activationZone.position) <= activationZoneRadius)
            {
                Debug.Log("Press 'E' to activate the hidden object.");
            }
        }
    }
}
