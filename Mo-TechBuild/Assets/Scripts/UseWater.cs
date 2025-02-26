using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class UseWater : MonoBehaviour
{

    public ItemCollector itemCollector;

    public ParticleSystem waterPS;

    public float duration;


    public bool isEmitting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waterPS = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEmitting && itemCollector.GetAvailableSpirits()>0 && Input.GetKeyDown(KeyCode.T)){
            //Debug.Log("yes yes");
            StartCoroutine(EmmitWater());
        }
    }

    private IEnumerator EmmitWater(){
        isEmitting = true;
        itemCollector.UseSpirit();
        waterPS.Play();
        yield return new WaitForSeconds(duration);
        waterPS.Stop(true,ParticleSystemStopBehavior.StopEmitting);
        isEmitting = false;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("WildFire")){
            Debug.Log("HItting");
            Destroy(other.gameObject, 0.5f);
        }
    }
}
