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

    float waitTimer = 0.0f;
    bool isWaiting;

    float serveTimer = 0.0f;
    bool isBeingServed;

    float curTimer = 0.0f;
    serviceTypes currentServiceState;

    // Start is called before the first frame update
    void Start()
    {
        angerTimer = 0.0f;
        gridRef = GameObject.Find("PathManager").GetComponent<CreateGrid>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //find nearest reception and queue up
        currentServiceState = serviceTypes.reception;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingServed)
        {
            isAngry = false;
            curTimer -= Time.deltaTime;
            if(curTimer < 0)
            {
                //move to next task
                if (currentServiceState == serviceTypes.reception)
                {
                    currentServiceState = serviceTypes.bedroom;
                    //travel to bedroom => wait on clean => sleep

                }
                else if (currentServiceState == serviceTypes.bedroom)
                {
                    currentServiceState = serviceTypes.kitchen;
                    //travel to kitchen => wait => eat

                }
                else if (currentServiceState == serviceTypes.kitchen)
                {
                    currentServiceState = serviceTypes.exit;
                    //travel to exit => destroy self
                    GetComponent<NodeList>().travel(gridRef.nodeGrid[0][0], destroyMe);
                }
                else
                {
                    print("npc is lost on improper service state");
                    print("exiting npc.....");
                    GetComponent<NodeList>().travel(gridRef.nodeGrid[0][0], destroyMe);
                }
            }
        }
        else if (isWaiting)
        {
            if(curTimer > 0.0f)
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
                isAngry = true;
                gameManager.decrement(20);
                //unoccupy hotel room
                GetComponent<NodeList>().travel(
                    gridRef.nodeGrid[0][0],
                    destroyMe
                );
            }
        }
    }

    void destroyMe()
    {
        Destroy(this);
    }
}
