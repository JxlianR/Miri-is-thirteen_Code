using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questlog : MonoBehaviour
{
    [HideInInspector]
    public bool open;
    public bool questlogEnabled;

    // Array of all Images for quests
    public Image[] quests;

    public Animator questlogAnimator, newQuestAnimator;

    public ReadMode readMode;

    // Start is called before the first frame update
    void Start()
    {
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks for Input at sets open to the opposite; Plays proper animation
        if (questlogEnabled == true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                open = !open;
                questlogAnimator.SetBool("Open", open);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>().controlsEnabled = !open; // Enables/Disables Controls
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>().DisableWalkingAnimation(); // Stops Walking Animation

                /// Audio.
                if (open && !PauseMenu.gameIsPaused)
                {
                    SoundManager.PlayOneShotSoundByType("Questlog_PickUp", SoundType.UI); /// AUDIO. 
                }
                else if(!open && !PauseMenu.gameIsPaused)
                {
                    SoundManager.PlayOneShotSoundByType("Questlog_PutDown", SoundType.UI); /// AUDIO. 
                }
            }
        }
    }

    // Function gets called through Unity Event or InteractableObject script
    // Sets the sprite of one of the Images in the array "quests" equals to the sprite of another gameObject
    public void UpdateQuestlog(GameObject newQuest)
    {
        StartCoroutine(NewQuestAnimation()); // Plays animation for getting a new quest

        var quest = quests[newQuest.GetComponent<QuestEntry>().textField];
        quest.GetComponent<Image>().sprite = newQuest.GetComponent<QuestEntry>().questEntry; // Changing sprite
        readMode.NewText(newQuest.GetComponent<QuestEntry>().readModeText, newQuest.GetComponent<QuestEntry>().textField); // Adding text to read mode

        // Setting alpha value to 1
        Color newColor = quest.GetComponent<Image>().color;
        newColor.a = 1;
        quest.GetComponent<Image>().color = newColor;
    }

    // Function gets called through Unity Event or InteractableObject script
    // Changes sprite of finished quest to to crossed out sprite
    public void CrossOutQuest(GameObject crossedOutQuest)
    {
        // Goes through each Image of the quest entries and checks if one is the same as replacedQuest
        foreach (Image quest in quests)
        {
            if (quest.GetComponent<Image>().sprite == crossedOutQuest.GetComponent<QuestEntryCrossedOut>().replacedQuest)
            {
                quest.GetComponent<Image>().sprite = crossedOutQuest.GetComponent<QuestEntryCrossedOut>().questEntry; // Changes sprite to crossed out version
                readMode.CrossOutText(crossedOutQuest.GetComponent<QuestEntryCrossedOut>().readModeText); // Calls function in readMode script to cross out text
                return;
            }
        }
    }

    // Opens the questlog
    public void OpenQuestlog()
    {
        open = true;
        questlogAnimator.SetBool("Open", open); // Plays opening animation
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>().controlsEnabled = !open;
    }

    public void EnableQuestlog()
    {
        questlogEnabled = true;
    }

    public void DisableQuestlog()
    {
        questlogEnabled = false;
    }

    // Changes quest log according to conflict quest line
    public void ConflictQuestLog()
    {
        // Goes through each Image and disables the Image component of those with a sprite
        foreach (Image quest in quests)
        {
            if (quest.GetComponent<Image>().sprite != null)
            {
                quest.GetComponent<Image>().enabled = false;
            }
        }
    }

    // Plays new quest animation
    private IEnumerator NewQuestAnimation()
    {
        yield return new WaitForSeconds(2.5f);
        newQuestAnimator.SetTrigger("NewQuest");
        SoundManager.PlayOneShotSoundByType("QuestLog_NewEntryAnimation", SoundType.SFX); /// AUDIO. 
    }
}
