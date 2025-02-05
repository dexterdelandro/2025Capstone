using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public int totalCollectedSpirits = 0; 
    public int requiredSpiritsForEarthAbility = 3; 
    public Text spiritUIText; 

    public AudioClip spiritCollectSound; 
    private AudioSource audioSource; 

    private bool canCollectSpirit = false;
    private Spirit currentSpirit;

    void Start()
    {
        // 获取玩家的 AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("ItemCollector: No AudioSource found on Player! Add one to play sounds.");
        }
    }

    void Update()
    {
        if (canCollectSpirit && Input.GetKeyDown(KeyCode.E))
        {
            CollectSpirit();
        }
    }

    private void CollectSpirit()
    {
        if (currentSpirit != null)
        {
            totalCollectedSpirits++;
            UpdateSpiritUI();

            // 播放 Spirit 收集音效
            if (audioSource != null && spiritCollectSound != null)
            {
                audioSource.PlayOneShot(spiritCollectSound);
            }

            currentSpirit.DestroySpirit();
            currentSpirit = null;
        }
    }

    public int GetTotalCollectedSpirits()
    {
        return totalCollectedSpirits;
    }

    private void UpdateSpiritUI()
    {
        if (spiritUIText != null)
        {
            spiritUIText.text = "Spirits: " + totalCollectedSpirits;
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
                currentSpirit.ShowPopUp(true);
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
                currentSpirit.ShowPopUp(false);
                currentSpirit = null;
            }
        }
    }
}
