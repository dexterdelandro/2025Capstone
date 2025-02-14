using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;


public class CompanionFollow : MonoBehaviour
{

    public float collectionTime;

    public RectTransform centerdot;
    public Camera mainCamera;

    public LayerMask spriteLayer;
    enum CurrentAction{
        Idle,
        Following,
        Collecting
    }

    CurrentAction currentAction = CurrentAction.Idle;

    public GameObject player;

    public ParticleSystem particesystem;

    public List<String>complaints;

    public float complaintDisplayTime;

    [SerializeField]
    private Canvas dialogueUI;

    public TMP_Text complaintText;
    private float time;

    [Header("-----Movement Variables-----")]
    [SerializeField]
    private float movementSpeed;

    public float StartingMoveSpeed;
    public float minStartDistance; //the distance away from player to START following

    public float minEndDistance;

    public float endLocationRadius;

    public float currDistance;

    public string fileName = "/Data/LogFile.txt";

    [Space(10)]
    [Header("-----Ink Caps-----")]
    [SerializeField]
    private float softCap1;

    [SerializeField]
    private float hardCap1;

    [SerializeField]
    private float softCap2;

    [SerializeField]
    private float hardCap2;

    public bool isStoic;

    public float _softCap;

    public float _hardCap;


    [SerializeField]
    private float nextSoftCapComplaintTime = 0;

    [SerializeField]
    private float nextHardCapComplaintTime = 0;

    [Space(10)]
    [Header("-----Research Data-----")]
    [SerializeField]
    private float totalInkLevel;
    
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
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {   
        if(isStoic){
            _softCap = softCap2;
            _hardCap = hardCap2;
        }else{
            _softCap = softCap1;
            _hardCap = hardCap1;
        }
        movementSpeed = StartingMoveSpeed;
        agent = GetComponent<NavMeshAgent>();
    }

    public void UpdateNumCollectedSprites(int num){
        if(num==5){
            if(!particesystem.isPlaying)particesystem.Play();
        }else{
            if(particesystem.isPlaying)particesystem.Stop();
        }
        if(num>0){
            movementSpeed = (1.0f-(num*0.1f))*StartingMoveSpeed;
        }else{
            movementSpeed = StartingMoveSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Manager.Instance.GetGameState() != Manager.GameState.Playing) return;

        LogData();

        if(currentAction==CurrentAction.Collecting)return;

        agent.speed = movementSpeed;
        currDistance = Vector3.Distance(transform.position, player.transform.position);


        //follow the player
        if(currDistance>minStartDistance){
            currentAction = CurrentAction.Following;
            Vector3 targetLocation = player.transform.position + Random.insideUnitSphere*endLocationRadius;
            agent.SetDestination(targetLocation);

        }else{
            currentAction = CurrentAction.Idle;
        }


        if(Input.GetKeyDown(KeyCode.F))CheckTarget();



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

    private void CheckTarget(){
        Ray ray = mainCamera.ScreenPointToRay(centerdot.position);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 20.0f, spriteLayer)){
            Debug.Log("SPrite found :D");
            currentAction = CurrentAction.Collecting;
            agent.SetDestination(hit.transform.position);
            StartCoroutine(StartCollecting());
        }
    }

    private IEnumerator StartCollecting(){
        yield return new WaitForSeconds(collectionTime);
        currentAction = CurrentAction.Idle;
    }

    private void LogData(){
        time += Time.deltaTime;
        if(time>=1.0f){ //log ink level every second
            time = 0;
            avgInk += currentInkLevel/100.0f;
        }

        if(currentInkLevel>=_hardCap){
            overSoftCap = false;
            if(!overHardCap){ //if just crossed cap
                 overHardCap = true;
                 overHardCapList.Add(Manager.Instance.GetTimeStamp());
            }
            timeOverHardCap+= Time.deltaTime;
            if(timeOverHardCap>=nextHardCapComplaintTime){
                if(isStoic){
                    nextHardCapComplaintTime += UnityEngine.Random.Range(20.0f, 40.0f);
                }else{
                    nextHardCapComplaintTime += UnityEngine.Random.Range(10.0f, 30.0f);
                }
                StartCoroutine(DisplayComplaint());
            }
        }else if (currentInkLevel>=_softCap){
            overHardCap = false;
            if(!overSoftCap){ //if just crossed cap
                overSoftCap = true;
                 overSoftCapList.Add(Manager.Instance.GetTimeStamp());
            }
            timeOverSoftCap += Time.deltaTime;
            if(!isStoic && timeOverSoftCap>nextSoftCapComplaintTime){
                nextSoftCapComplaintTime += UnityEngine.Random.Range(20.0f, 40.0f);
                StartCoroutine(DisplayComplaint());
            }
        }else{
            overSoftCap = false;
            overHardCap = false;
        }
    }

    public void PrintData(){

        string clipboardcopy="";
        if(isStoic){
            clipboardcopy += "Stoic,";
        }else{
            clipboardcopy += "Sensitive,";
        }
        clipboardcopy+=avgInk/avgInkCounter + "," + timeOverSoftCap + "," + overSoftCapList.Count + ",";
        foreach(float timestamp in overSoftCapList){
            clipboardcopy+=timestamp+",";
        }
        clipboardcopy +="," + timeOverHardCap + "," + overHardCapList.Count+",";
        foreach(float timestamp in overHardCapList){
            clipboardcopy+=timestamp + ",";
        }
        clipboardcopy +="done";
        GUIUtility.systemCopyBuffer = clipboardcopy;

        //Format: Stoic, Average Ink Level, # times over soft cap, # times over hard cap, ,list of timestamps, done
        string path = Application.dataPath + "/data.txt";
        //Debug.Log("File Written to: " + path);

        StreamWriter writer = new StreamWriter(filePath, false);
        if(isStoic){
            writer.Write("Stoic,");
        }else{
            writer.Write("Sensitive,");
        }
        writer.Write(avgInk/avgInkCounter+",");

        writer.Write(timeOverSoftCap+",");
        writer.Write(overSoftCapList.Count+",");
        foreach(float timestamp in overSoftCapList){
            writer.Write(timestamp+",");
        }

        writer.Write(",");
        writer.Write(timeOverHardCap+",");
        writer.Write(overHardCapList.Count+",");
        foreach(float timestamp in overHardCapList){
            writer.Write(timestamp+",");
        }

        writer.Write("done");
        writer.Close();

        Debug.Log("File written to: " + filePath);
    }
}
