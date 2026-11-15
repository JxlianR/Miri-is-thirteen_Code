using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public bool interactable;
    public bool blocked;
    public bool triggersCloseUpText, repeatCloseUp; // Close-Up related bools (repeatCloseUp -> Object is always interactable and shows only the close-Up after the first time the player interacted)

    // How often an object was interacted with
    private int interacted;
    // Questline the object is part of
    public int questLineId;

    // GameObject that includes Sprites for Quest Log
    public GameObject enabledQuest, crossedOutQuest;

    // Flowchart that this Object can trigger
    public GameObject monologueFlowchart, monologueFlowchart2;

    [HideInInspector]
    public GameObject interactabilityIcon;

    public Questlog questlog;

    public UnityEvent onClickedEvents, onSecondClickedEvents;

    // Start is called before the first frame update
    void Start()
    {
        // Assigns the interactability icon (first child object) to GameObject
        interactabilityIcon = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if the object is blocked (In first and conflict quest line)
        // Enables Collider when object is not blocked and interactable
        if (blocked == false)
        {
            if (interactable == true)
            {
                gameObject.GetComponent<SphereCollider>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SphereCollider>().enabled = false;
                interactabilityIcon.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }

    public void OnClicked()
    {
        interacted++;
        interactable = false;
        interactabilityIcon.GetComponent<SpriteRenderer>().enabled = false; // Disable interactibility icon
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>().DisableControls(); // Disable Controls
        questlog.DisableQuestlog(); // Disable Quest Log

        // First time the player interacts with the object
        if (interacted == 1)
        {
            onClickedEvents.Invoke();

            // Activates the flowchart when one is assigned
            if (monologueFlowchart != null)
            {
                monologueFlowchart.SetActive(true);
                monologueFlowchart = null;
            }

            // Makes the object interactable when it shows only the close-up
            if (repeatCloseUp == true)
            {
                interactable = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>().interactableObject = null;
            }
        }
        else // Every time the player interacts after the first time
        {
            onSecondClickedEvents.Invoke();

            // Activates another flowchart if one is assigned
            if (monologueFlowchart2 != null)
            {
                monologueFlowchart2.SetActive(true);
            }

            // If the object is not belonging to quest line 5, it gets set to interactable (Razor in quest line 5 should not be interactable after interacting a second time)
            if (questLineId != 5)
            {
                interactable = true;
            }

            // Sets interactable Object to null to make the player not being able to interact with the object directly again
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>().interactableObject = null;
        }

        // Calls a function on a different script that changes the text of the close-up read mode
        // Only if this script is attached to the object and if it is a close-up object (triggersCloseUpText == true)
        if (triggersCloseUpText == true && gameObject.GetComponent<InteractableObjectCloseUp>() != null)
        {
            gameObject.GetComponent<InteractableObjectCloseUp>().StartCloseUpReadMode();
        }
    }

    // Makes object interactable
    public void Activate()
    {
        interactable = true;
    }

    // Makes object not interactable
    public void Deactivate()
    {
        interactable = false;
    }

    // Inserts a new quest into quest log
    public void newQuest()
    {
        questlog.UpdateQuestlog(enabledQuest);
    }

    // Changes sprite of finished quest to to crossed out sprite
    public void FinishedQuest()
    {
        questlog.CrossOutQuest(crossedOutQuest);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactabilityIcon.GetComponent<SpriteRenderer>().enabled = true; // Enables icon
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactabilityIcon.GetComponent<SpriteRenderer>().enabled = false; // Disables icon
        }
    }
}
