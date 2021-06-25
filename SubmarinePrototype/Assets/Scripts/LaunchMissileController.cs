using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMissileController : MonoBehaviour
{
    //public GameManager gameManager;
    public GameObject hole;
    public AudioSource uiAS;
    private AudioClip leverRotate;
    private AudioClip openMissile;
    private bool isrotate = false;
    private bool isLeft = false;
    private bool isOpen = false;
    private int rotateValues = 0;
    private void Awake()
    {
        leverRotate = Resources.Load<AudioClip>("Sound/missileLever");
        openMissile = Resources.Load<AudioClip>("Sound/openMissile");
        
    }
    private void OnMouseDown()
    {
        isrotate = true;
        //gameManager.ExecuteFinal(1);
        
    }
    private void OnMouseUp()
    {
        isrotate = false;

    }
    private void Update()
    {
        if(isrotate && isOpen == false)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
            );

            float curAngle = transform.eulerAngles.z;
            transform.up = direction;

            float desiredAngle = transform.eulerAngles.z;

            float angleDif = desiredAngle - curAngle;

            if (angleDif < 0)
                angleDif += 360;

            if (angleDif > 0 && angleDif < 180)
            {
                isLeft = true;
            }
            else
            {
                isLeft = false;
            }
        }
    }
    public void OpenDoor()
    {
        uiAS.PlayOneShot(openMissile, 0.5f);
        transform.parent.GetComponent<Animator>().SetBool("isOpen", true);
        Debug.Log("Open");
        hole.GetComponent<CircleCollider2D>().enabled = true;
        hole.GetComponent<MissileHole>().startChecking = true;
        isOpen = true;
    }
    public void PutMissile()
    {
    }

    public void AimMissile()
    {

    }

    public void LaunchMissile()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLeft)
            rotateValues--;
        else
            rotateValues++;
        uiAS.PlayOneShot(leverRotate);
        //Debug.Log(rotateValues);
        if (rotateValues > 30)
            OpenDoor();
    }
}
