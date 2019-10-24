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
    [SerializeField] Text popSize = default;
    [SerializeField] Text trainingDur = default;
    [SerializeField] Text mutationRate = default;

    private void Update()
    {
        UpdateValues();
    }

    void UpdateValues()
    {
        timeLeft.text = "Time left: " + manager.timeLeftForThisLoop.ToString("F2");
        genNbText.text = "Generation: #" + manager.generationNb.ToString();
        popSize.text = "Population: " + manager.populationSize.ToString();
        trainingDur.text = "Duration: " + manager.trainingDuration.ToString();
        mutationRate.text = "Mutation: " + manager.mutationRate.ToString() + "%";
    }
}