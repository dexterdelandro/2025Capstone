using UnityEngine;
using TMPro;
using FMODUnity;

public class ItemCollector : MonoBehaviour
{
    public int totalCollectedSpirits = 0;
    public int currentSpiritsAvailable = 5;
    public TextMeshProUGUI spiritUIText;

    private bool canCollectSpirit = false;
    private Spirit currentSpirit;

    public CompanionFollow companion;

    [Header("Audio Settings")]
    public AudioClip spiritCollectSound;
    private AudioSource audioSource;
    private FMOD.Studio.EventInstance absorb;

    [Header("Animation Settings")]
    public Animator characterAnimator; // 角色的 Animator
    public string collectAnimationTrigger = "Collect"; // 触发动画的参数

    [SerializeField] private EventReference absorbSound;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //if (audioSource == null)
        //{
        //    audioSource = gameObject.AddComponent<AudioSource>();
        //}
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
            currentSpiritsAvailable++;
            companion.UpdateNumCollectedSprites(currentSpiritsAvailable);

            UpdateSpiritUI();

            // 播放收集声音
            //if (spiritCollectSound != null && audioSource != null)
            //{
            //    audioSource.PlayOneShot(spiritCollectSound);
            //}

            AudioManager.instance.PlayOneShot(absorbSound, this.transform.position);
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Absorb");

            // 触发收集动画
            if (characterAnimator != null)
            {
                characterAnimator.SetTrigger(collectAnimationTrigger);
            }

            currentSpirit.DestroySpirit();
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
