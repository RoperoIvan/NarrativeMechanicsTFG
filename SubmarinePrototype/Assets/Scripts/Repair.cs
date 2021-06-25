using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Repair : MonoBehaviour
{

    private Camera myMainCamera;
    private Vector3 mousePosition;

    [HideInInspector]
    public bool clicking = false;
    public bool isTorch = false;

    private void OnEnable()
    {
        Cursor.visible = false;
    }
    private void OnDisable()
    {
        Cursor.visible = true;
    }
    private void Start()
    {
        myMainCamera = Camera.main;
    }

    private void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition = myMainCamera.ScreenToWorldPoint(mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Leak") && clicking)
        {
            collision.gameObject.GetComponent<Leak>().Repairing();
        }
    }
}
