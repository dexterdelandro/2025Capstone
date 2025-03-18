using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

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
        if(!isEmitting && itemCollector.GetAvailableSpirits()>0 && Input.GetKeyDown(KeyCode.F)){
            //Debug.Log("yes yes");
            StartCoroutine(EmmitFlames());
        }
    }

    private IEnumerator EmmitFlames(){
        isEmitting = true;
        itemCollector.UseSpirit();
        firePS.Play();
        flameHitBox.enabled = true;
        yield return new WaitForSeconds(1.75f);
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
