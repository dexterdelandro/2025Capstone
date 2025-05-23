using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;
using System.IO;
using System;
using TMPro;


public class CompanionFollow : MonoBehaviour
{

    public float collectionTime;

    private Queue<string> previousComplaints = new Queue<string>();


    public LayerMask spriteLayer;
    enum CurrentAction{
        Idle,
        Following,
        Collecting,

        Waiting
    }

    CurrentAction currentAction = CurrentAction.Idle;

    public GameObject player;

    public ParticleSystem particesystem;

    public List<String>complaints;

    public float complaintDisplayTime;

    [SerializeField]
    private Canvas dialogueUI;

    [SerializeField]
    private Canvas countUI;

    [SerializeField]
    private TMP_Text complaintText;
    private float time;

    [Header("-----Movement Variables-----")]
    [SerializeField]
    private float movementSpeed;

    public float StartingMoveSpeed;
    public float minMoveSpeed;
    public float minStartDistance; //the distance away from player to START following

    public float minEndDistance;

    public float endLocationRadius;

    public float currDistance;

    // public string fileName;

    // [Space(10)]
    // [Header("-----Ink Caps-----")]
    // [SerializeField]
    // private float softCap;

    // [SerializeField]
    // private float hardCap;

    // [SerializeField]
    // private float softCap2;

    // [SerializeField]
    // private float hardCap2;

    // public bool isStoic;

    public float _softCap;

    public float _hardCap;

        [SerializeField]
    private float nextSoftCapComplaintTime = 0;

    [SerializeField]
    private float nextHardCapComplaintTime = 0;

    [Space(10)]
    [Header("-----Research Data-----")]
    [SerializeField]
    private float maxInkLevel = 12;
    
    [SerializeField]
    private float inkCheckCount;

    [SerializeField]
    private float currentInkLevel;

    [SerializeField]
    private bool overSoftCap;

    [SerializeField]
    private bool overHardCap;

    [SerializeField]
    private List<float> overHardCapList;

    [SerializeField]
    private List<float> overSoftCapList;

    [SerializeField]
    private float timeOverHardCap;

    [SerializeField]
    private float timeOverSoftCap;

    [SerializeField]
    private float avgInk; 

    [SerializeField]
    private float avgInkCounter;

    public GameObject Canvas_Tutorial_Skill;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {   
        // if(isStoic){
        //     _softCap = softCap2;
        //     _hardCap = hardCap2;
        // }else{
        //     _softCap = softCap1;
        //     _hardCap = hardCap1;
        // }
        movementSpeed = StartingMoveSpeed;
        agent = GetComponent<NavMeshAgent>();
        currentAction = CurrentAction.Waiting;
    }

    public void UpdateNumCollectedSprites(int num){
        currentInkLevel = num;
    
        if(currentInkLevel>_hardCap){
            if(!particesystem.isPlaying)particesystem.Play();
        }else{
            if(particesystem.isPlaying)particesystem.Stop();
        }
        
        if(currentInkLevel>_softCap){
            movementSpeed = Mathf.Max(minMoveSpeed,(1.0f-(num*0.1f))*StartingMoveSpeed);
        }else{
            movementSpeed = StartingMoveSpeed;
        }
    }

    public void StartCompanion(){
        currentAction = CurrentAction.Idle;
        countUI.gameObject.SetActive(true);
        Canvas_Tutorial_Skill.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Manager.Instance.GetGameState() != Manager.GameState.Playing) return;


        if(currentAction==CurrentAction.Waiting)return;

        HandleInkLevel();

        agent.speed = movementSpeed;
        currDistance = Vector3.Distance(transform.position, player.transform.position);


        //follow the player
        if(currDistance>minStartDistance){
            currentAction = CurrentAction.Following;
            Vector3 targetLocation = player.transform.position + UnityEngine.Random.insideUnitSphere*endLocationRadius;
            agent.SetDestination(targetLocation);

        }else{
            currentAction = CurrentAction.Idle;
        }


        //if(Input.GetKeyDown(KeyCode.F))CheckTarget();



        //change color of object depending on currentAction
        // switch(currentAction){
        //     case CurrentAction.Idle:
        //         GetComponent<MeshRenderer>().material.color = Color.blue;
        //         break;
        //     case CurrentAction.Following:
        //         GetComponent<MeshRenderer>().material.color = Color.red;
        //         break;
        // }
    }

    // private void CheckTarget(){
    //     Ray ray = mainCamera.ScreenPointToRay(centerdot.position);
    //     RaycastHit hit;

    //     if(Physics.Raycast(ray, out hit, 20.0f, spriteLayer)){
    //         Debug.Log("SPrite found :D");
    //         currentAction = CurrentAction.Collecting;
    //         agent.SetDestination(hit.transform.position);
    //         StartCoroutine(StartCollecting());
    //     }
    // }

    // private IEnumerator StartCollecting(){
    //     yield return new WaitForSeconds(collectionTime);
    //     currentAction = CurrentAction.Idle;
    // }

    private IEnumerator DisplayComplaint(){
        GenerateNewComplaint();
        dialogueUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(complaintDisplayTime);
        dialogueUI.gameObject.SetActive(false);
    }

    public void GenerateNewComplaint(){
        List<string> availableChoices = new List<string>(complaints);
        
        foreach (string str in previousComplaints){
            availableChoices.Remove(str);
        }
        string chosen = availableChoices[UnityEngine.Random.Range(0,availableChoices.Count)];

        complaintText.text = chosen;

        if(previousComplaints.Count >=2){
            previousComplaints.Dequeue();
        }
        previousComplaints.Enqueue(chosen);
    }

    private void HandleInkLevel(){
        if(currentInkLevel>=_hardCap){
            overSoftCap = false;
            if(!overHardCap){ //if just crossed cap
                 overHardCap = true;
            }
            timeOverHardCap+= Time.deltaTime;
            if(timeOverHardCap>=nextHardCapComplaintTime){
                nextHardCapComplaintTime += UnityEngine.Random.Range(20.0f, 40.0f);
                StartCoroutine(DisplayComplaint());
            }
        }else if (currentInkLevel>=_softCap){
            overHardCap = false;
            if(!overSoftCap){ //if just crossed cap
                overSoftCap = true;
            }
            timeOverSoftCap += Time.deltaTime;
            if(timeOverSoftCap>nextSoftCapComplaintTime){
                nextSoftCapComplaintTime += UnityEngine.Random.Range(12.0f, 24.0f);
                StartCoroutine(DisplayComplaint());
            }
        }else{
            overSoftCap = false;
            overHardCap = false;
        }
    }

    // private void LogData(){
    //     time += Time.deltaTime;
    //     if(time>=1.0f){ //log ink level every second
    //         time = 0;
    //         avgInk += currentInkLevel;
    //         avgInkCounter++;
    //     }

    //     if(currentInkLevel>=_hardCap){
    //         overSoftCap = false;
    //         if(!overHardCap){ //if just crossed cap
    //              overHardCap = true;
    //              overHardCapList.Add(Manager.Instance.GetTimeStamp());
    //         }
    //         timeOverHardCap+= Time.deltaTime;
    //         if(timeOverHardCap>=nextHardCapComplaintTime){
    //             if(isStoic){
    //                 nextHardCapComplaintTime += UnityEngine.Random.Range(20.0f, 40.0f);
    //             }else{
    //                 nextHardCapComplaintTime += UnityEngine.Random.Range(10.0f, 30.0f);
    //             }
    //             StartCoroutine(DisplayComplaint());
    //         }
    //     }else if (currentInkLevel>=_softCap){
    //         overHardCap = false;
    //         if(!overSoftCap){ //if just crossed cap
    //             overSoftCap = true;
    //              overSoftCapList.Add(Manager.Instance.GetTimeStamp());
    //         }
    //         timeOverSoftCap += Time.deltaTime;
    //         if(!isStoic && timeOverSoftCap>nextSoftCapComplaintTime){
    //             nextSoftCapComplaintTime += UnityEngine.Random.Range(20.0f, 40.0f);
    //             StartCoroutine(DisplayComplaint());
    //         }
    //     }else{
    //         overSoftCap = false;
    //         overHardCap = false;
    //     }
    // }

    // public void PrintData(){

    //     string clipboardcopy="";
    //     if(isStoic){
    //         clipboardcopy += "Stoic,";
    //     }else{
    //         clipboardcopy += "Sensitive,";
    //     }
    //     clipboardcopy+=avgInk/avgInkCounter + "," + timeOverSoftCap + "," + overSoftCapList.Count + ",";
    //     foreach(float timestamp in overSoftCapList){
    //         clipboardcopy+=timestamp+",";
    //     }
    //     clipboardcopy +="," + timeOverHardCap + "," + overHardCapList.Count+",";
    //     foreach(float timestamp in overHardCapList){
    //         clipboardcopy+=timestamp + ",";
    //     }
    //     clipboardcopy +="done";
    //     GUIUtility.systemCopyBuffer = clipboardcopy;

    //     //Format: Stoic, Average Ink Level, # times over soft cap, # times over hard cap, ,list of timestamps, done
    //     string path = Application.dataPath + "/data.txt";
    //     //Format: Stoic, Average Ink Level, # times over soft cap, # times over hard cap, ,list of timestamps, done
    //     //Debug.Log("File Written to: " + path);

    //     StreamWriter writer = new StreamWriter(path, false);
    //     if(isStoic){
    //         writer.Write("Stoic,");
    //     }else{
    //         writer.Write("Sensitive,");
    //     }
    //     writer.Write(avgInk/avgInkCounter+",");

    //     writer.Write(timeOverSoftCap+",");
    //     writer.Write(overSoftCapList.Count+",");
    //     foreach(float timestamp in overSoftCapList){
    //         writer.Write(timestamp+",");
    //     }

    //     writer.Write(",");
    //     writer.Write(timeOverHardCap+",");
    //     writer.Write(overHardCapList.Count+",");
    //     foreach(float timestamp in overHardCapList){
    //         writer.Write(timestamp+",");
    //     }

    //     writer.Write("done");
    //     writer.Close();

    //     Debug.Log("File written to: " + path);
    // }
}