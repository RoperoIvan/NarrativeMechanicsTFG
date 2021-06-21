using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherManager : MonoBehaviour
{
    public VisualCommunicationController visualCommunicationController;
    public void SetBase()
    {
        visualCommunicationController.shipBaseAnimator.SetBool("hasBase",visualCommunicationController.hasBase);
    }
    public void SetCannon()
    {
        visualCommunicationController.shipCannonAnimator.SetBool("hasCannon", visualCommunicationController.hasCannon);
    }
}
