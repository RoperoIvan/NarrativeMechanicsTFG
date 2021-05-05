using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : MonoBehaviour
{
    public SpriteRenderer rSprite;
    private void OnMouseDown()
    {
        rSprite.color = new Color(0.3773585f, 0.2331791f, 0.2331791f);
        //new Color(0.126157f, 0.2926755f, 0.5943396f);
    }

    public void ResetRepair()
    {
        rSprite.color = new Color(0.126157f, 0.2926755f, 0.5943396f);
    }
}
