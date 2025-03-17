using UnityEngine;
using System.Collections;

public class UseFire : MonoBehaviour
{

    public ItemCollector itemCollector;
    public ParticleSystem firePS;

    public BoxCollider flameHitBox;

    public bool isEmitting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePS = GetComponentInChildren<ParticleSystem>();
        flameHitBox = GetComponentInChildren<BoxCollider>();
        flameHitBox.enabled = false;
    }

    // Update is called once per frame
    void Update()
{
    float dpadY = Input.GetAxis("Vertical"); // 读取手柄十字键的 Y 轴

    if (!isEmitting && itemCollector.GetAvailableSpirits() > 0 &&
        (Input.GetKeyDown(KeyCode.F) || dpadY > 0.5f)) 
    {
        StartCoroutine(EmmitFlames());
    }
}

    private IEnumerator EmmitFlames(){
        isEmitting = true;
        itemCollector.UseSpirit();
        firePS.Play();
        flameHitBox.enabled = true;
        yield return new WaitForSeconds(0.75f);
        firePS.Stop(true,ParticleSystemStopBehavior.StopEmitting);
        flameHitBox.enabled = false;
        isEmitting = false;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Burnable")){
            //Debug.Log("HItting");
            Destroy(other.gameObject, 0.5f);
        }
    }
}
