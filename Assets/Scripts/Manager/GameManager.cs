using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float winAmount; //10 million
    public float startAmount; //testing for 5mil, default 50,000
    public float currentAmount;

    public ScrollList scrollList;

    public Transform winScreen;
    public Transform loseScreen;

    // Start is called before the first frame update
    void Start()
    {
        currentAmount = startAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void charge(float cash)
    {
        currentAmount += cash;
        scrollList.RefreshDisplay();
        checkWinOrLose();
    }

    private void checkWinOrLose()
    {
        //  WIN CONDITION
        if (currentAmount >= winAmount)
        {
            Time.timeScale = 0;
            // set win UI
            winScreen.gameObject.SetActive(true);
        }

        //  LOSE CONDITION
        if (currentAmount <= 0)
        {
            Time.timeScale = 0;
            // set lose UI
            loseScreen.gameObject.SetActive(true);
        }
    }
}
