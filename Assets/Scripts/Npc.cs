using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    float angerTimer;
    bool isAngry;
    public float maxAngerTime = 30.0f;
    CreateGrid gridRef;
    GameManager gameManager;

    //float waitTimer = 0.0f;
    public bool isWaiting;

    //float serveTimer = 0.0f;
    public bool isBeingServed;

    public float curTimer = 0.0f;
    public serviceTypes currentServiceState;
    public ServiceQueue currentServiceQueue;

    private Transform onDestinationCall;

    // Start is called before the first frame update
    void Start()
    {
        angerTimer = 0.0f;
        gridRef = GameObject.Find("PathManager").GetComponent<CreateGrid>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //find nearest reception and queue up
        currentServiceState = serviceTypes.reception;
        onDestinationCall = findNextDestination();
        onDestinationCall.GetComponent<ServiceQueue>().customerTraveling.Add(this.transform);
        GetComponent<NodeList>().travel(onDestinationCall, startWaitCall);
        currentServiceQueue = onDestinationCall.GetComponent<ServiceQueue>();
        //GetComponent<NodeList>().travel(gridRef.nodeGrid[8][8], startWaitCall);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingServed)
        {
            isAngry = false;
            curTimer -= Time.deltaTime;
            //print("being serviced");
            Transform dummyDestination = gridRef.nodeGrid[1][1];
            if(curTimer <= 0)
            {
                //move to next task
                
                if (currentServiceState == serviceTypes.reception)
                {
                    currentServiceQueue.customerServed();
                    currentServiceState = serviceTypes.bedroom;
                    onDestinationCall = findNextDestination();
                    //remove from old service queue

                    //travel to bedroom => wait on clean => sleep
                    //customerServed() call here....
                    gameManager.charge(100.0f);
                    Camera.main.transform.GetComponent<ObjectPlacer>().PlaySfx(0);
                    currentServiceQueue = onDestinationCall.GetComponent<ServiceQueue>();
                    //print("onDestinationCall: " + onDestinationCall.position);
                    //print("currentServiceQueuePos: " + currentServiceQueue.transform.position);
                    //print("Go to Bedroom serviceQueueType: " + currentServiceQueue.serviceType);
                    onDestinationCall.GetComponent<ServiceQueue>().customerTraveling.Add(this.transform);
                    GetComponent<NodeList>().travel(onDestinationCall, startWaitCall);
                }
                else if (currentServiceState == serviceTypes.bedroom)
                {
                    currentServiceQueue.customerServed();
                    currentServiceState = serviceTypes.kitchen;
                    onDestinationCall = findNextDestination();
                    
                    //travel to kitchen => wait => eat
                    currentServiceQueue = onDestinationCall.GetComponent<ServiceQueue>();
                    onDestinationCall.GetComponent<ServiceQueue>().customerTraveling.Add(this.transform);
                    GetComponent<NodeList>().travel(onDestinationCall, startWaitCall);
                }
                else if (currentServiceState == serviceTypes.kitchen)
                {
                    currentServiceQueue.customerServed();
                    currentServiceState = serviceTypes.exit;
                    onDestinationCall = findNextDestination();
                    
                    //travel to exit => destroy self
                    //currentServiceQueue = onDestinationCall.GetComponent<ServiceQueue>();
                    //onDestinationCall.GetComponent<ServiceQueue>().customerTraveling.Add(this.transform);
                    GetComponent<NodeList>().travel(onDestinationCall, destroyMe);
                }
                else
                {
                    currentServiceQueue.customerServed();
                    currentServiceState = serviceTypes.exit;
                    onDestinationCall = findNextDestination();
                    print("npc is lost on improper service state");
                    print("exiting npc.....");
                    currentServiceQueue = onDestinationCall.GetComponent<ServiceQueue>();
                    GetComponent<NodeList>().travel(onDestinationCall, destroyMe);
                }
                isBeingServed = false;
            }
        }
        else if (isWaiting)
        {
;            if(curTimer > 0.0f)
            {
                curTimer -= Time.deltaTime;
            }
            else
            {
                isAngry = true;
            }
        }

        if (isAngry)
        {
            angerTimer += Time.deltaTime;

            if(angerTimer >= maxAngerTime)
            {
                print("A customer got angry and left...");
                currentServiceQueue.removeCustomer(this.transform);
                gameManager.charge(-20);
                //unoccupy hotel room
                currentServiceState = serviceTypes.exit;
                GetComponent<NodeList>().travel(onDestinationCall, destroyMe);
                isAngry = false;
            }
            
        }
    }

    public Transform findNextDestination()
    {
        Transform newDestination = gridRef.getBestCustomerRoom(this.transform, currentServiceState);
        //Debug.Log("destination id: " + newDestination.gameObject.GetInstanceID().ToString());
        //Debug.Log(newDestination.position);
        //return gridRef.findClosestFreeRoomNode(this.transform, currentServiceState);
        return newDestination;
    }

    public void startWaitCall()
    {
        //print("startWaitCall called");
        onDestinationCall.GetComponent<ServiceQueue>().addNewCustomer(this.transform);
    }

    void destroyMe()
    {
        Destroy(this.gameObject);
    }
}
