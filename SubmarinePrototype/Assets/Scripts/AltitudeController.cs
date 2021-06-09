using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeController : MonoBehaviour
{
    public GameObject topBound;
    public GameObject botBound;
    public GameObject baseShip;
    public GameObject baseSubmarine;
    public GameObject flagShip;
    public GameObject flagSubmarine; 
    public GameObject poleSubmarine;
    public GameObject poleShip;
    public GameObject launcher;

    public bool isSurface = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == topBound)
        {
            isSurface = true;
            baseShip.SetActive(true);
            baseSubmarine.SetActive(true);
            flagShip.SetActive(true);
            flagSubmarine.SetActive(true);
            poleSubmarine.SetActive(true);
            poleShip.SetActive(true);
            launcher.SetActive(true);
        }

        if (collision.gameObject == botBound)
        {
            isSurface = false;
            baseShip.SetActive(false);
            baseSubmarine.SetActive(false);
            flagShip.SetActive(false);
            flagSubmarine.SetActive(false);
            poleSubmarine.SetActive(false);
            poleShip.SetActive(false);
            launcher.SetActive(false);
        }
    }
}
