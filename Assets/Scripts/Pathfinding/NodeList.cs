using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeList : MonoBehaviour
{

    Transform curNode; //current node that the person is at
    int curDestination;
    bool shouldMove;
    public float moveSpeed = 40.0f;
    Transform emptyTransform; //transform to establish a null or non-existent

    List<List<Transform>> myGrid = new List<List<Transform>>();

    List<Transform> openList = new List<Transform>();
    List<Transform> closedList = new List<Transform>();
    List<Transform> completePath = new List<Transform>();

    public Transform startPoint;
    Transform curPoint;
    public Transform destination;

    int shortestDistanceIndex = 0;
    float curShortestDistance = -1.0f;

    public delegate void npcDelegate();
    public npcDelegate m_newTask;

    //public Material[] colors = new Material[4];
    //baseColor(0), startColor(1), pathColor(2), endColor(3);

    //public InputField startField;
    //public InputField DestinationField;

    //public Text startText;
    //public Text DestinationText;

    // Use this for initialization
    void Start()
    {
        emptyTransform = GameObject.Find("EmptyTransform").transform;
        myGrid = GameObject.Find("PathManager").GetComponent<CreateGrid>().nodeGrid;

        curNode = myGrid[0][0];
        //travel(myGrid[9][9]);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            if (completePath[curDestination] == curNode)
            {
                if (curNode == completePath[completePath.Count - 1]) //destination reached
                {
                    openList.Clear();
                    closedList.Clear();
                    completePath.Clear();
                    curDestination = 0;
                    transform.position = curNode.position;
                    shouldMove = false;

                    //
                    //  CALL destination achieved methods here....
                    //
                    m_newTask();
                    //
                    //  END destination
                    //

                }
                else //go to the next position in the path
                {
                    curDestination++;
                    transform.LookAt(completePath[curDestination]);
                }
            }
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Node" && collision.transform == completePath[curDestination])
        {
            curNode = collision.transform;
        }
    }

    public void travel(Transform destinationNode, npcDelegate sentTask)
    {
        //print(destinationNode);
        //print(sentTask);
        m_newTask = sentTask;
        setStart();
        setDestination(destinationNode);
        calcRoute();
        shouldMove = true;
    }

    //Top Level
    void calcRoute()
    {
        //add startpoint to open list
        openList.Add(startPoint);

        //loop begin
        //find shortest distance node in openlist
        while (openList.Count > 0)
        {
            curPoint = findClosestNode();
            openList.Remove(curPoint);
            closedList.Add(curPoint);
            if (curPoint == destination)
                break;
            addToOpenList(curPoint);
        }
        if (curPoint == destination)
        {
            List<Transform> stack = new List<Transform>();
            fillStack(stack, curPoint);
            for (int i = 0; i < stack.Count; i++)
            {
                completePath.Add(stack[stack.Count - (i + 1)]);
                //completePath[i].GetChild(0).GetComponent<Renderer>().material.color = colors[2].color;
            }
            //completePath[0].GetChild(0).GetComponent<Renderer>().material.color = colors[1].color;
            //completePath[completePath.Count - 1].GetChild(0).GetComponent<Renderer>().material.color = colors[3].color;

            print("Found the fastest path to the destination.");
        }
        else if (openList.Count == 0)
        {
            print("No viable path found.");
        }
        else
            print("Unclear error with finding a path.");
    }

    void addToOpenList(Transform parent)
    {
        foreach (Transform path in parent.GetComponent<Node>().possiblePaths)
        {
            if (path != emptyTransform && !closedList.Contains(path))
            {
                if (!openList.Contains(path)) //checks if the surrounding nodes are not already on the open list
                {
                    openList.Add(path);
                    path.GetComponent<Node>().parentPath = parent; //used to solve for the chosen most efficient path

                    //h = heuristic for distance from curPoint to destination
                    path.GetComponent<Node>().heuristic = Vector3.Distance(curPoint.position, destination.position);
                    //g = movement cost from startpoint to curpoint using generated path
                    path.GetComponent<Node>().genPath = findCurPathDistance(path);
                    //f = g + h
                    path.GetComponent<Node>().final = path.GetComponent<Node>().genPath + path.GetComponent<Node>().heuristic;
                }
                else if (openList.Contains(path))
                {
                    //if this path to that square is better, change parent
                    if (path.GetComponent<Node>().genPath > findCurPathDistance(parent) + Vector3.Distance(parent.position, path.position))
                    {
                        path.GetComponent<Node>().parentPath = parent;
                        path.GetComponent<Node>().genPath = findCurPathDistance(path);
                        path.GetComponent<Node>().final = path.GetComponent<Node>().genPath + path.GetComponent<Node>().heuristic;
                    }
                }
            }
        }
    }

    //selects closest node to the destination node in the open list calculated by distance
    Transform findClosestNode()
    {
        shortestDistanceIndex = 0;
        for (int i = 0; i < openList.Count; i++)
        {
            //print("openList[i]: " + openList[i]);
            //print("destination: " + destination);
            float temp = Vector3.Distance(openList[i].position, destination.position);
            if (curShortestDistance == -1.0f)
            {
                curShortestDistance = temp;
                shortestDistanceIndex = 0;
            }
            else if (curShortestDistance > temp)
            {
                curShortestDistance = temp;
                shortestDistanceIndex = i;
            }
        }
        return openList[shortestDistanceIndex];
    }

    float findCurPathDistance(Transform baseNode)
    {
        if (baseNode == startPoint)
        {
            return 0.0f;
        }
        else
        {
            return (Vector3.Distance(baseNode.position, baseNode.GetComponent<Node>().parentPath.position) +
                findCurPathDistance(baseNode.GetComponent<Node>().parentPath));
        }
    }

    void fillStack(List<Transform> stackList, Transform baseNode)
    {
        if (baseNode != startPoint)
        {
            stackList.Add(baseNode);
            fillStack(stackList, baseNode.GetComponent<Node>().parentPath);
        }
        else
            stackList.Add(baseNode);
    }

    public void setStart()
    {
        startPoint = curNode;
    }

    public void setDestination(Transform destinationNode)
    {
        //Debug.Log(destinationNode.position);
        destination = destinationNode;
    }

}
