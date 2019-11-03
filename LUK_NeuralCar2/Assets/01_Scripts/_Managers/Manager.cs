using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Variables
    [ContextMenuItem("Randomize Population", "RandomPop")]
    public int populationSize = 50;
    public int trainingDuration = 30;
    public int mutationRate = 8;
    public int generationNb = 0;
    public float timeLeftForThisLoop = 30f;

    public GameObject agentPrefab;
    public Transform agentGroup;

    public List<Agent> agents = new List<Agent>();
    Agent agent;

    public bool autoSave;
    public bool restarted = false;

    public int[] layers;

    float timeSpentOnScene = 0;
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
    void RandomPop()
    {
        //populationSize = UnityEngine.Random.Range(0, 250);
    }
    #endregion
}