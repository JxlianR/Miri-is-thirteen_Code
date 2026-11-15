using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirisPhone : MonoBehaviour
{
    public bool interactable;

    [HideInInspector]
    public bool blocked;

    // Queues
    private Queue<GameObject> tutorialQueue, conflictQueue, shavingQueue, minigamesQueue, interactablesQueue;

    private GameObject interactabilityIcon;

    public Image background; // Background during dialogues

    public Animator animator;

    /// Sound related variables
    [Header("Audio")]
    public float phoneAudioFadingSpeedWhenChangingRooms = 0.5f;
    public AudioSource mirisRoomAudioSource;
    public AudioSource kitchenAudioSource;
    public AudioSource bathroomAudioSource;
    public AmbienceFadingBehavior ambienceFadingBehavior;

    public Sound mirisRoomSound;
    public Sound kitchenSound;
    public Sound bathroomSound;

    public SoundFadingBehavior soundFadingBehavior;

    [HideInInspector]
    public float maxVolume;
    [HideInInspector]
    public bool isRinging = false;

    public string currentAudioClipName;

    private void Awake()
    {
        /// AUDIO.
        maxVolume = mirisRoomAudioSource.volume;
        mirisRoomSound.InitSoundByType();
        kitchenSound.InitSoundByType();
        bathroomSound.InitSoundByType();

        soundFadingBehavior.Init(maxVolume, phoneAudioFadingSpeedWhenChangingRooms, phoneAudioFadingSpeedWhenChangingRooms);

        mirisRoomSound.SetCurrentVolumeState(SoundVolumeState.Muted);
        kitchenSound.SetCurrentVolumeState(SoundVolumeState.Muted);
        bathroomSound.SetCurrentVolumeState(SoundVolumeState.Muted);

        mirisRoomSound.audioSource.volume = 0f;
        kitchenSound.audioSource.volume = 0f;
        bathroomSound.audioSource.volume = 0f;
    }

    void Start()
    {
        // Assign the interactability icon (first child object) to GameObject
        interactabilityIcon = transform.GetChild(0).gameObject;

        // Declaring all Queues
        tutorialQueue = new Queue<GameObject>();
        conflictQueue = new Queue<GameObject>();
        shavingQueue = new Queue<GameObject>();
        minigamesQueue = new Queue<GameObject>();
        interactablesQueue = new Queue<GameObject>();
    }

    void Update()
    {
        /// Audio
        if (isRinging)
        {
            SwapRingingAudioClipByRoom();   /// AUDIO.
            soundFadingBehavior.UpdateVolumeByVolumeState(mirisRoomSound);
            soundFadingBehavior.UpdateVolumeByVolumeState(kitchenSound);
            soundFadingBehavior.UpdateVolumeByVolumeState(bathroomSound);
        }

        // Check if phone is not blocked -> Phone is blocked during the conflict quest line
        if (blocked == false)
        {
            // Check if one of the queues stores something
            if (tutorialQueue.Count != 0 || conflictQueue.Count != 0 || shavingQueue.Count != 0 || minigamesQueue.Count != 0 || interactablesQueue.Count != 0)
            {
                interactable = true; // Make Phone interactable
            }
            else
            {
                interactable = false; // Phone is not interactable
            }
        }
        else // phone is blocked -> only conflict queue is available
        {
            // Sets interactable to true if conflict queue store something
            if (conflictQueue.Count != 0)
            {
                interactable = true;
            }
            else
            {
                interactable = false;
            }
        }

        // Enables the collider when phone is interactable
        if (interactable == true)
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SphereCollider>().enabled = false; // Disables collider
            interactabilityIcon.GetComponent<SpriteRenderer>().enabled = false; // Disables icon
        }
    }


    // Functions to enqueue Flowchart into queue
    public void TutorialEnqueue(GameObject dialogueFlowchart)
    {
        tutorialQueue.Enqueue(dialogueFlowchart);
    }

    public void ConflictEnqueue(GameObject dialogueFlowchart)
    {
        conflictQueue.Enqueue(dialogueFlowchart);
    }

    public void ShavingEnqueue(GameObject dialogueFlowchart)
    {
        shavingQueue.Enqueue(dialogueFlowchart);
    }

    public void MinigamesEnqueue(GameObject dialogueFlowchart)
    {
        minigamesQueue.Enqueue(dialogueFlowchart);
    }

    public void InteractablesEnqueue(GameObject dialogueFlowchart)
    {
        interactablesQueue.Enqueue(dialogueFlowchart);
    }

    // Function plays the ringing animation
    public void TriggerAnimation()
    {
        animator.SetTrigger("Ringing");
        isRinging = true;     /// AUDIO.
    }

    public void OnClicked()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>().controlsEnabled = false; // Disable player controls
        GameObject.FindGameObjectWithTag("Questlog").GetComponent<Questlog>().questlogEnabled = false; // Disable questlog
        background.GetComponent<Image>().enabled = true; // Enable background

        animator.SetBool("Ringing", false); // Plays an empty animation

        /// AUDIO.
        isRinging = false;     
        mirisRoomSound.SetCurrentVolumeState(SoundVolumeState.Muted);
        kitchenSound.SetCurrentVolumeState(SoundVolumeState.Muted);
        bathroomSound.SetCurrentVolumeState(SoundVolumeState.Muted);
        mirisRoomSound.audioSource.volume = 0f;
        kitchenSound.audioSource.volume = 0f;
        bathroomSound.audioSource.volume = 0f;


        GameObject flowchart; // GameObject that temporarily stores the object that gets dequeued from a queue

        // Check if one of the queues stores something (in order of the priorities) and dequeue and set the GameObject to active if that is the case
        if (tutorialQueue.Count != 0)
        {
            flowchart = tutorialQueue.Dequeue();
            flowchart.SetActive(true);
        }
        else if (conflictQueue.Count != 0)
        {
            flowchart = conflictQueue.Dequeue();
            flowchart.SetActive(true);
        }
        else if (shavingQueue.Count != 0)
        {
            flowchart = shavingQueue.Dequeue();
            flowchart.SetActive(true);
        }
        else if (minigamesQueue.Count != 0)
        {
            flowchart = minigamesQueue.Dequeue();
            flowchart.SetActive(true);
        }
        else
        {
            flowchart = interactablesQueue.Dequeue();
            flowchart.SetActive(true);
        }
    }

    public void DisabelBackground()
    {
        background.GetComponent<Image>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactabilityIcon.GetComponent<SpriteRenderer>().enabled = true; // Enables background
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactabilityIcon.GetComponent<SpriteRenderer>().enabled = false; // Disables background
        }
    }

    /// AUDIO.
    public void SwapRingingAudioClipByRoom()
    {
        switch (ambienceFadingBehavior.currentAmbience)
        {
            case ActiveAmbience.MirisRoom:
                if(mirisRoomSound.CurrentVolumeState != SoundVolumeState.PlayingNormalVolume || mirisRoomSound.CurrentVolumeState != SoundVolumeState.FadingIn)
                {
                    //Debug.LogError($"SWAP PHONE RINGING AUDIO: currentAmbience = {ambienceFadingBehavior.currentAmbience}");
                    mirisRoomSound.SetCurrentVolumeState(SoundVolumeState.FadingIn);
                    kitchenSound.SetCurrentVolumeState(SoundVolumeState.FadingOut);
                    bathroomSound.SetCurrentVolumeState(SoundVolumeState.FadingOut);
                }
                break;

            case ActiveAmbience.Bathroom:
                if (bathroomSound.CurrentVolumeState != SoundVolumeState.PlayingNormalVolume || bathroomSound.CurrentVolumeState != SoundVolumeState.FadingIn)
                {
                    //Debug.LogError($"SWAP PHONE RINGING AUDIO: currentAmbience = {ambienceFadingBehavior.currentAmbience}");
                    mirisRoomSound.SetCurrentVolumeState(SoundVolumeState.FadingOut);
                    kitchenSound.SetCurrentVolumeState(SoundVolumeState.FadingOut);
                    bathroomSound.SetCurrentVolumeState(SoundVolumeState.FadingIn);
                }
                break;

            case ActiveAmbience.Kitchen:
                if (kitchenSound.CurrentVolumeState != SoundVolumeState.PlayingNormalVolume || kitchenSound.CurrentVolumeState != SoundVolumeState.FadingIn)
                {
                    //Debug.LogError($"SWAP PHONE RINGING AUDIO: currentAmbience = {ambienceFadingBehavior.currentAmbience}");
                    mirisRoomSound.SetCurrentVolumeState(SoundVolumeState.FadingOut);
                    kitchenSound.SetCurrentVolumeState(SoundVolumeState.FadingIn);
                    bathroomSound.SetCurrentVolumeState(SoundVolumeState.FadingOut);
                }
                break;
        }
    }
}
