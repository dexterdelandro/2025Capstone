using UnityEngine;
using System.Collections;

public class UseFire : MonoBehaviour
{
    public ParticleSystem firePS;

    public BoxCollider flameHitBox;

    public bool isEmitting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePS = GetComponentInChildren<ParticleSystem>();
        flameHitBox = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEmitting && Input.GetKeyDown(KeyCode.F)){
            Debug.Log("yes yes");
            StartCoroutine(EmmitFlames());
        }
    }

    private IEnumerator EmmitFlames(){
        isEmitting = true;
        firePS.Play();
        flameHitBox.enabled = true;
        yield return new WaitForSeconds(0.75f);
        firePS.Stop(true,ParticleSystemStopBehavior.StopEmitting);
        flameHitBox.enabled = false;
        isEmitting = false;
    }

    void OnTriggerEnter(Collider other){
        if(isEmitting && other.CompareTag("BurnDown")){
            Debug.Log("burn baby burn");
            Destroy(other.gameObject, 0.5f);
        }
    }
}
