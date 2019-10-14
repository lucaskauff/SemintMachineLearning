using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Transform nextCheckpoint;

    public void CheckpointReached(Transform _nextCheckpoint)
    {
        nextCheckpoint = _nextCheckpoint;
    }
}