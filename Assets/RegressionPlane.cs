using UnityEngine;

public class RegressionPlane : MonoBehaviour
{
    public float a = 1; // Coefficient for x
    public float b = 1; // Coefficient for z
    public float c = 0; // y-intercept

    void Start()
    {
        // Assuming the plane is initially aligned with the y-axis,
        // adjust its rotation to align with the regression coefficients
        Vector3 normal = new Vector3(-a, 1, -b).normalized; // A vector orthogonal to the plane
        transform.up = normal; // Align the plane's up direction with the calculated normal

        // Position the plane according to the y-intercept
        transform.position = new Vector3(0, c, 0); // This is a simplification
    }
}
