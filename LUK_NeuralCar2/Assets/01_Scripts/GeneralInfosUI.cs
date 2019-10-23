using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralInfosUI : MonoBehaviour
{
    [Header("Objects to serialize")]
    [SerializeField] Manager manager = default;
    [SerializeField] Text timeLeft = default;
    [SerializeField] Text genNbText = default;
    //[SerializeField] 

    private void Update()
    {
        UpdateValues();

    }

    void UpdateValues()
    {
        timeLeft.text = "Time left: " + manager.trainingDuration.ToString("F2");
        genNbText.text = "Generations: " + manager.generationNb.ToString();
    }
}