using UnityEngine;
using TMPro; // ���� TextMeshPro �����ռ�

public class ItemCollector : MonoBehaviour
{
    public int totalCollectedSpirits = 0; // ����ռ����� Spirit ����
    public int currentSpiritsAvailable = 5; // ��ҿ��õ� Spirit ����
    public TextMeshProUGUI spiritUIText; 

    private bool canCollectSpirit = false; 
    private Spirit currentSpirit; 

    public CompanionFollow companion;

    [Header("Audio Settings")]
    public AudioClip spiritCollectSound; // �ռ�Spirit����Ч
    private AudioSource audioSource; // ���ڲ�����Ч

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // ���û�� AudioSource���Զ�����
        }
    }

    void Update()
    {
        // ��Ұ� E �ռ� Spirit
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
            currentSpiritsAvailable++; 
            companion.UpdateNumCollectedSprites(currentSpiritsAvailable);

            UpdateSpiritUI();

            // ���� Spirit �ռ���Ч
            if (spiritCollectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(spiritCollectSound);
            }

            currentSpirit.DestroySpirit(); // ���� Spirit
            currentSpirit = null;
        }
    }

    public bool CanUseEarthAbility()
    {
        return currentSpiritsAvailable > 0;
    }

   
    public void UseSpirit()
    {
        if (currentSpiritsAvailable > 0)
        {
            currentSpiritsAvailable--;
            companion.UpdateNumCollectedSprites(currentSpiritsAvailable);
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
                currentSpirit.ShowPopUp(true); // ��ʾ������ʾ
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
                currentSpirit.ShowPopUp(false); // ���ؽ�����ʾ
                currentSpirit = null;
            }
        }
    }
}
