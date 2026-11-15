using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // int for delay before switching to menu
    public int waitBeforeMenu;

    public void GoToMenu()
    {
        StartCoroutine(WaitGoToMenu());
    }

    // Called through an event in the credits animation
    IEnumerator WaitGoToMenu()
    {
        yield return new WaitForSeconds(waitBeforeMenu);
        SceneManager.LoadScene(0); // Starts Menu
    }
}
