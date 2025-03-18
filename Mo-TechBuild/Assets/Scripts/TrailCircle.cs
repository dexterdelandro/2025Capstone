using UnityEngine;

public class TrailCircle : MonoBehaviour
{

    public float radius;

    public float speed;

    public GameObject trail;

    public Transform BanPos;

   private Vector3 centerPoint;

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
            float x = centerPoint.x + Mathf.Cos(Time.time * speed) * radius;
            float z = centerPoint.z + Mathf.Sin(Time.time * speed) * radius;
            trail.transform.position = new Vector3(x, transform.position.y, z);
        }else{
            Vector3 targetDirection = (BanPos.position - trail.transform.position).normalized;
            trail.transform.position += targetDirection * speed * Time.deltaTime;
            if(Vector3.Distance(trail.transform.position, BanPos.position)<0.5f){
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hey!");
        if(other.CompareTag("Player")){
            didCollect = true;
        }
        
    }
}
