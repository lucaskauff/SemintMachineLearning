using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{

    public int[] layers;  //x = layer  {inputs, hidden layers, outputs}
    public float[][] neurons; //x = layer, y = neuron
    public float[][][] axons; //x = layer, y = neuron, z = axon

    MutationBehaviour mutationBehaviour;

    int x;
    int y;
    int z;

    public NeuralNetwork()
    {

    }

    public NeuralNetwork(int[] _layers)
    {
        layers = new int[_layers.Length];

        for (x = 0; x < _layers.Length; x++)
        {
            layers[x] = _layers[x];
        }

        InitNeurons();
        InitAxons();
    }

    void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();

        for (x = 0; x < layers.Length; x++)
        {
            neuronsList.Add(new float[layers[x]]);
        }

        neurons = neuronsList.ToArray();
    }

    void InitAxons()
    {

        //Liste à 2 dimensions
        List<float[][]> axonsList = new List<float[][]>();

        //On parcourt tous les layers
        for (x = 1; x < layers.Length; x++)
        {
            List<float[]> axonsInNeuronInLayer = new List<float[]>(); 

            int neuronsInPreviousLayer = layers[x - 1];

            //On parcourt tous les neurones de chaque layer
            for (y = 0; y < layers[x]; y++)
            {
                float[] axonsInNeuron = new float[neuronsInPreviousLayer];

                //On parcourt tous les axons de chaque neurone
                for (z = 0; z < neuronsInPreviousLayer; z++)
                {
                    axonsInNeuron[z] = UnityEngine.Random.Range(-1f, 1f);
                }

                axonsInNeuronInLayer.Add(axonsInNeuron);
            }

            axonsList.Add(axonsInNeuronInLayer.ToArray());
        }

        axons = axonsList.ToArray();
    }

    public void CopyNet(NeuralNetwork netToCopy)
    {
        for (int x = 0; x < netToCopy.axons.Length; x++)
        {
            for (int y = 0; y < netToCopy.axons[x].Length; y++)
            {
                for (int z = 0; z < netToCopy.axons[x][y].Length; z++)
                {
                    axons[x][y][z] = netToCopy.axons[x][y][z];
                }
            }
        }
    }


    float value;
    public void FeedForward(float[] inputs)
    {
        neurons[0] = inputs; //Initialisation des valeurs du premier layer de neurones.

        for (x = 1; x < layers.Length; x++)
        {
            for (y = 0; y < layers[x]; y++)
            {
                value = 0;

                for (z = 0; z < layers[x-1]; z++)
                {
                    value += neurons[x - 1][z] * axons[x - 1][y][z];
                }

                neurons[x][y] = (float)Math.Tanh(value);

            }
        }
    }

    float randomNumber;
    public void Mutate(float probability, MutationBehaviour mutation = MutationBehaviour.UseProbability)
    {
        if(probability >= 0f && probability <= 100f) // A changer en fonction du randomNumber --> Si on met 1f ou 100f
        {
            for (x = 1; x < layers.Length; x++)
            {
                for (y = 0; y < layers[x]; y++)
                {
                    for (z = 0; z < layers[x - 1]; z++)
                    {
                        value = axons[x - 1][y][z];

                        /*switch(mutation)
                        {
                            case MutationBehaviour.UseProbability:
                                continue;
                            case MutationBehaviour.Inversed:
                                break;
                        }

                        switch(probability)
                        {
                            case float n when n < 0.06f:
                                break;
                        }*/

                        randomNumber = UnityEngine.Random.Range(0f, 100f);

                        if (randomNumber < 0.06f * probability)
                        {
                            value = UnityEngine.Random.Range(-1f, 1f);
                        }
                        else if (randomNumber < 0.07f * probability)
                        {
                            value *= -1f;
                        }
                        else if (randomNumber < 0.5f * probability)
                        {
                            value += 0.1f * UnityEngine.Random.Range(-1f, 1f);
                        }
                        else if (randomNumber < 0.75f * probability)
                        {
                            value *= 1 + UnityEngine.Random.Range(0f, 1f);
                        }
                        else if (randomNumber < 1f * probability)
                        {
                            value *= UnityEngine.Random.Range(0f, 1f);
                        }

                        /*if (UnityEngine.Random.Range(0f, 1f) <= probability)
                        {
                            value = value*UnityEngine.Random.Range(-1f, 1f);
                        }
                        else
                        {
                            value *= -1f;
                        }*/

                        axons[x - 1][y][z] = value;
                    }
                }
            }
        }
    }

}
public enum MutationBehaviour {UseProbability, Random, Inversed, SmallUp, SmallDown}
