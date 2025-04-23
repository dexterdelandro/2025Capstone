using System.Collections;
using System.Threading;
using StarterAssets;
using UnityEngine;
using FMODUnity;


public class UseWindJump : MonoBehaviour
{
        [SerializeField]
    private bool isJumpActive = false;

    [SerializeField]
    private ItemCollector itemCollector;

    [SerializeField]
    private ThirdPersonController controller;

    public float standardJumpHeight = 3;

    public float windJumpHeight;

    public ParticleSystem tornadoVFX;

    [SerializeField] private EventReference stormSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = Manager.Instance.GetPlayer().GetComponent<ThirdPersonController>();
        itemCollector = Manager.Instance.GetPlayer().GetComponent<ItemCollector>();
        tornadoVFX.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Q)|| Input.GetKeyDown(KeyCode.JoystickButton2))&&itemCollector.GetAvailableSpirits() > 1){
            itemCollector.UseSpirit();
            PrepareJump();
        }

        if(isJumpActive && Input.GetKeyDown(KeyCode.Space)){
            ResetJump();
        }


    }

    private void PrepareJump(){
        isJumpActive = true;
        tornadoVFX.gameObject.SetActive(true);
        controller.JumpHeight = windJumpHeight;
        tornadoVFX.Play();
    }

    private void ResetJump(){
        isJumpActive = false;
        AudioManager.instance.PlayOneShot(stormSound, this.transform.position);
        StartCoroutine(StartVFXDelay());
        
    }

    IEnumerator StartVFXDelay(){
        yield return new WaitForSeconds(0.25f);
        controller.JumpHeight = standardJumpHeight;
        tornadoVFX.Stop();
        tornadoVFX.gameObject.SetActive(false);
    }
}
