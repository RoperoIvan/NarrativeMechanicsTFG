using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
    public Screens screenToGo;
    public AudioSource uiAS;
    private AudioClip click;
    private void Awake()
    {
        click = Resources.Load<AudioClip>("Sound/mainMenuBtnSound");
    }
    private void OnMouseDown()
    {
        uiAS.PlayOneShot(click);
        OpenScreen.openScreenManager.GoToScreen(screenToGo);
    }
}
