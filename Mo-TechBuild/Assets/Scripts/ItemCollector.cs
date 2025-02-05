using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间

public class ItemCollector : MonoBehaviour
{
    public int totalCollectedSpirits = 0; // 玩家收集到的 Spirit 总数
    public int currentSpiritsAvailable = 0; // 玩家可用的 Spirit 数量
    public TextMeshProUGUI spiritUIText; 

    private bool canCollectSpirit = false; 
    private Spirit currentSpirit; 

    [Header("Audio Settings")]
    public AudioClip spiritCollectSound; // 收集Spirit的音效
    private AudioSource audioSource; // 用于播放音效

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // 如果没有 AudioSource，自动添加
        }
    }

    void Update()
    {
        // 玩家按 E 收集 Spirit
        if (canCollectSpirit && Input.GetKeyDown(KeyCode.E))
        {
            CollectSpirit();
        }
    }

    private void CollectSpirit()
    {
        if (currentSpirit != null)
        {
            totalCollectedSpirits++; // 增加总收集数量
            currentSpiritsAvailable++; // 增加可用的 Spirit 计数
            UpdateSpiritUI();

            // 播放 Spirit 收集音效
            if (spiritCollectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(spiritCollectSound);
            }

            currentSpirit.DestroySpirit(); // 销毁 Spirit
            currentSpirit = null;
        }
    }

    /
    public bool CanUseEarthAbility()
    {
        return currentSpiritsAvailable > 0;
    }

   
    public void UseSpirit()
    {
        if (currentSpiritsAvailable > 0)
        {
            currentSpiritsAvailable--; 
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
            }
        }
    }
}
