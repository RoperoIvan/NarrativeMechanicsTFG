using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHole : MonoBehaviour
{
    public GameManager gameManager;
    public bool startChecking = false;
    private void OnMouseDown()
    {
        if(startChecking)
            gameManager.ExecuteFinal(1);
    }
}
