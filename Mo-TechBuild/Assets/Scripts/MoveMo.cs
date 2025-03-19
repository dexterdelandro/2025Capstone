using UnityEngine;

public class TrailCircle : MonoBehaviour
{
    public float speed;

    public GameObject trail;

    public Transform BanPos;

   private Vector3 centerPoint;

   public float movementRange;

   Collider playerCollider;

    [SerializeField]
   private bool didCollect = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BanPos = GameObject.FindWithTag("Ban").transform;
        centerPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!didCollect){

            float y = centerPoint.y + Mathf.Sin(Time.time * speed) * movementRange;
            trail.transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }else{
            Vector3 targetDirection = (BanPos.position - trail.transform.position).normalized;
            trail.transform.position += targetDirection * speed * 2.5f * Time.deltaTime;
            
            if(Vector3.Distance(trail.transform.position, BanPos.position)<0.5f){
                playerCollider.GetComponent<ItemCollector>().CollectSpirit();
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hey!");
        if(other.CompareTag("Player")){
            playerCollider = other;
            didCollect = true;
        }
        
    }
}
