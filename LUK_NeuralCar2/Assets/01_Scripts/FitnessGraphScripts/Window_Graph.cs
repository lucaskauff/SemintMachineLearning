﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph : MonoBehaviour
{
    [Header("Objects to serialize")]
    [SerializeField] Manager manager = default;
    [SerializeField] Sprite circleSprite = default;
    [SerializeField] RectTransform graphContainer = default;
    [SerializeField] RectTransform ligneMediane = default;
    [SerializeField] Text ord = default;
    [SerializeField] Text ordMoyenne = default;

    [Header("Serializable variables")]
    [SerializeField] float circleSize = 2f;
    [SerializeField] float highestFitnessOnStart = 100f;
    [SerializeField] Color defaultClr = Color.white;
    [SerializeField] Color mutatedClr = Color.green;

    //Hidden public variables
    [HideInInspector] public List<float> valueList;

    //Private
    RectTransform[] dotList;
    Image[] circleList;

    Vector2 defaultPos = Vector2.zero;
    float graphWidth;
    float graphHeight;

    float highestFitness;
    float fitnessAddition;
    float averageFitness;

    private void Start()
    {
        graphWidth = graphContainer.sizeDelta.x;
        graphHeight = graphContainer.sizeDelta.y;

        highestFitness = highestFitnessOnStart;

        dotList = new RectTransform[manager.populationSize];
        circleList = new Image[dotList.Length];

        CreateCircles(defaultPos);
    }

    void CreateCircles(Vector2 startPos)
    {
        for (int i = 0; i < manager.populationSize; i++)
        {
            GameObject gO = new GameObject("Circle" + i, typeof(Image));
            RectTransform rectT = gO.GetComponent<RectTransform>();
            Image theImage = gO.GetComponent<Image>();

            gO.transform.SetParent(graphContainer, false);
            theImage.sprite = circleSprite;

            rectT.anchoredPosition = startPos;
            rectT.sizeDelta = new Vector2(circleSize, circleSize);
            rectT.anchorMin = new Vector2(0, 0);
            rectT.anchorMax = new Vector2(0, 0);

            circleList[i] = theImage;
            dotList[i] = rectT;
            valueList.Add(0);
        }
    }

    private void Update()
    {
        UpdateCircles();
        UpdateOrd();
    } 

    void UpdateCircles()
    {
        fitnessAddition = 0;

        for (int i = 0; i < dotList.Length; i++)
        {
            valueList[i] = manager.agents[i].fitness;

            fitnessAddition += valueList[i];

            if (valueList[i] > highestFitness)
            {
                highestFitness = valueList[i];
            }

            if (manager.agents[i].isMutated)
            {
                circleList[i].color = mutatedClr;
            }
            else
            {
                circleList[i].color = defaultClr;
            }

            float xPosition = i * (graphWidth / dotList.Length);
            float yPosition = (valueList[i] / highestFitness) * graphHeight;
            dotList[i].anchoredPosition = new Vector2(xPosition, yPosition);
        }

        /*
        ligneMediane.anchoredPosition = new Vector2(ligneMediane.anchoredPosition.x,
            dotList[(int)dotList.Length / 2].anchoredPosition.y);
        */
        manager.agents.Sort();
    }

    void UpdateOrd()
    {
        if (manager.restarted)
        {
            highestFitness = highestFitnessOnStart;
            averageFitness = 0;
            manager.restarted = false;
        }
        else
        {
            averageFitness = fitnessAddition / valueList.Count;
        }

        ord.text = highestFitness.ToString("F0");
        ordMoyenne.text = averageFitness.ToString("F0");
    }
}