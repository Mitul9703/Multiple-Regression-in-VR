using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public float movementSpeed = 100f;
    public float turnSpeed = 60f;
    public float elevationSpeed = 30f;
    public float lookSpeed = 15f;

    void Update()
    {
        // Replace GetAxis with specific key checks for forward/backward movement
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f; // Forward
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f; // Backward
        }

        Vector3 forwardMovement = transform.forward * verticalInput;

        // Replace GetAxis with specific key checks for strafe movement
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f; // Left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f; // Right
        }

        Vector3 rightMovement = transform.right * horizontalInput;

        // Combine forward/backward and left/right movement
        Vector3 movement = (forwardMovement + rightMovement).normalized * movementSpeed;
        transform.Translate(movement * Time.deltaTime, Space.World);

        // Elevation control with Up and Down Arrow keys
        float elevationInput = 0f;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            elevationInput = 1f; // Up
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            elevationInput = -1f; // Down
        }

        Vector3 elevationMovement = Vector3.up * elevationInput * elevationSpeed;
        transform.Translate(elevationMovement * Time.deltaTime, Space.World);

        // Turning left and right with Left and Right Arrow keys
        float turnInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            turnInput = 1f; // Right turn
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            turnInput = -1f; // Left turn
        }

        transform.Rotate(Vector3.up, turnInput * turnSpeed * Time.deltaTime);

        // Look up and down with E and Q keys
        float lookInput = 0f;
        if (Input.GetKey(KeyCode.E))
        {
            lookInput = 1f; // Look up
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            lookInput = -1f; // Look down
        }

        // Apply the look up/down rotation
        transform.Rotate(Vector3.right, lookInput * lookSpeed * Time.deltaTime);
    }
}
