using UnityEngine;

public class endingMusicPreserve : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static endingMusicPreserve instance { get; private set; }

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
