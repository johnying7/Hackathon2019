using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceQueue : MonoBehaviour
{
    
    public serviceTypes serviceType;

    public int curEmployees; //ex: 3 for kitchen
    int maxEmployees;

    float maxWaitTime;
    float maxServeTime;

    List<Transform> customerWaiting = new List<Transform>();
    List<Transform> customerServing = new List<Transform>();
    List<float> waitTimer = new List<float>();
    List<float> serveTimer = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        if(serviceType == serviceTypes.reception)
        {
            maxEmployees = 1;
            maxWaitTime = 10.0f;
            maxServeTime = 2.0f;
        }
        else if (serviceType == serviceTypes.kitchen)
        {
            maxEmployees = 3;
            maxWaitTime = 15.0f;
            maxServeTime = 5.0f;
        }
        else //should not be called
        {
            print("unspecified service type");
            maxEmployees = 0;
            maxWaitTime = 1.0f;
            maxServeTime = 1.0f;
        }

        curEmployees = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //for(int i = 0; i < customerServing.Count; i++)
        //{
        //    timer[i] -= Time.deltaTime;
        //}

        //for (int i = 0; i < customerServing.Count; i++)
        //{
        //    timer.RemoveAt(0);
        //    customerServing.RemoveAt(0);
        //}
    }

    public void increaseEmployee()
    {
        if(curEmployees < maxEmployees)
        {
            curEmployees++;
            startServingCustomer();
        }
    }

    public void decreaseEmployee()
    {
        if (curEmployees > 0)
        {
            if(curEmployees == customerServing.Count)
            {
                kickServingCustomer();
            }
            curEmployees--;
        }
    }

    public void addNewCustomer(Transform newWaitingCustomer)
    {
        customerWaiting.Add(newWaitingCustomer);
        float newWaitTime = new float();
        newWaitTime = maxWaitTime;
        waitTimer.Add(newWaitTime);
    }

    public void removeCustomer(Transform angryCustomer)
    {
        for(int i = 0; i < customerWaiting.Count; i++)
        {
            if(customerWaiting[i] == angryCustomer)
            {
                customerWaiting.RemoveAt(i);
                waitTimer.RemoveAt(i);
                Npc angryNpc = angryCustomer.GetComponent<Npc>();
                
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

            float newServeTime = waitTimer[0];
            waitTimer.RemoveAt(0);

            customerServing.Add(newServingCustomer);
            newServeTime = maxServeTime;

            serveTimer.Add(newServeTime);
        }
    }

    void kickServingCustomer()
    {
        if (customerServing.Count > 0)
        {
            Transform newWaitingCustomer = customerServing[customerServing.Count-1];
            customerServing.RemoveAt(customerServing.Count-1);

            float newWaitTime = serveTimer[serveTimer.Count-1];
            serveTimer.RemoveAt(serveTimer.Count - 1);

            customerWaiting.Insert(0, newWaitingCustomer);
            newWaitTime = maxWaitTime;
            
            waitTimer.Add(newWaitTime);
        }
    }

    void customerServed()
    {
        //find unoccupied hotel room and travel to it
    }
}

public enum serviceTypes { other, reception, bedroom, kitchen, exit, hallway };