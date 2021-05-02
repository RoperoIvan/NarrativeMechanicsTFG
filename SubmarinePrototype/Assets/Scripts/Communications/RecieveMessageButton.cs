using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieveMessageButton : MonoBehaviour
{
    //private bool isButton = false;
    public bool activateButton = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //isButton = true;
        GetComponent<SpriteRenderer>().color = new Vector4(0.254717f, 0.05647027f, 0.05647027f, 1f); // HOVER
    }

    private void OnMouseUp()
    {
        //isButton = false;
        GetComponent<SpriteRenderer>().color = new Vector4(0.4339623f, 0.09620862f, 0.09620862f, 1f); // NOTHOVER
    }
}
