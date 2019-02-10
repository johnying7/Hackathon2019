using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// room types: other(0), reception(1), bedroom(2), kitchen(3), dining(4), exit(5), hallway(6)
/// </summary>p
public enum serviceTypes { other, reception, bedroom, kitchen, dining, exit, hallway };

public class ServiceQueue : MonoBehaviour
{
    public serviceTypes serviceType;

    public int curEmployees; //ex: 3 for kitchen
    public int maxEmployees;

    float maxWaitTime;
    float maxServeTime;

    public List<Transform> customerTraveling = new List<Transform>();
    public List<Transform> customerWaiting = new List<Transform>();
    public List<Transform> customerServing = new List<Transform>();

    public bool isRoomFull;

    private Transform pM;

    // Start is called before the first frame update
    void Start()
    {
        pM = GameObject.Find("PathManager").transform;
    }

    // Update is called once per frame
    void Update()
    {

        //check for fill serving customer
        if(curEmployees > customerServing.Count && customerWaiting.Count > 0)
        {
            //TODO: remove customer from traveling list
            startServingCustomer();
        }
    }

    public void fillService()
    {
        if (serviceType == serviceTypes.reception)
        {
            maxEmployees = 2;
            maxWaitTime = 10.0f;
            maxServeTime = 2.0f;
        }
        else if (serviceType == serviceTypes.kitchen)
        {
            maxEmployees = 3;
            maxWaitTime = 15.0f;
            maxServeTime = 5.0f;
        }
        else if (serviceType == serviceTypes.bedroom)
        {
            //curEmployees = 1;
            maxEmployees = 1;
            maxWaitTime = 1.0f;
            maxServeTime = 15.0f;
        }
        else //should not be called
        {
            print("unspecified service type");
            maxEmployees = 0;
            maxWaitTime = 1.0f;
            maxServeTime = 1.0f;
        }
    }

    public void increaseEmployee()
    {
        if(curEmployees < maxEmployees && pM.GetComponent<EmployeeManager>().isStaffAvailable(serviceType))
        {
            curEmployees++;
            pM.GetComponent<EmployeeManager>().hireStaff(serviceType);
            startServingCustomer();
        }
    }

    public void decreaseEmployee()
    {
        if (curEmployees > 0 && pM.GetComponent<EmployeeManager>().canFireStaff(serviceType))
        {
            if(curEmployees == customerServing.Count)
            {
                kickServingCustomer();
            }
            curEmployees--;
            pM.GetComponent<EmployeeManager>().fireStaff(serviceType);
        }
    }

    public void addNewCustomer(Transform newWaitingCustomer) //add customer to waiting
    {
        //find the customer in the traveling list and remove
        for(int i = 0; i < customerTraveling.Count; i++)
        {
            if(newWaitingCustomer == customerTraveling[i])
            {
                customerTraveling.RemoveAt(i);
                break;
            }
        }

        Npc myNpc = newWaitingCustomer.GetComponent<Npc>();
        if (serviceType == serviceTypes.bedroom)
        {
            isRoomFull = true;
            myNpc.isBeingServed = false;
            myNpc.curTimer = maxWaitTime;
            myNpc.isWaiting = true;
            
        }
        else
        {
            isRoomFull = false;
            myNpc.isWaiting = true;
            myNpc.isBeingServed = false;
            myNpc.curTimer = maxWaitTime;
            //print("added new customer with maxWaitTime: " + maxWaitTime);
            //print("modified customer with curTime: " + myNpc.curTimer);
        }

        customerWaiting.Add(newWaitingCustomer);        
    }

    public void removeCustomer(Transform angryCustomer)
    {
        for(int i = 0; i < customerWaiting.Count; i++)
        {
            if(customerWaiting[i] == angryCustomer)
            {
                customerWaiting.RemoveAt(i);                
                break;
            }
        }
    }

    void startServingCustomer()
    {
        if(customerWaiting.Count > 0 && customerServing.Count < maxEmployees)
        {
            Transform newServingCustomer = customerWaiting[0];
            customerWaiting.RemoveAt(0);
            customerServing.Add(newServingCustomer);
            Npc curNpc = newServingCustomer.GetComponent<Npc>();
            curNpc.curTimer = maxServeTime;
            curNpc.isBeingServed = true;
            curNpc.isWaiting = false;
        }
    }

    void kickServingCustomer()
    {
        if (customerServing.Count > 0)
        {
            Transform newWaitingCustomer = customerServing[customerServing.Count-1];
            customerServing.RemoveAt(customerServing.Count-1);

            customerWaiting.Insert(0, newWaitingCustomer);
            newWaitingCustomer.GetComponent<Npc>().curTimer = maxWaitTime;
        }
    }

    public void customerServed()
    {
        //remove customer
        if(customerServing.Count > 0)
            customerServing.RemoveAt(0);
    }

    public void initServices()
    {
        if (serviceType == serviceTypes.other)
        {
            print("trying to initialize service without proper serviceType");
        }
        fillService();
    }

    public void initServices(serviceTypes newServiceType)
    {
        serviceType = newServiceType;
        fillService();
    }
    
}