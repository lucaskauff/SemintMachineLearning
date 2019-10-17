using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] CarController carController = default;

    private void Update()
    {
        carController.horizontalInput = Input.GetAxis("Horizontal");
        carController.verticalInput = Input.GetAxis("Vertical");
    }
}