using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class FrequencyCommunicationController : MonoBehaviour
{
    public GameObject buttonReciever;
    public List<FrequencyWheelController> frequencyWheels = new List<FrequencyWheelController>();
    public GameObject graph;

    private float val = 0f;
    private void Update()
    {
        val+=0.01f;
        graph.GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_ForegroundMask", new Vector2(val, -0.59f));
    }
    public void SetFrequenciesValues()
    {
        foreach(FrequencyWheelController wheel in frequencyWheels)
        {
            wheel.range = Random.Range(3, 300);// ELF Frequency range in submarines
            wheel.detect = true;
            Debug.Log(wheel.range);
        }
    }

    public string GetModifiedPhrase(string originalString)
    {
        string[] modPhrase = originalString.Split();
        int modWords = 0;
        switch (CheckWheelsInPosition())
        {
            case 0:
                modWords = Random.Range(3, modPhrase.Length);
                break;
            case 1:
                if (modPhrase.Length > 5)
                    modWords = Random.Range(3, modPhrase.Length);
                else
                    modWords = 3;

                break;
            case 2:
                if (modPhrase.Length > 5)
                    modWords = Random.Range(1, 3);
                else
                    modWords = 2;

                break;
            case 3:
                if (modPhrase.Length > 5)
                    modWords = Random.Range(1, 2);
                else
                    modWords = 1;
                break;
            case 4:
                break;
            default:
                Debug.LogError("MORE WHEELS THAN IT SHOULD EXISTS ARE IN POSITION!");
                break;
        }

        int lastIndexWord = 0;
        for(int i = 0; i < modWords; ++i)
        {
            int index = Random.Range(0, modPhrase.Length);
            string randomWord;
            if (lastIndexWord != index)
                randomWord = RandomString(modPhrase[index].Length);
            else
            {
                index = Random.Range(0, modPhrase.Length);
                randomWord = RandomString(modPhrase[index].Length);
            }
                
            lastIndexWord = index;
            modPhrase[index] = randomWord;
        }


        return string.Join(" ", modPhrase);
    }

    public int CheckWheelsInPosition()
    {
        int inRange = 0;
        foreach (FrequencyWheelController wheel in frequencyWheels)
        {
            if (wheel.isInRange)
                inRange++;
        }

        return inRange;
    }

    public static string RandomString(int length)
    {
        System.Random random = new System.Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray()).ToLower();
    }
}
