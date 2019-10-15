using UnityEngine;
using System;

public class Agent : MonoBehaviour
{
    public NeuralNetwork net;
    public CarController carController;
    public float fitness;
    public float rayRange = 1;

    public LayerMask layerMask;

    float[] inputs;

    public Transform nextCheckpoint;
    public float nextCheckpointDist;
    public float distanceTraveled = 0;
    public float lastDistCheckpoint;

    public Rigidbody rb;

    public void Start()
    {
        Init();
    }

    public void ResetAgent()
    {
        fitness = 0;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        distanceTraveled = 0;

        Init();
    }

    void Init()
    {
        inputs = new float[net.layers[0]];

        nextCheckpoint = CheckpointManager.instance.firstCheckpoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
    }

    public void CheckpointReached(Transform _nextCheckpoint)
    {
        distanceTraveled += nextCheckpointDist;
        nextCheckpoint = _nextCheckpoint;
    }

    private void FixedUpdate()
    {
        InputUpdate();
        OutputUpdate();
        FitnessUpdate();
        lastDistCheckpoint = DistCheckpoint();
    }

    float DistCheckpoint()
    {
        return ClosestDistToPointOnLine(nextCheckpoint.position, nextCheckpoint.position + nextCheckpoint.forward, transform.position);
    }

    float distCheckpoint;
    float angleZ;
    Vector3 pos;
    void InputUpdate()
    {
        pos = transform.position;

        inputs[0] = RaySensor(pos + Vector3.up * 0.2f, transform.forward, 4f);
        inputs[1] = RaySensor(pos + Vector3.up * 0.2f, transform.right, 1.5f);
        inputs[2] = RaySensor(pos + Vector3.up * 0.2f, -transform.right, 1.5f);
        inputs[3] = RaySensor(pos + Vector3.up * 0.2f, transform.right + transform.forward, 2f);
        inputs[4] = RaySensor(pos + Vector3.up * 0.2f, -transform.right + transform.forward, 2f);

        inputs[5] = 1 - (float)Math.Tanh(rb.velocity.magnitude / 20);

        distCheckpoint = DistCheckpoint();
        inputs[6] = (float)Math.Tanh((lastDistCheckpoint - distCheckpoint)*5);

        inputs[7] = (float)Math.Tanh(rb.angularVelocity.z * 0.1f);

        inputs[8] = (float)Math.Tanh(rb.angularVelocity.y * 0.1f);

        inputs[9] = (float)Math.Tanh(transform.InverseTransformDirection(ClosestPointOnLine(nextCheckpoint.position, nextCheckpoint.position + nextCheckpoint.forward, transform.position)).x);

        inputs[10] = (float)Math.Tanh(transform.InverseTransformDirection(ClosestPointOnLine(nextCheckpoint.position, nextCheckpoint.position + nextCheckpoint.forward, transform.position)).z);

        angleZ = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;

        inputs[11] = (float)Math.Tanh(angleZ / 90);
    }

    public void OutputUpdate()
    {
        net.FeedForward(inputs);

        carController.horizontalInput = net.neurons[net.layers.Length - 1][0];
        carController.verticalInput = net.neurons[net.layers.Length - 1][1];
    }

    public void FitnessUpdate()
    {
        SetFitness(distanceTraveled + (nextCheckpointDist -(transform.position - nextCheckpoint.position).magnitude));
    }

    void SetFitness(float _newFitness)
    {
        if (fitness < -_newFitness)
        {
            fitness = _newFitness;
        }
    }

    RaycastHit hit;
    float RaySensor(Vector3 pos, Vector3 dir, float vectorLength)
    {
        if(Physics.Raycast(pos, dir, out hit, rayRange * vectorLength, layerMask))
        {
            Debug.DrawRay(pos, dir * hit.distance, Color.green);
            return (rayRange * vectorLength - hit.distance) / (rayRange * vectorLength);
        }

        Debug.DrawRay(pos, dir * vectorLength * rayRange, Color.red);
        return 0;
    }

    float ClosestDistToPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        return (vA + (vB - vA).normalized * Vector3.Dot((vB - vA).normalized, vPoint - vA) - vPoint).magnitude;
    }

    Vector3 v;
    float d;
    float t;
    Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        v = (vB - vA).normalized;
        d = Vector3.Distance(vA, vB);
        t = Vector3.Dot(v, vPoint - vA);

        if (t <= 0)
            return vA;

        if (t >= d)
            return vB;

        return vA + v * t;
    }
}