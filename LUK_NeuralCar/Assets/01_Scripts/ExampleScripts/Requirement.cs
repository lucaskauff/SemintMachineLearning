using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Requirement : MonoBehaviour, IComparable<Requirement>
{
    public float fitness;

    public static Requirement instance;

    public List<int> listOfInt;
    public int[] arrayOfInt;
    public int[,] array2DOfInt;

    public int[][] jaggedArrayOfInt;
    public int[][][] jaggedArray3D;

    public int CompareTo(Requirement other)
    {
        if (fitness < other.fitness)
        {
            return 1;
        }
        else if (fitness > other.fitness)
        {
            return -1;
        }

        return 0;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TestList();
        TestArray();
        TestJaggedArray();
    }

    void TestList()
    {
        listOfInt = new List<int>();

        int myInt = 1;

        listOfInt.Add(0);
        listOfInt.Add(myInt);

        Debug.Log(listOfInt[0]);

        listOfInt.RemoveAt(1);

        listOfInt.Sort();

        for (int i = 0; i < listOfInt.Count; i++)
        {
            Debug.Log(listOfInt[i]);
        }
    }

    void TestArray()
    {
        arrayOfInt = new int[10];

        arrayOfInt[0] = 123;

        Debug.Log(arrayOfInt[0]);


        array2DOfInt = new int[10, 5];
        array2DOfInt[5, 3] = 123;
    }

    void TestJaggedArray()
    {
        jaggedArrayOfInt = new int[10][];

        for (int i = 0; i < jaggedArrayOfInt.Length; i++)
        {
            jaggedArrayOfInt[i] = new int[i + 1];
        }

        for (int x = 0; x < jaggedArrayOfInt.Length; x++)
        {
            for (int y = 0; y < jaggedArrayOfInt[x].Length; y++)
            {
                jaggedArrayOfInt[x][y] = 123;
            }
        }


        jaggedArray3D = new int[10][][];

        for (int i = 0; i < jaggedArray3D.Length; i++)
        {
            jaggedArray3D[i] = new int[i + 1][];
        }

        for (int x = 0; x < jaggedArray3D.Length; x++)
        {
            for (int y = 0; y < jaggedArray3D[x].Length; y++)
            {
                for (int z = 0; z < jaggedArray3D[y].Length; z++)
                {
                    jaggedArray3D[x][y][z] = 123;
                }
            }
        }
    }

    private void Update()
    {

    }
}