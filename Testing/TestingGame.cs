using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to test the game more easily
public class TestingGame : MonoBehaviour
{
    public bool startGameNormally;
    public bool enablePlayerControls;
    public bool enableQuestlog;
    public bool disableAllFlowcharts;
    public bool unblockAllObjects;

    public PlayerControls player;
    public Questlog questlog;
    public ChapterManager manager;
    public GameObject firstFlowchart;
    public GameObject flowchartToTest;

    private GameObject[] flowcharts; // Array with all flowcharts

    // Start is called before the first frame update
    void Start()
    {
        flowcharts = GameObject.FindGameObjectsWithTag("Flowchart"); // Assigns all gameObjects with the tag "Flowchart" to the array

        // Starts the game from the beginning
        if(startGameNormally == true)
        {
            StartGame();
            return;
        }

        // Enables the player controls
        if (enablePlayerControls == true)
        {
            player.controlsEnabled = true;
        }

        // Enables the quest log
        if (enableQuestlog == true)
        {
            questlog.questlogEnabled = true;
        }

        // Disables all flowcharts
        if (disableAllFlowcharts == true)
        {
            foreach (GameObject flowchart in flowcharts)
            {
                flowchart.SetActive(false);
            }
        }

        // If a flowchart is assigned it gets enabled and starts when the game begins
        if (flowchartToTest != null)
        {
            TestFlowchart();
        }

        // Unblocks all interactable objects
        if (unblockAllObjects == true)
        {
            manager.UnblockObjects();
        }
    }

    void StartGame()
    {
        player.controlsEnabled = false; // Disables controls
        questlog.questlogEnabled = false; // Disables quest log

        // Disables all flowcharts
        foreach (GameObject flowchart in flowcharts)
        {
            flowchart.SetActive(false);
        }

        firstFlowchart.SetActive(true); // Enables the first flowchart
    }

    void TestFlowchart()
    {
        // Disables all flowcharts
        foreach (GameObject flowchart in flowcharts)
        {
            flowchart.SetActive(false);
        }

        flowchartToTest.SetActive(true); // Enables the flowchart that needs to be tested
    }
}
