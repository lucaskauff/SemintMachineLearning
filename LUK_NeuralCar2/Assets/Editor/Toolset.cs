using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Toolset : MonoBehaviour
{
    #region Menu Items
    /*
    [MenuItem("LUK_Menu/Print General Infos #g")]
    static void Print()
    {
        Manager manager = FindObjectOfType<Manager>();
        Window_Graph windowgraph = FindObjectOfType<Window_Graph>();
        Debug.Log("Generation: " + manager.generationNb.ToString() +
            " | Simulation time: " + manager.timeSpentOnScene.ToString("F2") +
            " | Average fitness: " + windowgraph.averageFitness.ToString());
    }
    */

    [MenuItem("CONTEXT/Manager/LUK_Apply default values")]
    static void DefaultValues(MenuCommand command)
    {
        Manager manager = (Manager)command.context;
        manager.populationSize = 50;
        manager.trainingDuration = 30;
        manager.mutationRate = 5;
    }
    #endregion
}