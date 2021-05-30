using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iconEv : MonoBehaviour
{
    public GameObject iconParent;
    public float timeEvent = 0f;
    private bool beginAnim = false;
    private bool disap = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        iconParent = GameObject.Find("Icons Container");
    }

    // Update is called once per frame
    void Update()
    {
        if(beginAnim)
        {
            if (Time.realtimeSinceStartup - timeEvent >= 5f && Time.realtimeSinceStartup - timeEvent <= 8f)
            {
                GetComponent<Animator>().SetBool("isFive", true);
            }
            if (Time.realtimeSinceStartup - timeEvent >= 8f)
            {
                GetComponent<Animator>().SetBool("isFive", false);
                GetComponent<Animator>().SetBool("isEight", true);

            }
            if (Time.realtimeSinceStartup - timeEvent >= 10f)
            {
                Destroy(gameObject);
            }
        }
    }
    public void ChangeParent()
    {
        transform.SetParent(iconParent.transform);
        timeEvent = Time.realtimeSinceStartup;
        beginAnim = true;
    }

    public void Disappear()
    {
        disap = !disap;

        GetComponent<Image>().enabled = disap;
    }
}
