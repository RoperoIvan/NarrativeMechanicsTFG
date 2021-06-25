using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leak : MonoBehaviour
{
    public SpriteRenderer rSprite;
    public float repairValue = 0f;
    public Repair repairTool;
    public RepairController repairController;
    public float repairSpeed = 20f; 
    public bool isRepaired = true;
    public bool isActivate = false;
    public AudioSource buttonAS;
    //private AudioClip torch;
    //private bool isTorch = false;
    // Start is called before the first frame update
    private void Awake()
    {
        //torch = Resources.Load<AudioClip>("Sound/blowtorch");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetRepair()
    {
       //rSprite.color = new Color(0.3773585f, 0.2331791f, 0.2331791f); //REPAIRED
        rSprite.color = new Color(0.126157f, 0.2926755f, 0.5943396f); //BROKEN
        isRepaired = false;
        isActivate = true;
        repairValue = 0f;
    }

    public void Repairing()
    {
        //Debug.Log(repairValue);
        if(repairValue < 100)
        {
            repairValue += repairSpeed * Time.deltaTime;
        }   
        else
        {
            isRepaired = true;
            rSprite.color = new Color(0.1564614f, 0.1497864f, 0.1698113f);
            repairController.CheckRepair();
        }
    }

    private void OnMouseDown()
    {
        if(!isRepaired)
            buttonAS.Play();

        repairTool.clicking = true;
    }


    private void OnMouseUp()
    {
        repairTool.clicking = false;
        buttonAS.Stop();
    }
}
