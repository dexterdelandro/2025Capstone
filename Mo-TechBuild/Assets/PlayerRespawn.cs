using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform lastCheckpoint;
    public Transform defaultSpawnPoint; // 默认生成点
    public GameObject player;

    void Start()
    {

        
           lastCheckpoint = defaultSpawnPoint;
            //lastCheckpoint = transform.position;
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Water"))
        {
            Debug.Log("touch water");
            
            Manager.Instance.restart();
            
          
        }
    }

    // public void SetCheckpoint(Vector3 newCheckpoint)
    // {
    //     lastCheckpoint = newCheckpoint;
    // }

}
