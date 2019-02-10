using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> tabs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        setTab(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTab(int input)
    {
        for(int i = 0; i < tabs.Count; i++)
        {
            if(i == input)
            {
                tabs[i].SetActive(true);
                if(tabs[i].GetComponent<EmployeeManager>() != null)
                {

                }
            } else
            {
                tabs[i].SetActive(false);
            }
        }
    }
}
