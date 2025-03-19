using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
using FMODUnity;

public class UseFire : MonoBehaviour
{

    public ItemCollector itemCollector;
    public ParticleSystem firePS;
    public BoxCollider flameHitBox;

    public bool isEmitting = false;

    [SerializeField] private EventReference fireSound;

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
    // 获取 R2 扳机键的输入（0 到 1 之间的浮点数）
    float r2Trigger = Input.GetAxis("Trigger"); // 或者 "Joystick Axis 9"

    if (!isEmitting && itemCollector.GetAvailableSpirits() > 0 &&
        (Input.GetKeyDown(KeyCode.F) || r2Trigger > 0.5f))
    {
        StartCoroutine(EmmitFlames());
    }
}

    private IEnumerator EmmitFlames(){
        isEmitting = true;
        itemCollector.UseSpirit();
        firePS.Play();
        AudioManager.instance.PlayOneShot(fireSound, this.transform.position);
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
