using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public Transform nextCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        Agent agent = other.GetComponent<Agent>();

        if(agent != null)
        {
            if(agent.nextCheckpoint == transform)
            {
                agent.CheckpointReached(nextCheckpoint);
            }
        }
    }
}
