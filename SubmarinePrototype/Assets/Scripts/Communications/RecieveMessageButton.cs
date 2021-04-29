using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieveMessageButton : MonoBehaviour
{
    private bool isButton = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().color = new Vector4(0.254717f, 0.05647027f, 0.05647027f, 1f); // HOVER
    }
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = new Vector4(0.4339623f, 0.09620862f, 0.09620862f, 1f); // NOTHOVER
    }
    private void OnMouseDown()
    {
        
    }
    private void OnMouseUp()
    {
        
    }
}
