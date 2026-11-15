using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cameras : MonoBehaviour
{
    // Highest Priority at the start
    int highestPriority = 5;

    // Changes the virtual camera that is used by mainCamera by changing the priority
    public void ChangeVcam(CinemachineVirtualCamera newVcam)
    {
        highestPriority++;
        newVcam.Priority = highestPriority;
    }
}
