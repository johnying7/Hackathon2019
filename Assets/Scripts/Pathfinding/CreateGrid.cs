using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    //each room is 10 units by 10 units
    //xSize and zSize declare how many rooms in a row you can place
    public int squareSize = 10;
    public int xSize;
    public int zSize;

    public Transform nodePrefab;
    public List<List<Transform>> nodeGrid = new List<List<Transform>>();

    public List<Transform>[] roomList = new List<Transform>[7]; //array size of serviceTypes

    public Terrain terrain;

    public Transform customer;

    private Transform emptyTransform;

    private void Awake()
    {
        xSize = squareSize;
        zSize = squareSize;
        for (int i = 0; i < 7; i++)
        {
            roomList[i] = new List<Transform>();
        }
        initGrid(); //creates grid list
        terrain.terrainData.size = new Vector3(xSize * 10, 0, zSize * 10);
    }

    // Start is called before the first frame update
    void Start()
    {
        initPossiblePaths(); //sets connections
        emptyTransform = GameObject.Find("EmptyTransform").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            Vector3 modifiedPos = nodeGrid[0][0].position;
            modifiedPos.y += 0.6f;
            //Instantiate(customer, modifiedPos, nodeGrid[0][0].rotation);
            Transform newCustomer = Instantiate(customer, modifiedPos, nodeGrid[0][0].rotation);
            newCustomer.Translate(modifiedPos);
        }
    }

    public Transform getBestCustomerRoom(Transform npcPosition, serviceTypes roomType)
    {

        Transform bestRoom = emptyTransform;
        float shortestDistance = Vector3.Distance(npcPosition.position, bestRoom.position);
        float tempDistance;

        Transform fullBestRoom = emptyTransform;
        float fullShortestDistance = Vector3.Distance(npcPosition.position, bestRoom.position);
        float fullTempDistance;

        //first pass for rooms that have non-waiting spots for the customer
        for (int i = 0; i < roomList[(int)roomType].Count; i++)
        {
            ServiceQueue sQ = roomList[(int)roomType][i].GetComponent<ServiceQueue>();
            //print("customer serving + traveling < current employees");
            //print(sQ.customerServing.Count + " + " + sQ.customerTraveling.Count + " < " + sQ.curEmployees);
            if (sQ.customerServing.Count + sQ.customerTraveling.Count < sQ.curEmployees)
            {
                //print("succeeded in integrating traveling employees?");
                if (bestRoom == emptyTransform)
                {
                    bestRoom = roomList[(int)roomType][i];
                }
                else
                {
                    tempDistance = Vector3.Distance(npcPosition.position, roomList[(int)roomType][i].position);
                    if (tempDistance < shortestDistance)
                    {
                        bestRoom = roomList[(int)roomType][i];
                        shortestDistance = tempDistance;
                    }
                }
            }
            else //if the room has all busy staff members
            {
                if(fullBestRoom == emptyTransform)
                {
                    fullBestRoom = roomList[(int)roomType][i];
                    fullTempDistance = Vector3.Distance(npcPosition.position, roomList[(int)roomType][i].position);
                }
                else
                {
                    fullTempDistance = Vector3.Distance(npcPosition.position, roomList[(int)roomType][i].position);
                    if (fullTempDistance < fullShortestDistance)
                    {
                        fullBestRoom = roomList[(int)roomType][i];
                        fullShortestDistance = fullTempDistance;
                    }
                }

            }
        }

        if(bestRoom != emptyTransform)
        {
            return bestRoom;
        }
        else if (fullBestRoom != emptyTransform)
        {
            return fullBestRoom;
        }
        else
        {
            //print("no available rooms for customer to go to!!!");
            //print("write code for customer waiting queue until room available and manned");
            Debug.LogWarning("no available rooms for customer to go to!!!\n" +
                "write code for customer waiting queue until room available and manned\n" +
                "sending customer to emptyTransform...");

            return nodeGrid[0][0];
        }
    }

    /// <summary>
    /// references need to be changed to getBestCustomerRoom (deprecated)
    /// </summary>
    /// <param name="basePoint"></param>
    /// <param name="roomType"></param>
    /// <returns></returns>
    public Transform findClosestFreeRoomNode(Transform basePoint, serviceTypes roomType)
    {
        Transform bestRoom = emptyTransform;
        float shortestDistance = Vector3.Distance(basePoint.position, bestRoom.position);
        float tempDistance;
        for (int i = 0; i < nodeGrid.Count; i++)
        {
            for(int j = 0; j < nodeGrid[i].Count; j++)
            {
                if(nodeGrid[i][j].GetComponent<Node>().roomType == roomType)
                {
                    if (bestRoom == emptyTransform)
                    {
                        bestRoom = nodeGrid[i][j];
                    } else
                    {
                        tempDistance = Vector3.Distance(basePoint.position, nodeGrid[i][j].position);
                        if (tempDistance < shortestDistance)
                        {
                            bestRoom = nodeGrid[i][j];
                            shortestDistance = tempDistance;
                        }
                    }
                }
            }
        }

        if( bestRoom == GameObject.Find("EmptyTransform").transform)
        {
            return nodeGrid[0][0];
        }
        return bestRoom;
    }

    void initGrid()
    {
        for (int i = 0; i < xSize; i++)
        {
            List<Transform> nodeGridRow = new List<Transform>();
            for (int j = 0; j < zSize; j++)
            {
                Transform newNode = Instantiate(nodePrefab, new Vector3((i * 10)  +5, 0, (j * 10) + 5), Quaternion.identity).transform;
                nodeGridRow.Add(newNode);
            }
            nodeGrid.Add(nodeGridRow);
        }
        nodeGrid[0][0].GetComponent<Node>().roomType = serviceTypes.exit;
        roomList[(int)serviceTypes.exit].Add(nodeGrid[0][0]);
    } 

    void initPossiblePaths()
    {
        for (int i = 0; i < xSize; i++) //sets border possible paths, outside is null
        {
            for (int j = 0; j < zSize; j++)
            {
                if (i > 0)
                {
                    nodeGrid[i][j].GetComponent<Node>().possiblePaths[3] = nodeGrid[i - 1][j];
                }

                if (i < xSize - 1)
                {
                    nodeGrid[i][j].GetComponent<Node>().possiblePaths[1] = nodeGrid[i + 1][j];
                }

                if (j > 0)
                {
                    nodeGrid[i][j].GetComponent<Node>().possiblePaths[0] = nodeGrid[i][j - 1];
                }

                if (j < zSize - 1)
                {
                    nodeGrid[i][j].GetComponent<Node>().possiblePaths[2] = nodeGrid[i][j + 1];
                }
            }
        }
    }
}
