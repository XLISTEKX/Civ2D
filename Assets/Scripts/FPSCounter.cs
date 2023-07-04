using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] TMP_Text counter;

    float _timer;

    public void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            counter.text = fps + "FPS";
            _timer = Time.unscaledTime + 0.125f;
        }
    }

}
