using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
    public Screens screenToGo;
    private void OnMouseDown()
    {
        OpenScreen.openScreenManager.GoToScreen(screenToGo);
    }
}
