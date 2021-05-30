using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBar : MonoBehaviour
{
    public GameObject filling;

    public void BeginFilling(float timeEv)
    {
        StartCoroutine(SimpleLerp(timeEv));
    }

    IEnumerator SimpleLerp(float x)
    {
        float a = -1f;  // start
        float b = 0f;  // end
        for (float f = 0; f <= x; f += Time.deltaTime)
        {
            filling.transform.localPosition = new Vector3(filling.transform.localPosition.x, Mathf.Lerp(a, b, f / x), filling.transform.localPosition.z);
            yield return null;
        }
    }

    public void ResetBar()
    {
        filling.transform.localPosition = new Vector3(filling.transform.localPosition.x, -1f, filling.transform.localPosition.z);
    }
}
