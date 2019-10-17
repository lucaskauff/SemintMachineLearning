using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour, IComparable<Agent>
{

    public Transform nextCheckpoint;
    public float nextCheckpointDist;
    public float lastCheckpointDist;
    public float distanceTravelled = 0f;
    public float fitness;

    public NeuralNetwork net;
    public CarController carController;

    public Rigidbody rb;

    public float rayRange = 1f; //Taille générale des rayons
    public LayerMask layerMask;

    public float[] inputs;

    public Material firstMat;
    public Material mutatedMat;
    public Material defaultMat;

    public Renderer render;
    public Renderer posRender;

    public CarColor carColor;

    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        InputUpdate();
        OutputUpdate();
        UpdateFitness();
        lastCheckpointDist = distCheckpoint;
    }

    float DistCheckpoint()
    {
        return ClosestDistToAPointOnALine(nextCheckpoint.position, nextCheckpoint.position + nextCheckpoint.forward, transform.position);
    }

    public void CheckpointReached(Transform _nextCheckpoint)
    {
        distanceTravelled += nextCheckpointDist;
        nextCheckpoint = _nextCheckpoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
        lastCheckpointDist = DistCheckpoint();
    }

    public void Init()
    {
        inputs = new float[net.layers[0]];        

        nextCheckpoint = CheckpointManager.instance.firstCheckpoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
    }

    public void ResetAgent()
    {
        fitness = 0f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        distanceTravelled = 0f;

        Init();
    }

    float distCheckpoint;
    float angleZ;
    Vector3 pos;
    void InputUpdate()
    {
        pos = transform.position;

        inputs[0] = RaySensor(pos + Vector3.up * 0.2f, transform.forward, 4f); //Devant
        inputs[1] = RaySensor(pos + Vector3.up * 0.2f, transform.right, 1.5f); //Droite
        inputs[2] = RaySensor(pos + Vector3.up * 0.2f, -transform.right, 1.5f); //Gauche
        inputs[3] = RaySensor(pos + Vector3.up * 0.2f, (transform.forward + transform.right), 2f); //Diago Droite
        inputs[4] = RaySensor(pos + Vector3.up * 0.2f, (transform.forward - transform.right), 2f); //Diago Gauche

        inputs[5] = 1 - (float)Math.Tanh(rb.velocity.magnitude / 20f); //Vitesse
        distCheckpoint = DistCheckpoint();
        inputs[6] = (float)Math.Tanh((lastCheckpointDist - distCheckpoint)*5);
        inputs[7] = (float)Math.Tanh(rb.angularVelocity.z * 0.1f); //Vitesse angulaire

        inputs[8] = (float)Math.Tanh(transform.InverseTransformDirection(
            ClosestPointOnALine(nextCheckpoint.position,
            nextCheckpoint.position + nextCheckpoint.forward,
            transform.position)).x);

        inputs[9] = (float)Math.Tanh(transform.InverseTransformDirection(
            ClosestPointOnALine(nextCheckpoint.position,
            nextCheckpoint.position + nextCheckpoint.forward,
            transform.position)).z);

        inputs[10] = (float)Math.Tanh(rb.angularVelocity.y * 0.1f);

        angleZ = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z; //Ternary operator
        inputs[11] = (float)Math.Tanh(angleZ / 90f);
    }

    void OutputUpdate()
    {
        net.FeedForward(inputs);

        carController.horizontalInput = net.neurons[net.layers.Length - 1][0];
        carController.verticalInput = net.neurons[net.layers.Length - 1][1];
    }

    void UpdateFitness()
    {
        SetFitness(distanceTravelled + (nextCheckpointDist - (transform.position - nextCheckpoint.position).magnitude));
    }

    void SetFitness(float newFitness)
    {
        if(fitness < newFitness)
        {
            fitness = newFitness;
        }
    }

    RaycastHit hit;
    float RaySensor(Vector3 pos, Vector3 direction, float length)
    {
        if (Physics.Raycast(pos, direction, out hit, rayRange * length, layerMask))
        {
            Debug.DrawRay(pos, direction * hit.distance, Color.green);

            return (rayRange*length - hit.distance)/(rayRange*length);
        }

        Debug.DrawRay(pos, direction * length * rayRange, Color.red);
        return 0;
    }

    float ClosestDistToAPointOnALine(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        return (vA + (vB - vA).normalized * Vector3.Dot((vB - vA).normalized, vPoint - vA) - vPoint).magnitude;
    }

    Vector3 v;
    float d;
    float t;
    Vector3 ClosestPointOnALine(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        v = (vB - vA).normalized;
        d = Vector3.Distance(vA, vB);
        t = Vector3.Dot(v, vPoint - vA);

        if(t <= 0)
        {
            return vA;
        }

        if(t >= d)
        {
            return vB;
        }

        return vA + v * t;
    }
    public int CompareTo(Agent other)
    {
        if (fitness < other.fitness)
        {
            return 1;
        }
        else if(fitness > other.fitness)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public void SetColor(CarColor carColor)
    {
        switch(carColor)
        {
            case CarColor.First:
                render.material = firstMat;
                posRender.material = firstMat;
                break;
            case CarColor.Default:
                render.material = defaultMat;
                posRender.material = defaultMat;
                break;
            case CarColor.Mutated:
                render.material = mutatedMat;
                posRender.material = mutatedMat;
                break;
        }
    }
}

public enum CarColor{First, Default, Mutated}
