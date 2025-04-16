using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform lastCheckpoint;
    public Transform defaultSpawnPoint; // 默认生成点
    public GameObject player;

    public GameObject waterUi;

    public bool isDead= false;

    void Start()
    {

        
           lastCheckpoint = defaultSpawnPoint;
            //lastCheckpoint = transform.position;
        
    }

    void Update()
    {
        //if(isDead == true && (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.R)))
        //{
            //isDead= false;
             //Manager.Instance.restart();
             
        //}
    }
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Water"))
        {
            Debug.Log("touch water");

            Manager.Instance.PlayerDied();

            waterUi.gameObject.SetActive(true);
            isDead=true;

          
        }
    }

    
}
