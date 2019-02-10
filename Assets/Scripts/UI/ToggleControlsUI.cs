using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControlsUI : MonoBehaviour
{
    public Transform controls;
    public Transform intro;

    private bool canSeeControls;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            controls.gameObject.SetActive(!controls.gameObject.activeSelf);
            intro.gameObject.SetActive(!intro.gameObject.activeSelf);
        }
    }
}
