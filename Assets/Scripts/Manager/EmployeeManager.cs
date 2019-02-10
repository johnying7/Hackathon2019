using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeManager : MonoBehaviour
{

    public int totalEmployees;

    public int busyWaitStaff;
    public int waitStaff;
    public Text waitText;

    public int busyCleaningStaff;
    public int cleaningStaff;
    public Text cleaningText;

    public int busyCookStaff;
    public int cookStaff;
    public Text cookText;

    public int availableEmployees;
    public int workingEmployees;
    public Text totalEmployeeText;

    private List<List<Transform>> nodeGrid = new List<List<Transform>>();

    private GameManager gM;

    // Start is called before the first frame update
    void Start()
    {
        nodeGrid = GameObject.Find("PathManager").GetComponent<CreateGrid>().nodeGrid;
        updateAllEmployees();
        //exampleStaff = ScriptableObject.CreateInstance<Staff>();
        gM = GameObject.Find("GameManager").GetComponent<GameManager>();
        print("implement npc angry code");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateAllEmployees()
    {
        updateWaitText();
        updateCleaningText();
        updateCookText();
    }

    private void updateTotalEmployeeText()
    {
        workingEmployees = busyWaitStaff + busyCookStaff + busyCleaningStaff;
        totalEmployeeText.text = workingEmployees.ToString() + " / " + totalEmployees.ToString();
    }

    public void updateWaitText()
    {
        waitText.text = busyWaitStaff.ToString() + " / " + waitStaff.ToString();
        updateTotalEmployeeText();
    }

    public void updateCleaningText()
    {
        cleaningText.text = busyCleaningStaff.ToString() + " / " + cleaningStaff.ToString();
        updateTotalEmployeeText();
    }

    public void updateCookText()
    {
        cookText.text = busyCookStaff.ToString() + " / " + cookStaff.ToString();
        updateTotalEmployeeText();
    }

    private void updateWorkingEmployees()
    {
        busyWaitStaff = 0;
        busyCookStaff = 0;
        busyCleaningStaff = 0;

        for(int i = 0; i < nodeGrid.Count; i++)
        {
            for(int j = 0; j < nodeGrid[i].Count; j++)
            {
                ServiceQueue sQ = nodeGrid[i][j].GetComponent<ServiceQueue>();
                serviceTypes currentService = sQ.serviceType;
                
                if (currentService != serviceTypes.other && currentService != serviceTypes.exit && currentService != serviceTypes.hallway)
                {
                    //room types: other, reception, bedroom, kitchen, dining, exit, hallway
                    switch (currentService)
                    {
                        case serviceTypes.reception:
                            busyWaitStaff += sQ.curEmployees;
                            break;
                        case serviceTypes.bedroom:
                            busyCleaningStaff += sQ.curEmployees;
                            break;
                        case serviceTypes.kitchen:
                            busyCookStaff += sQ.curEmployees;
                            break;
                        case serviceTypes.dining:
                            busyWaitStaff += sQ.curEmployees;
                            break;
                        default:
                            //do nothing
                            break;
                    }
                }
            }
        }
        workingEmployees = busyWaitStaff + busyCookStaff + busyCleaningStaff;
    }

    /// <summary>
    /// fix later
    /// </summary>
    /// <param name="staff"></param>
    public void addStaff(Staff staff)
    {
        totalEmployees++;
        staff.add();
        updateTotalEmployeeText();
    }

    /// <summary>
    /// fix later
    /// </summary>
    /// <param name="staff"></param>
    public void removeStaff(Staff staff)
    {
        totalEmployees--;
        staff.remove();
        updateTotalEmployeeText();
    }

    public void addCook()
    {
        gM.charge(-100.0f);
        cookStaff++;
        totalEmployees++;
        updateCookText();
    }

    public void addWait()
    {
        gM.charge(-100.0f);
        waitStaff++;
        totalEmployees++;
        updateWaitText();
    }

    public void addCleaning()
    {
        gM.charge(-100.0f);
        cleaningStaff++;
        totalEmployees++;
        updateCleaningText();
    }

    public void removeCook()
    {
        if (busyCookStaff == cookStaff)
        {
            print("you must remove employee from a room to fire them");
        }
        if (cookStaff > 0 && busyCookStaff < cookStaff)
        {
            cookStaff--;
            totalEmployees--;
            updateCookText();
        }
    }

    public void removeWait()
    {
        if (busyWaitStaff == waitStaff)
        {
            print("you must remove employee from a room to fire them");
        }
        if (waitStaff > 0 && busyWaitStaff < waitStaff)
        {
            waitStaff--;
            totalEmployees--;
            updateWaitText();
        }
    }

    public void removeCleaning()
    {
        if (busyCleaningStaff == cleaningStaff)
        {
            print("you must remove employee from a room to fire them");
        }
        if (cleaningStaff > 0 && busyCleaningStaff < cleaningStaff)
        {
            cleaningStaff--;
            totalEmployees--;
            updateCleaningText();
        }
    }

    /// <summary>
    /// Based on employee type number, returns a boolean if one is available to start working.
    /// </summary>p
    /// /// <param name="employeeType">Refer to serviceTypes enum for complete list</param>
    public bool isStaffAvailable(serviceTypes employeeType)
    {
        /// room types: other(0), reception(1), bedroom(2), kitchen(3), dining(4), exit(5), hallway(6)
        switch (employeeType)
        {
            case serviceTypes.reception:
                return busyWaitStaff < waitStaff;
                //break;
            case serviceTypes.bedroom:
                return busyCleaningStaff < cleaningStaff;
                //break;
            case serviceTypes.kitchen:
                return busyCookStaff < cookStaff;
                //break;
            case serviceTypes.dining:
                return busyWaitStaff < waitStaff;
                //break;
            default:
                Debug.LogError("Invalid staff check.");
                return false;
                //break;
        }
    }

    public bool canFireStaff(serviceTypes employeeType)
    {
        /// room types: other(0), reception(1), bedroom(2), kitchen(3), dining(4), exit(5), hallway(6)
        switch (employeeType)
        {
            case serviceTypes.reception:
                return 0 < busyWaitStaff && busyWaitStaff <= waitStaff;
            //break;
            case serviceTypes.bedroom:
                return 0 < busyCleaningStaff && busyCleaningStaff <= cleaningStaff;
            //break;
            case serviceTypes.kitchen:
                return 0 < busyCookStaff && busyCookStaff <= cookStaff;
            //break;
            case serviceTypes.dining:
                return 0 < busyWaitStaff && busyWaitStaff <= waitStaff;
            //break;
            default:
                Debug.LogError("Invalid staff fire check.");
                return false;
                //break;
        }
    }

    public void fireStaff(serviceTypes employeeType)
    {
        switch (employeeType)
        {
            case serviceTypes.reception:
                busyWaitStaff--;
                break;
            case serviceTypes.bedroom:
                busyCleaningStaff--;
                break;
            case serviceTypes.kitchen:
                busyCookStaff--;
                break;
            case serviceTypes.dining:
                busyWaitStaff--;
                break;
            default:
                Debug.LogError("Invalid staff fire.");
                break;
        }
        updateTotalEmployeeText();
    }

    public void hireStaff(serviceTypes employeeType)
    {
        switch (employeeType)
        {
            case serviceTypes.reception:
                busyWaitStaff++;
                break;
            case serviceTypes.bedroom:
                busyCleaningStaff++;
                break;
            case serviceTypes.kitchen:
                busyCookStaff++;
                break;
            case serviceTypes.dining:
                busyWaitStaff++;
                break;
            default:
                Debug.LogError("Invalid staff fire.");
                break;
        }
        updateTotalEmployeeText();
    }
}
