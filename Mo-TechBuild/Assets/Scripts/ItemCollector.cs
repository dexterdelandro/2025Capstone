using UnityEngine;
using TMPro; 

public class ItemCollector : MonoBehaviour
{
<<<<<<< Updated upstream
    public int totalItemsToCollectZone1 = 5;
    public int totalItemsToCollectZone2 = 3;
    public int currentCollectedItemsZone1 = 0;
    public int currentCollectedItemsZone2 = 0;
    public Text uiTextZone1;
    public Text uiTextZone2;
    public GameObject hiddenObjectZone1;
    public GameObject hiddenObjectZone2;
    public Transform activationZone1;
    public Transform activationZone2;
    public float activationZoneRadius = 2f;
    private bool canActivateZone1 = false;
    private bool canActivateZone2 = false;

    public CompanionFollow companion;

    void Start()
    {
        if (hiddenObjectZone1 != null)
        {
            hiddenObjectZone1.SetActive(false);
        }

        if (hiddenObjectZone2 != null)
        {
            hiddenObjectZone2.SetActive(true);
        }
       
    }

    void Update()
    {
        if (canActivateZone1 && Input.GetKeyDown(KeyCode.E))
=======
    public int totalCollectedSpirits = 0; 
    public int currentSpiritsAvailable = 0; 
    public TextMeshProUGUI spiritUIText; 

    private bool canCollectSpirit = false; 
    private Spirit currentSpirit; 

    void Update()
    {
        // 玩家按 E 收集 Spirit
        if (canCollectSpirit && Input.GetKeyDown(KeyCode.E))
>>>>>>> Stashed changes
        {
            ActivateHiddenObjectZone1();
        }

        if (canActivateZone2 && Input.GetKeyDown(KeyCode.E))
        {
            ActivateHiddenObjectZone2();
        }
    }

    void UpdateUI(Text uiText, int currentCollected, int totalToCollect)
    {
        if (uiText != null)
        {
<<<<<<< Updated upstream
            uiText.text = "Collected: " + currentCollected + " / " + totalToCollect;
        }
    }

    public void CollectItemForZone1()
    {
        currentCollectedItemsZone1++;
        GetComponent<AudioSource>().Play();
        if (companion != null) companion.UpdateNumCollectedSprites(currentCollectedItemsZone1);

        UpdateUI(uiTextZone1, currentCollectedItemsZone1, totalItemsToCollectZone1);

        if (currentCollectedItemsZone1 >= totalItemsToCollectZone1)
        {
            canActivateZone1 = true;
            Debug.Log("All items for Zone 1 collected! Go to the activation zone and press 'E'.");
        }
    }

    public void CollectItemForZone2()
    {
        currentCollectedItemsZone2++;
        GetComponent<AudioSource>().Play();
        if (companion != null) companion.UpdateNumCollectedSprites(currentCollectedItemsZone2);

        UpdateUI(uiTextZone2, currentCollectedItemsZone2, totalItemsToCollectZone2);

        if (currentCollectedItemsZone2 >= totalItemsToCollectZone2)
        {
            canActivateZone2 = true;
            Debug.Log("All items for Zone 2 collected! Go to the activation zone and press 'E'.");
        }
    }

    void ActivateHiddenObjectZone1()
    {
        if (hiddenObjectZone1 != null)
        {
            hiddenObjectZone1.SetActive(true);
            Debug.Log("Hidden object in Zone 1 is now visible!");
            ClearProgressZone1();
        }
    }

    void ActivateHiddenObjectZone2()
    {
        if (hiddenObjectZone2 != null)
        {
            hiddenObjectZone2.SetActive(false);
            Debug.Log("Hidden object in Zone 2 is now hidden!");
            ClearProgressZone2();
        }
    }

    void ClearProgressZone1()
    {
        currentCollectedItemsZone1 = 0;
        if (companion != null) companion.UpdateNumCollectedSprites(currentCollectedItemsZone1);
        UpdateUI(uiTextZone1, currentCollectedItemsZone1, totalItemsToCollectZone1);
        canActivateZone1 = false;
    }

    void ClearProgressZone2()
    {
        currentCollectedItemsZone2 = 0;
        if (companion != null) companion.UpdateNumCollectedSprites(currentCollectedItemsZone2);
        UpdateUI(uiTextZone2, currentCollectedItemsZone2, totalItemsToCollectZone2);
        canActivateZone2 = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canActivateZone1 && Vector3.Distance(other.transform.position, activationZone1.position) <= activationZoneRadius)
            {
                Debug.Log("Press 'E' to activate the hidden object in Zone 1.");
            }

            if (canActivateZone2 && Vector3.Distance(other.transform.position, activationZone2.position) <= activationZoneRadius)
            {
                Debug.Log("Press 'E' to activate the hidden object in Zone 2.");
=======
            totalCollectedSpirits++; // 增加总收集数量
            currentSpiritsAvailable++; // 增加可用的 Spirit 计数
            UpdateSpiritUI();
            currentSpirit.DestroySpirit();
            currentSpirit = null;
        }
    }

    // 检查是否还有可用的 Spirit
    public bool CanUseEarthAbility()
    {
        return currentSpiritsAvailable > 0;
    }

    // 消耗一个 Spirit
    public void UseSpirit()
    {
        if (currentSpiritsAvailable > 0)
        {
            currentSpiritsAvailable--; // 减少可用的 Spirit 数量
            UpdateSpiritUI();
        }
    }

    public int GetAvailableSpirits()
    {
        return currentSpiritsAvailable;
    }

    private void UpdateSpiritUI()
    {
        if (spiritUIText != null)
        {
            spiritUIText.text = "Spirits: " + currentSpiritsAvailable;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elemental"))
        {
            canCollectSpirit = true;
            currentSpirit = other.GetComponent<Spirit>();

            if (currentSpirit != null)
            {
                currentSpirit.ShowPopUp(true); // 显示交互提示
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Elemental"))
        {
            canCollectSpirit = false;
            if (currentSpirit != null)
            {
                currentSpirit.ShowPopUp(false); // 隐藏交互提示
                currentSpirit = null;
>>>>>>> Stashed changes
            }
        }
    }
}
