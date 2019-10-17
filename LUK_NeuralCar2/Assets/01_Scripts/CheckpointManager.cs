using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    public Transform firstCheckpoint;

    public List<Transform> checkpointsTransforms = new List<Transform>();
    public List<Checkpoint> checkpoints = new List<Checkpoint>();

    private void Awake()
    {
        instance = this;

        int i = 0;
        foreach(Checkpoint cp in checkpoints)
        {
            if(i >= checkpoints.Count-1)
            {
                cp.nextCheckpoint = checkpointsTransforms[0];
                return;
            }
            else
            {
                cp.nextCheckpoint = checkpointsTransforms[i + 1];
                i++;
            }
        }
    }
}
