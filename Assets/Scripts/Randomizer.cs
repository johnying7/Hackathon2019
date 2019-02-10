using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public GameObject[] objectList;
    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, objectList.Length);
        for (int i = 0; i < objectList.Length; i++)
        {
            objectList[i].SetActive(false);
        }
        objectList[index].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
