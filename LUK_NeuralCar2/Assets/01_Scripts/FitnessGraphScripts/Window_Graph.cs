using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph : MonoBehaviour
{
    [Header("Objects to serialize")]
    [SerializeField] Manager manager = default;
    [SerializeField] Sprite circleSprite = default;
    [SerializeField] RectTransform graphContainer = default;

    [Header("Serializable variables")]
    [SerializeField] Vector2 defaultPos = Vector2.zero;
    [SerializeField] Vector2 circleSize = default;
    [SerializeField] float highestFitness = 100f;

    //Hidden public variables
    [HideInInspector] public List<float> valueList;

    //Private
    RectTransform[] dotList;

    float graphWidth;
    float graphHeight;

    private void Start()
    {
        graphWidth = graphContainer.sizeDelta.x;
        graphHeight = graphContainer.sizeDelta.y;

        dotList = new RectTransform[manager.populationSize];

        CreateCircles(defaultPos);
    }

    private void Update()
    {
        UpdateCircles();
    } 

    void CreateCircles(Vector2 startPos)
    {
        for (int i = 0; i < manager.populationSize; i++)
        {
            GameObject gO = new GameObject("Circle"+i, typeof(Image));
            gO.transform.SetParent(graphContainer, false);
            gO.GetComponent<Image>().sprite = circleSprite;

            RectTransform rectT = gO.GetComponent<RectTransform>();
            rectT.anchoredPosition = startPos;
            rectT.sizeDelta = circleSize;
            rectT.anchorMin = new Vector2(0, 0);
            rectT.anchorMax = new Vector2(0, 0);

            dotList[i] = rectT;
            valueList.Add(0);
        }
    }

    void UpdateCircles()
    {
        for (int i = 0; i < dotList.Length; i++)
        {
            valueList[i] = manager.agents[i].fitness;

            if (valueList[i] > highestFitness)
            {
                highestFitness = valueList[i];
            }

            valueList.Sort();

            float xPosition = i * (graphWidth / dotList.Length);
            float yPosition = (valueList[i] / highestFitness) * graphHeight;
            dotList[i].anchoredPosition = new Vector2(xPosition, yPosition);
        }
    }
}