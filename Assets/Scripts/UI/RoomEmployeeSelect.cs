using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomEmployeeSelect : MonoBehaviour
{
    public ServiceQueue sQ;
    public Text employeeText;

    public Canvas roomUI;
    private EmployeeManager eM;
    
    // Start is called before the first frame update
    void Start()
    {
        eM = GameObject.Find("PathManager").GetComponent<EmployeeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sQ != null)
        {
            employeeText.text = sQ.curEmployees + " / " + sQ.maxEmployees;
        }

        if( (int)Camera.main.GetComponent<Player>().current_dir == 1 ) //positive Z
        {
            roomUI.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        else if ((int)Camera.main.GetComponent<Player>().current_dir == 2) //negative x
        {
            roomUI.transform.rotation = Quaternion.LookRotation(Vector3.left);
        }
        else if((int)Camera.main.GetComponent<Player>().current_dir == 3) // positive x
        {
            roomUI.transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
        else if ((int)Camera.main.GetComponent<Player>().current_dir == 4) //negative z
        {
            roomUI.transform.rotation = Quaternion.LookRotation(Vector3.back);
        }
        else
        {
            print("invalid camera/player direction...");
        }
        //Quaternion.LookRotation(transform.position - Camera.main.transform.position)
        //roomUI.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    //update room employee button to increase and decrease nodes

    public void addStaff()
    {
        sQ.increaseEmployee();
        eM.updateAllEmployees();
    }//end add staff

    public void removeStaff()
    {
        //room types: other, reception, bedroom, kitchen, dining, exit, hallway
        sQ.decreaseEmployee();
        eM.updateAllEmployees();
    }//end remove staff
}
