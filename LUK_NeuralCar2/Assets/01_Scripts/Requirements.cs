using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Requirements : MonoBehaviour, IComparable<Requirements>
{
    public List<int> miskina = new List<int>();

    public int[][][] int3D = new int[10][][];

    public float fitness;

    public int CompareTo(Requirements require)
    {
        if(fitness < require.fitness)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public static Requirements instance;

    

    public void Awake()
    {
        instance = this;
    }

    // Start
    void Start()
    {

    }

    // Update
    void Update()
    {
        
    }

    void TestList()
    {
        for (int x = 0; x < int3D.Length; x++)
        {
            int3D[x] = new int[x + 1][];

            for (int y = 0; y < int3D[x].Length; y++)
            {
                int3D[x][y] = new int[y + 1];

            }
        }
    }
}

