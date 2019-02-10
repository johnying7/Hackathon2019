using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public float rotateSpeed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rotateSpeed == 0.0f)
        {
            transform.Rotate(100.0f * Vector3.up * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Rotate(rotateSpeed * Vector3.up * Time.deltaTime, Space.World);
        }
        
    }
}
