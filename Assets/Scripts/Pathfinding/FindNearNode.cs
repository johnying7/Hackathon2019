using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNearNode : MonoBehaviour
{
    CreateGrid searchList;
    // Start is called before the first frame update
    void Start()
    {
        searchList = GameObject.Find("PathManager").GetComponent<CreateGrid>();
    }

    public Transform getNearestSnap(Vector3 clickedPosition)
    {
        Transform nearestTrans = searchList.nodeGrid[0][0];
        float shortestDistance = Vector3.Distance(clickedPosition, searchList.nodeGrid[0][0].position);
        for (int i = 0; i < searchList.nodeGrid.Count; i++)
        {
            for(int j = 0; j < searchList.nodeGrid[i].Count; j++)
            {
                float temp = Vector3.Distance(clickedPosition, searchList.nodeGrid[i][j].position);
                if(temp < shortestDistance)
                {
                    shortestDistance = temp;
                    nearestTrans = searchList.nodeGrid[i][j];
                }
            }
        }
        return nearestTrans;
    }
}
