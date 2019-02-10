using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Staff Types: Wait / Server, Cleaning, Cook
/// </summary>
public class Staff : ScriptableObject
{
    public int busyStaff;
    public int totalStaff;
    public Text staffText;

    public void add()
    {
        totalStaff++;
        updateText();
        //totalEmployees++;
        //updateCookText();
    }

    public void remove()
    {
        totalStaff--;
        updateText();
        //totalEmployees--;
        //updateWaitText();
    }

    public void updateText()
    {
        staffText.text = busyStaff.ToString() + " / " + totalStaff.ToString();
    }
}
