using UnityEngine;
using UnityEngine.UI;

public class Fps : MonoBehaviour
{
    public TextMesh myTxt = default;

    const float updateInterval = .5f;
    float accum;
    float frames;
    float timeLeft;
    float fpsCount;

    private void Start()
    {
        timeLeft = updateInterval;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeLeft <= 0)
        {
            fpsCount = accum / frames;
            myTxt.text = fpsCount.ToString("f0");

            if (fpsCount < 30)
            {
                myTxt.color = Color.red;
            }
            else if (fpsCount < 60)
            {
                myTxt.color = Color.yellow;
            }
            else
            {
                myTxt.color = Color.green;
            }

            timeLeft = updateInterval;
            accum = 0;
            frames = 0;
        }
    }
}