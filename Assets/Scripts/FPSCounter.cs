using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] TMP_Text counter;

    float _timer;
    int lastFrameIndex;
    float[] frameDeltaTimeArray;

    private void Awake()
    {
        frameDeltaTimeArray = new float[50];
    }

    public void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        counter.text = Mathf.RoundToInt(CalculateFPS()) + "FPS";
    }

    float CalculateFPS()
    {
        float total = 0f;
        foreach(float delta in frameDeltaTimeArray)
        {
            total += delta;
        }
        return frameDeltaTimeArray.Length / total;
    }
}
