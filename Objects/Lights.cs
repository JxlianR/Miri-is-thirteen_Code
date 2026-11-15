using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    // Array with all dark planes of one room
    public GameObject[] darkPlanes;

    // Sound related variable
    public bool isMirisRoomLightCollider = true;

    private void OnTriggerEnter(Collider other)
    {
        // Starts coroutine to disable dark planes when the player enters the collider
        if(other.tag == "Player")
        {
            StartCoroutine(EnableLightSwitchWithDelay(false, SoundAssets.Instance.lightSwitchSoundDelay));

            /// AUDIO.
            if (isMirisRoomLightCollider)
            {
                isMirisRoomLightCollider = false;
            }
            else
            {
                StartCoroutine(PlayLightSwitchSoundWithDelay(SoundAssets.Instance.lightSwitchSoundDelay));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Starts coroutine to enable dark planes when the player exits the collider
        if (other.tag == "Player")
        {
            StartCoroutine(EnableLightSwitchWithDelay(true, SoundAssets.Instance.lightSwitchSoundDelay));
        }
    }

    // Enables/Disables dark plane after a small delay
    private IEnumerator EnableLightSwitchWithDelay(bool isEnabled, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject plane in darkPlanes)
        {
            plane.GetComponent<SpriteRenderer>().enabled = isEnabled;
        }
    }

    // Sound
    private IEnumerator PlayLightSwitchSoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log($"Play Light Switch Sound");
        SoundManager.PlayOneShotSoundByType("LightSwitch", SoundType.SFX); /// AUDIO.
    }
}
