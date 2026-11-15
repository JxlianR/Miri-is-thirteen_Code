using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChapterManager : MonoBehaviour
{
    public GameObject conflictDialogue, endingDialogue, credits;

    private InteractableObject[] interactableObjects; // Array with all interactable objects
    public MirisPhone phone;

    public GameObject conflictQuestEntry1, conflictQuestEntry2, conflictQuestText1, conflictQuestText2;
    public GameObject[] questEntries, readModeTexts;

    void Start()
    {
        Cursor.visible = true;
        interactableObjects = GameObject.FindObjectsOfType<InteractableObject>(); // Finds all objects with the interactableObject script and assigns them to the array
        CheckCompletedMinigames();
    }

    public void CheckCompletedMinigames()
    {
        // Checks if 3 minigames are completed -> Called after every quest line (through an event)
        if (MinigamesMaster.Instance.CompletedMinigames() == 3)
        {
            phone.ConflictEnqueue(conflictDialogue); 
            phone.animator.SetBool("Ringing", true);
            phone.blocked = true; // if blocked equals true, only the conflict queue is available on the phone
            phone.isRinging = true;     /// AUDIO.
            phone.SwapRingingAudioClipByRoom();     /// AUDIO.

            // blocks all objects that do not have the questline id 4
            foreach (InteractableObject obj in interactableObjects)
            {
                if (obj.GetComponent<InteractableObject>().questLineId != 4)
                {
                    obj.GetComponent<InteractableObject>().blocked = true;
                }
            }
        }

        // Checks if 6 minigames are completed
        if (MinigamesMaster.Instance.CompletedMinigames() == 6)
        {
            phone.TutorialEnqueue(endingDialogue);
            phone.animator.SetBool("Ringing", true);
            phone.isRinging = true;     /// AUDIO.
            phone.SwapRingingAudioClipByRoom();     /// AUDIO.
        }
    }

    // Gets called after the conflict questline ended
    public void EndConflictQuestLine()
    {
        // Unblocks all objects -> Player can interact with them again
        foreach (InteractableObject obj in interactableObjects)
        {
            obj.GetComponent<InteractableObject>().blocked = false;
        }

        // Enables Image component of all quest entries
        foreach (GameObject questEntry in questEntries)
        {
            questEntry.GetComponent<Image>().enabled = true;
        }

        // Enables all texts in the quest log read mode
        foreach (GameObject text in readModeTexts)
        {
            text.SetActive(true);
        }

        // Disables quest entries of the conflict questline
        conflictQuestEntry1.GetComponent<Image>().enabled = false;
        conflictQuestEntry2.GetComponent<Image>().enabled = false;

        // Disables read mode texts of the conflict questline
        conflictQuestText1.SetActive(false);
        conflictQuestText2.SetActive(false);

        // Unblocks the phone -> All queues are available
        phone.blocked = false;
    }

    // Unblocks all objects (Needed after the tutorial questline)
    public void UnblockObjects()
    {
        foreach (InteractableObject obj in interactableObjects)
        {
            obj.GetComponent<InteractableObject>().blocked = false;
        }
    }

    // Unblocks the phone
    public void UnblockPhone()
    {
        phone.blocked = false;
    }

    // Starts credits
    public void StartCredits()
    {
        credits.SetActive(true);
    }
}
