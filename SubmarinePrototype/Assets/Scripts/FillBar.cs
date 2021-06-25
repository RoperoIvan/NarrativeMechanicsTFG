using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBar : MonoBehaviour
{
    public GameObject filling;
    private Coroutine fill;
    public bool stop = false;
    public void BeginFilling(float timeEv)
    {
       
        fill = StartCoroutine(SimpleLerp(timeEv));
    }

    IEnumerator SimpleLerp(float x)
    {
        float a = -1f;  // start
        float b = 0f;  // end
        for (float f = 0; f <= x; f += Time.deltaTime)
        {
            if (stop)
                break;

            filling.transform.localPosition = new Vector3(filling.transform.localPosition.x, Mathf.Lerp(a, b, f / x), filling.transform.localPosition.z);
            yield return null;
        }
        stop = false;
        ResetBar();
       
    }

    public void ResetBar()
    {
        filling.transform.localPosition = new Vector3(filling.transform.localPosition.x, -1f, filling.transform.localPosition.z);
    }

    public void Stopbar()
    {
        StopCoroutine(fill);
    }
}
