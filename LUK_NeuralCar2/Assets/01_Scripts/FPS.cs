using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    const float updateInterval = 0.5f;
    float accum;
    float frames;
    float timeLeft;
    float fpsCount;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = updateInterval;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale/Time.deltaTime;
        frames++;

        if (timeLeft <= 0)
        {
            fpsCount = accum / frames;
            text.text = fpsCount.ToString("f2");

            switch (fpsCount)
            {
                case float i when i < 25f:
                    text.color = Color.red;
                    break;
                case float i when i < 60f:
                    text.color = Color.yellow;
                    break;
                case float i when i >= 60f:
                    text.color = Color.green;
                    break;
            }

            timeLeft = updateInterval;
            accum = 0;
            frames = 0;
        }

    }
}
