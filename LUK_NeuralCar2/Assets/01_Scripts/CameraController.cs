using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform target;
    public Vector3 decalLook;
    public Vector3 decalPos;

    public float posLerpSpeed = 0.02f;
    public float lookLerpSpeed = 0.1f;

    public Vector3 wantedPos;

    private void Awake()
    {
        Init();
    }

    // Start
    void Start()
    {
        
    }

    public virtual void Init()
    {
        instance = this;
    }

    // FixedUpdate
    void FixedUpdate()
    {
        if(target != null)
        {
            wantedPos = target.TransformPoint(decalPos);
            wantedPos.y = decalPos.y;

            transform.position = Vector3.Lerp(transform.position, wantedPos, posLerpSpeed);

            Quaternion wantedLook = Quaternion.LookRotation(target.TransformPoint(decalLook) - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, wantedLook, lookLerpSpeed);
        }
    }
}
