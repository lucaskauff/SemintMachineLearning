using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Variables

    //Custom editor variables
    [ContextMenuItem("Randomize population size", "RandomPop"),
        ContextMenuItem("Add 50 cars", "AddPop"),
        ContextMenuItem("Remove 50 cars", "RemPop"),
        Tooltip("Right click to apply a function or edit manually !")]
    public int populationSize = 200;

    [ContextMenuItem("Randomize training duration", "RandomDur"),
        ContextMenuItem("Add 30s of training", "AddDur"),
        ContextMenuItem("Remove 30s of training", "RemDur"),
        Tooltip("Right click to apply a function or edit manually !")]
    public int trainingDuration = 300;

    [ContextMenuItem("Randomize mutation rate", "RandomMut"),
        ContextMenuItem("Add 10% mutation probability", "AddMut"),
        ContextMenuItem("Remove 10% mutation probability", "RemMut"),
        Tooltip("Right click to apply a function or edit manually !")]
    public int mutationRate = 100;

    //Regular variables
    public int generationNb = 0;
    public float timeLeftForThisLoop = 30f;

    public GameObject agentPrefab;
    public Transform agentGroup;

    public List<Agent> agents = new List<Agent>();
    Agent agent;

    public bool autoSave;
    public bool restarted = false;

    public int[] layers;

    public float timeSpentOnScene = 0;
    #endregion

    #region Unity Methods
    void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    private void Update()
    {
        timeLeftForThisLoop = trainingDuration - (Time.time - timeSpentOnScene);
    }
    #endregion

    #region Coroutines
    IEnumerator InitCoroutine()
    {
        NewGeneration();
        InitNeuralNetworkViewer();
        Load1();
        Focus();

        timeSpentOnScene = Time.time;

        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        NewGeneration();
        Focus();
        if (autoSave)
        {
            Save();
        }

        timeSpentOnScene = Time.time;

        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(Loop());
    }
    #endregion

    #region Custom Methods
    public void NewGeneration()
    {
        AddOrRemoveAgent();
        agents.Sort();
        Mutate();
        SetColorToCar();
        ResetAgents();

        generationNb += 1;
    }
    
    void AddOrRemoveAgent()
    {
        if (agents.Count != populationSize)
        {           
            int diff = populationSize - agents.Count;

            if(diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {                   
                    AddAgent();
                }
            }
            else
            {
                for (int i = 0; i < -diff; i++)
                {
                    RemoveAgent();
                }
            }
        }
    }

    void AddAgent()
    {
        agent = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentGroup).GetComponent<Agent>();
        agent.net = new NeuralNetwork(layers);

        agents.Add(agent);
    }

    void RemoveAgent()
    {
        Destroy(agents[agents.Count - 1].gameObject);
        agents.RemoveAt(agents.Count - 1);
    }

    void Mutate()
    {
        for (int i = agents.Count/2; i < agents.Count; i++)
        {
            agents[i].net.CopyNet(agents[i-(agents.Count/2)].net);
            agents[i].net.Mutate(mutationRate);
        }
    }

    void ResetAgents()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].ResetAgent();
        }
    }

    void SetColorToCar()
    {
        agents[0].SetColor(CarColor.First);

        for (int i = 1; i < populationSize/2; i++)
        {
            agents[i].SetColor(CarColor.Default);
        }

        for (int i = populationSize/2; i < populationSize; i++)
        {
            agents[i].SetColor(CarColor.Mutated);
        }
    }

    void Focus()
    {
        NeuralNetworkViewer.instance.agent = agents[0];
        NeuralNetworkViewer.instance.RefreshAxon();
        CameraController.instance.target = agents[0].transform;
    }

    public void ReFocus()
    {
        agents.Sort();
        Focus();
    }

    public void ResetNets()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].net = new NeuralNetwork(agent.net.layers);
        }

        Restart();
    }

    public void InitNeuralNetworkViewer()
    {
        NeuralNetworkViewer.instance.Init(agents[0]);
    }

    public void Load1()
    {
        Data data = DataManager.instance.Load();
        
        if (data != null)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].net = data.nets[i];
            }
        }
    }

    public void Load2()
    {
        Load1();
        Restart();
    }

    //Booléen pour un autoreset au bout de 10 secondes si la first ne bouge pas par exemple.
    public void Restart()
    {
        StopAllCoroutines();
        StartCoroutine(Loop());

        restarted = true;
        generationNb = 1;
    }

    [ContextMenu("Save")]
    public void Save()
    {
        Debug.Log("Saved");
        List<NeuralNetwork> neuralList = new List<NeuralNetwork>();

        for (int i = 0; i < agents.Count; i++)
        {
            neuralList.Add(agents[i].net);
        }

        DataManager.instance.Save(neuralList);
    }
    #endregion

    #region Editor Methods
    //Global modifications
    public void RandomizeAll()
    {
        RandomPop();
        RandomDur();
        RandomMut();
    }

    //Population methods
    void RandomPop()
    {
        populationSize = UnityEngine.Random.Range(0, 200);
    }

    void AddPop()
    {
        if (populationSize + 50 > 200)
            populationSize = 200;
        else
            populationSize += 50;
    }

    void RemPop()
    {
        if (populationSize - 50 < 0)
            populationSize = 0;
        else
            populationSize -= 50;
    }

    //Duration methods
    void RandomDur()
    {
        trainingDuration = UnityEngine.Random.Range(0, 300);
    }

    void AddDur()
    {
        if (trainingDuration + 30 > 300)
            trainingDuration = 300;
        else
            trainingDuration += 30;
    }

    void RemDur()
    {
        if (trainingDuration - 30 < 0)
            trainingDuration = 0;
        else
            trainingDuration -= 30;
    }

    //Mutation methods
    void RandomMut()
    {
        mutationRate = UnityEngine.Random.Range(0, 100);
    }

    void AddMut()
    {
        if (mutationRate + 10 > 100)
            mutationRate = 100;
        else
            mutationRate += 10;
    }

    void RemMut()
    {
        if (mutationRate - 10 < 0)
            mutationRate = 0;
        else
            mutationRate -= 10;
    }
    #endregion
}