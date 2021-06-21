using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairController : MonoBehaviour
{
    public TimeLineController timeLine;
    public float floodTime = 30f;
    private float timer = 0f;
    public List<GameObject> leaks;
    public GameManager gameManager;
    private bool startLeaking = false;
    public bool hasRepaired = false;
    public GameObject water;
    // Start is called before the first frame update
    void Start()
    {
        //SetLeaks();
    }

    private void OnEnable()
    {
        water.GetComponent<Animator>().SetBool("isDrowning", startLeaking);
    }
    // Update is called once per frame
    void Update()
    {
        if(startLeaking)
        {
            if(Time.realtimeSinceStartup - timer >= floodTime)
            {
                startLeaking = false;
                hasRepaired = false;
                GameManager.isEnd = true;
                gameManager.ExecuteFinal(4);
                Debug.Log("NOTREPAIRED");
            }
        }
    }

    public void SetLeaks()
    {
        int rLeaks = Random.Range(1, leaks.Count);
        timer = Time.realtimeSinceStartup;
        int[] rn = new int[rLeaks];
        for (int i = 0; i < rLeaks; ++i)
        {
            rn[i] = Random.Range(0, rn.Length-1);
        }

        for(int i = 0; i < rLeaks; ++i)
        {
            leaks[rn[i]].GetComponent<Leak>().ResetRepair();
        }
        startLeaking = true;
        water.GetComponent<Animator>().SetBool("isDrowning", true);
    }

    public void CheckRepair()
    {
        int r = 0;
        foreach (GameObject leak in leaks)
        {
            
            
            if (leak.GetComponent<Leak>().isActivate && !leak.GetComponent<Leak>().isRepaired)
            {
                r = 1;
                break;
            }
                
        }

        if (r == 1) //Not all repaired
        {

        }
        else //All repaired
        {
            timeLine.doAttack = true;
            timeLine.timerNextEvent = Time.realtimeSinceStartup;
            startLeaking = false;
            hasRepaired = true;
            water.GetComponent<Animator>().SetBool("isDrowning", false);
            foreach (GameObject leak in leaks)
            {
                leak.GetComponent<Leak>().isActivate = false;
            }
        }
    }
}
