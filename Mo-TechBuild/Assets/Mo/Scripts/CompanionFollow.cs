using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AI;

public class CompanionFollow : MonoBehaviour
{

    enum CurrentAction{
        Idle,
        Following
    }

    CurrentAction currentAction = CurrentAction.Idle;

    public GameObject player;

    public ParticleSystem particesystem;

    [SerializeField]
    private float movementSpeed;

    public float StartingMoveSpeed;
    public float minStartDistance; //the distance away from player to START following

    public float minEndDistance;

    public float endLocationRadius;

    public float currDistance;

    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {   
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



        //change color of object depending on currentAction
        switch(currentAction){
            case CurrentAction.Idle:
                GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            case CurrentAction.Following:
                GetComponent<MeshRenderer>().material.color = Color.red;
                break;
        }
    }
}
