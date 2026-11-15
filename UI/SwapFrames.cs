using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapFrames : MonoBehaviour
{
    // Script to change, enable and disable Frames

    public Image characterPortrait;
    public Image overthinkingFrame1, overthinkingFrame2;
    public Image overthinkingPortrait1, overthinkingPortrait2;

    /// Minigame 06: Flirty Lines
    public Image characterPortraitMirror;
    public Image characterPortraitMoustache;
    public Image background;

    public GameObject razorIcon, readControl1, readControl2;

    // Changes Portrait of Miri
    public void SwapCharacterPortrait(Sprite portrait)
    {
        characterPortrait.GetComponent<Image>().sprite = portrait;
    }

    // Changes Frame in Overthinking Bubbles
    public void SwapOverthinkingFrame(Sprite overthinking)
    {
        overthinkingFrame1.GetComponent<Image>().sprite = overthinking;
        overthinkingFrame2.GetComponent<Image>().sprite = overthinking;
    }

    // Changes Portraits in the Mirror (Flirting Minigame)
    public void SwapCharacterPortraitMirror(Sprite portrait)
    {
        characterPortraitMirror.GetComponent<Image>().sprite = portrait;
    }

    // Changes Portrait with Moustache (Flirting Minigame)
    public void SwapCharacterPortraitMoustache(Sprite portrait)
    {
        characterPortraitMoustache.GetComponent<Image>().sprite = portrait;
    }

    // Changes Portrait of Miri during overthinking
    public void SwapCharacterPortraitOverthinking(Sprite portrait)
    {
        overthinkingPortrait1.GetComponent<Image>().sprite = portrait;
        overthinkingPortrait2.GetComponent<Image>().sprite = portrait;
    }

    // Changes icon of razor item
    public void SwapRazorIcon(Sprite icon)
    {
        razorIcon.GetComponent<SpriteRenderer>().sprite = icon;
        razorIcon.transform.localScale = new Vector3(0.07330005f, 0.07330005f, 0.07330005f); // Changes scale of razor icon (otherwise would be too small)
    }

    public void EnableBackground()
    {
        background.GetComponent<Image>().enabled = true;
    }

    public void DisableBackground()
    {
        background.GetComponent<Image>().enabled = false;
    }

    // Disables R to read text on Close-Up overlay
    public void DisableReadControl()
    {
        readControl1.SetActive(false);
        readControl2.SetActive(false);
    }

    // Enables R to read text on Close-Up overlay
    public void EnableReadControl()
    {
        readControl1.SetActive(true);
        readControl2.SetActive(true);
    }
}
