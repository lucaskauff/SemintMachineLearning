using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    public int legsNb;
    public int strength = 10;

    void Attack()
    {
        Debug.Log("Attacking!");
    }
}