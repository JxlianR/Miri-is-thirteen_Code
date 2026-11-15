using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectCloseUp : MonoBehaviour
{
    public ReadMode readMode;

    [TextArea(15, 20)]
    public string closeUpText;

    // Sets the text of the read mode to the text of the close-up object
    public void StartCloseUpReadMode()
    {
        readMode.SetCloseUpText(closeUpText);
    }
}
