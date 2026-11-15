using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Gives the player the opportunity to display text of the questlog in a normal font
public class ReadMode : MonoBehaviour
{
    public GameObject questlogReadMode, closeUpReadMode, whiteboardCloseUpReadMode; // All read mode overlays

    public Questlog questlog;

    // Text variables
    public TMP_Text[] questTexts; // Array with all text fields
    public TMP_Text conflictQuestText1, conflictQuestText2; // Text fields for conflict quest line
    public TMP_Text closeUpText, closeUpControls1, closeUpControls2, questlogControls; // Text for close-up and control texts at the bottom

    private bool questlogOpen, closeUpOpen, whiteboardCloseUpOpen, open, invokable;

    void Start()
    {
        open = false;
        //gameIsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        questlogOpen = questlog.open;
        invokable = questlog.questlogEnabled;

        // Checks if the quest log is open, the player can open/close it (only needed because of one monologue that is playing while ques log is open)
        // if the game is not paused and for input
        if (questlogOpen == true && invokable == true && Input.GetKeyDown(KeyCode.R) && !PauseMenu.gameIsPaused)
        {
            open = !open; // Changes open to the opposite state
            questlogReadMode.SetActive(open); // Enables the quest log read mode

            // Disables "R to read" text on quest log when read mode is open
            if (open == true)
            {
                questlogControls.GetComponent<TextMeshProUGUI>().enabled = false;
            }
            else // Enables text when read mode is closed
            {
                questlogControls.GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }

        // Checks if a close-up is currently open and for input of player
        if(closeUpOpen == true && Input.GetKeyDown(KeyCode.R) && !PauseMenu.gameIsPaused)
        {
            open = !open;
            closeUpReadMode.SetActive(open); // Enables close-up read mode

            // Disables control texts on close-up overlay when read mode is open
            if (open == true)
            {
                closeUpControls1.GetComponent<TextMeshProUGUI>().enabled = false;
                closeUpControls2.GetComponent<TextMeshProUGUI>().enabled = false;
            }
            else // Enables the texts when read mode is closed
            {
                closeUpControls1.GetComponent<TextMeshProUGUI>().enabled = true;
                closeUpControls2.GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }

        // Checks if whiteboard close-up is open, for input and if game is not paused
        if (whiteboardCloseUpOpen == true && Input.GetKeyDown(KeyCode.R) && !PauseMenu.gameIsPaused)
        {
            open = !open;
            whiteboardCloseUpReadMode.SetActive(open); // Enables close-up read mode

            if (open == true)
            {
                closeUpControls1.GetComponent<TextMeshProUGUI>().enabled = false;
                closeUpControls2.GetComponent<TextMeshProUGUI>().enabled = false;
            }
            else
            {
                closeUpControls1.GetComponent<TextMeshProUGUI>().enabled = true;
                closeUpControls2.GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }

        // Checks if none of quest log or the close-ups are open and disables every read mode
        if (questlogOpen == false && closeUpOpen == false && whiteboardCloseUpOpen == false)
        {
            open = false;
            questlogReadMode.SetActive(open);
            closeUpReadMode.SetActive(open);
            whiteboardCloseUpReadMode.SetActive(open);
        }
    }

    // Inserting a new text
    public void NewText(string newText, int textField)
    {
        questTexts[textField].text = newText;
    }

    // Changing font style of a text field to make the text crossed out
    public void CrossOutText(string oldText)
    {
        foreach(TMP_Text text in questTexts)
        {
            if (text.text == oldText)
            {
                text.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough; // Changing the font style to strikethrough
                return;
            }
        }
    }

    // Changes quest log read mode according to the conflict quest line
    public void ConflictReadMode()
    {
        // Disables every quest entry
        foreach(TMP_Text questText in questTexts)
        {
            questText.gameObject.SetActive(false);
        }

        // Enables the two quest entries needed fpr the conflict quest line
        conflictQuestText1.gameObject.SetActive(true);
        conflictQuestText2.gameObject.SetActive(true);
    }

    // Gets called at the end of a close-up flowchart
    public void EndCloseUpReadMode()
    {
        closeUpOpen = false;
        open = false;
        closeUpReadMode.SetActive(false); // Disables close-up read mode
        closeUpText.text = ""; // Makes the text field empty
    }

    // Changes text in close-up read mode
    public void SetCloseUpText(string newCloseUpText)
    {
        closeUpText.text = newCloseUpText;
        closeUpOpen = true;
    }

    // Gets called at the end of the whiteboard flowchart
    public void EndWhiteboardCloseUoReadMode()
    {
        whiteboardCloseUpOpen = false;
        open = false;
        closeUpReadMode.SetActive(false); // Disables whiteboard read mode
    }

    public void SetWhiteboardCloseUpReadMode()
    {
        whiteboardCloseUpOpen = true;
    }
}
