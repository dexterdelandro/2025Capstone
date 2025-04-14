using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class CaveMusic : MonoBehaviour
{
    public Transform player;
    private float newDist;
    public static CaveMusic instance { get; private set; }
    
    [SerializeField] private EventReference caveRevealMusic;
    [SerializeField] private EventReference caveMusic;
    [SerializeField] private bool hasTriggered = false;
    

    private EventInstance music;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        AudioManager.instance.PlayOneShot(caveMusic, this.transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist > 100f)
        {
            dist = 100f;
        }
        
        newDist = dist / 100;
        
        RuntimeManager.StudioSystem.setParameterByName("IntroDistance", newDist);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && hasTriggered == false)
        {
            AudioManager.instance.PlayOneShot(caveRevealMusic, this.transform.position);
            hasTriggered = true;
        }
    }

    bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }
}
