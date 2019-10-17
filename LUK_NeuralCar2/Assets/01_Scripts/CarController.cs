using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Range(0,180)]
    public float maxSteerAngle = 42;

    [Range(0,10000)]
    public int motorForce = 800;

    public WheelCollider frontLeftW;
    public WheelCollider frontRightW;
    public WheelCollider backLeftW;
    public WheelCollider backRightW;

    public Transform frontLeftT;
    public Transform frontRightT;
    public Transform backLeftT;
    public Transform backRightT;

    public float horizontalInput;
    public float verticalInput;

    public float steeringAngle;

    // Start
    void Start()
    {
        
    }

    // Update
    void Update()
    {
        
    }
    
    // FixedUpdate
    void FixedUpdate()
    {
        Steer();
        Accelerate();
    }

    void Steer()
    {
        steeringAngle = horizontalInput * maxSteerAngle;
        frontLeftW.steerAngle = steeringAngle;
        frontRightW.steerAngle = steeringAngle;
    }

    void Accelerate()
    {
        backRightW.motorTorque = verticalInput * motorForce;
        backLeftW.motorTorque = verticalInput * motorForce;
    }

    void UpdateWheelPos()
    {
        UpdateThisWheelPos(frontLeftW, frontLeftT);
        UpdateThisWheelPos(frontRightW, frontRightT);
        UpdateThisWheelPos(backLeftW, backLeftT);
        UpdateThisWheelPos(backRightW, backRightT);
    }

    Vector3 pos;
    Quaternion quat;

    void UpdateThisWheelPos(WheelCollider wheel, Transform tr)
    {
        pos = tr.position;
        quat = tr.rotation;

        wheel.GetWorldPose(out pos, out quat);

        tr.position = pos;
        tr.rotation = quat;
    }
}
