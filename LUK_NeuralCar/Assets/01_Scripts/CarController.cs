using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSteerAngle = 42;
    public float motorForce = 800;

    public WheelCollider frontDriverW;
    public WheelCollider backDriverW;
    public WheelCollider frontPassengerW;
    public WheelCollider backPassengerW;

    public Transform frontDriverT;
    public Transform backDriverT;
    public Transform frontPassengerT;
    public Transform backPassengerT;

    public float horizontalInput;
    public float verticalInput;
    public float steeringAngle;

    private void FixedUpdate()
    {
        Steer();
        Accelerate();
        UpdateWheelPos();
    }

    void Steer()
    {
        steeringAngle = horizontalInput * maxSteerAngle;
        frontDriverW.steerAngle = steeringAngle;
        frontPassengerW.steerAngle = steeringAngle;
    }

    void Accelerate()
    {
        backDriverW.motorTorque = verticalInput * motorForce;
        backPassengerW.motorTorque = verticalInput * motorForce;
    }

    void UpdateWheelPos()
    {
        UpdateThisWheelPos(frontDriverW, frontDriverT);
        UpdateThisWheelPos(backDriverW, backDriverT);
        UpdateThisWheelPos(frontPassengerW, frontPassengerT);
        UpdateThisWheelPos(backPassengerW, backPassengerT);
    }

    Vector3 pos;
    Quaternion quat;

    void UpdateThisWheelPos(WheelCollider col, Transform tr)
    {
        pos = tr.position;
        quat = tr.rotation;

        col.GetWorldPose(out pos, out quat);

        tr.position = pos;
        tr.rotation = quat;
    }
}