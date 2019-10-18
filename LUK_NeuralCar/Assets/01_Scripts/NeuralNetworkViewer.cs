using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuralNetworkViewer : MonoBehaviour
{
    public static NeuralNetworkViewer instance;

    public Gradient colorGradient;

    [SerializeField] float decalX = 100;
    [SerializeField] float decalY = 20;

    public Transform viewerGroup;

    [SerializeField] RectTransform neuronPrefab;
    public RectTransform neuronInstance;
    Image[][] neuronsImage;
    TextMesh[][] neuronsValue;

    [SerializeField] RectTransform axonPrefab;
    public RectTransform axonInstance;
    Image[][][] axons;

    public TextMesh fitness;

    int x;
    int y;
    int z;

    Agent agent;

    private void Awake()
    {
        instance = this;
    }

    public void Init(Agent _agent)
    {
        agent = _agent;

        CreateViewer(agent.net);
    }

    public void CreateViewer(NeuralNetwork net)
    {
        for (int i = viewerGroup.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(viewerGroup.GetChild(i).gameObject);
        }

        InitNeurons(net);
        InitAxons(net);
    }

    public void InitNeurons(NeuralNetwork net)
    {
        for (x = 0; x < net.neurons.Length; x++)
        {
            neuronsImage[x] = new Image[net.neurons[x].Length];
            neuronsValue[x] = new TextMesh[net.neurons[x].Length];

            for (y = 0; y < net.neurons[x].Length; y++)
            {
                neuronInstance = Instantiate(neuronPrefab, Vector3.zero, Quaternion.identity, viewerGroup);
                neuronInstance.anchoredPosition = new Vector2(x*decalX, y*decalY);

                neuronsImage[x][y] = neuronInstance.GetComponent<Image>();
                neuronsValue[x][y] = neuronInstance.GetChild(0).GetComponent<TextMesh>();
            }
        }
    }

    float angle;
    float posX;
    float posY;
    public void InitAxons(NeuralNetwork net)
    {
        axons = new Image[net.axons.Length][][];

        for (x = 0; x < net.axons.Length; x++)
        {
            axons[x] = new Image[net.axons[x].Length][];

            for (int y = 0; y < net.axons[x].Length; y++)
            {
                axons[x][y] = new Image[net.axons[x][y].Length];

                for (int z = 0; z < net.axons[x][y].Length; z++)
                {
                    posX = x * decalX;
                    posY = y * decalY;

                    angle = Mathf.Atan2(posX, posY) * Mathf.Rad2Deg;

                    axonInstance = Instantiate(axonPrefab, Vector3.zero, Quaternion.identity, viewerGroup);
                    axonInstance.anchoredPosition = new Vector2(posX, posY);
                    axonInstance.eulerAngles = new Vector3(0, 0, angle);
                    axonInstance.sizeDelta = new Vector2(new Vector2(posX, posY).magnitude, 2);
                }
            }
        }
    }
}