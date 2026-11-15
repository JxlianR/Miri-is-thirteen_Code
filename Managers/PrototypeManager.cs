using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeManager : MonoBehaviour
{
    public GameObject flowchart;

    public int waitForSeconds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndApplication()
    {
        flowchart.SetActive(true);
        StartCoroutine(End());
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(waitForSeconds);
        Application.Quit();
    }
}
