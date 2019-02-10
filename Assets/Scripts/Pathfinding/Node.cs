using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    //intersection of 90 degrees = 10 sec delay
    //one unit is 0.0625 miles or one side of a block

    public Transform[] possiblePaths; //possible branching roads to break off into
    //   Forward/PositiveZ(0), Right/PositiveX(1), Back/NegativeZ(3), Left/NegativeX(4) => Declared False
    public int[] pathSpeeds = new int[1];

    public float final = 0.0f; //final = genPath + heuristic
    public float heuristic = 0.0f; //heuristic for distance from curPoint to destination
    public float genPath = 0.0f; //movement cost from startpoint to curpoint using generated path

    public Transform parentPath;

    public serviceTypes roomType;
    public facingDirection currentFaceDirection;

    // Start is called before the first frame update
    void Awake()
    {
        roomType = serviceTypes.hallway;
        currentFaceDirection = facingDirection.forward;

        possiblePaths = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            possiblePaths[i] = GameObject.Find("EmptyTransform").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setRoom(serviceTypes setRoom, facingDirection curFaceDirection)
    {
        //print("setRoom roomType: " + roomType);
        roomType = setRoom;
        currentFaceDirection = curFaceDirection;
        GetComponent<ServiceQueue>().initServices(setRoom);
        //do space check for available paths
    }

    public void removeRoom()
    {
        //do space check for possible paths
    }
}

public enum facingDirection { forward, right, back, left };
