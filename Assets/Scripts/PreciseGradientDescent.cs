using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreciseGradientDescent : MonoBehaviour
{
    public DataManager dataManager; // Ensure this is populated with your data
    public float learningRate = 0.01f;
    public int iterations = 10000;
    private float a = 0; // Coefficient for x
    private float b = 0; // Coefficient for z
    private float c = 0; // y-intercept

    private List<DataPoint> dataPoints;

    // Make these public so they can be accessed by AdjustPlane
    public float A => a;
    public float B => b;
    public float C => c;

    private int convergenceCount = 0;
    private float previousCost = float.MaxValue;

    public TextMeshPro planeEquationText;
    private Coroutine descentCoroutine = null;

    private bool isSimulationRunning = false;

    public Vector3 initialPlanePosition = new Vector3(0, 0, 0); // Default position; adjust as needed
    public Quaternion initialPlaneRotation = Quaternion.identity; // Default rotation
    public Vector3 initialPlaneScale = new Vector3(0, 0,0);


    void Start()
    {
        // Initialize simulation setup but do not start the descent coroutine automatically.
        InitializeSimulation();
    }

    public void InitializeSimulation()
    {
        if (dataManager != null && dataManager.dataPoints != null)
        {
            dataPoints = dataManager.dataPoints;
            ResetSimulation(); // Reset simulation state before starting
                               // Do not automatically start the descent coroutine here.
            Debug.Log("Initialized simulation with coefficients: a={a}, b={b}, c={c}");
        }
        else
        {
            Debug.LogError("DataManager is not set or contains no data.");
        }
    }

    public void StartSimulation()
    {
        if (!isSimulationRunning)
        {

            if (regressionPlane != null)
            {
                regressionPlane.SetActive(true);
                Debug.Log("Plane set to active.");
            }
            ResetSimulation(); // Ensure simulation state is reset before starting.
            descentCoroutine = StartCoroutine(RunGradientDescent());
            isSimulationRunning = true;
            Debug.Log("Simulation started.");
        }
        else
        {
            Debug.Log("Simulation is already running.");
        }
    }

    public void TogglePauseResume()
    {
        if (!isSimulationRunning && descentCoroutine == null)
        {
            // If the simulation is not running and the coroutine is null, it's paused; resume it.
            descentCoroutine = StartCoroutine(RunGradientDescent());
            isSimulationRunning = true;
            Debug.Log("Simulation resumed.");
        }
        else if (isSimulationRunning && descentCoroutine != null)
        {
            // If the simulation is running, pause it.
            StopCoroutine(descentCoroutine);
            descentCoroutine = null;
            isSimulationRunning = false;
            Debug.Log("Simulation paused.");
        }
    }


    private void ResetSimulation()
    {
            // Reset functionality...
            if (descentCoroutine != null)
            {
                StopCoroutine(descentCoroutine);
                descentCoroutine = null;
            }
            isSimulationRunning = false;
            // Variable resets and UI updates...
            Debug.Log("Simulation reset.");
        // Reset regression coefficients
        a = 0f;
        b = 0f;
        c = 0f;

        // Reset simulation state variables
        previousCost = float.MaxValue;
        convergenceCount = 0;

        // Reset UI elements
        if (planeEquationText != null)
        {
            planeEquationText.text = "Plane Equation \n y = ax + bz + c";
            regressionPlane.transform.position = initialPlanePosition;
            regressionPlane.transform.rotation = initialPlaneRotation;
            regressionPlane.transform.localScale = initialPlaneScale;
            Debug.Log("Plane position, rotation, and scale reset.");

        }

        // Reset visualization elements
        // E.g., Reset the regressionPlane position, rotation, and scale if modified


        // Optionally, reset data points if they are modified during the simulation
        // This would require re-loading them or restoring their original values
    }


    void OnDisable()
    {
        if (descentCoroutine != null)
        {
            StopCoroutine(descentCoroutine);
        }
    }

    IEnumerator RunGradientDescent()
    {
        int m = dataPoints.Count;
        
        const float convergenceThreshold = 0.001f;  // Set this to a value that signifies convergence
         // Counter to check for consecutive non-significant changes

        for (int i = 0; i < iterations; i++)
        {
            float da = 0;
            float db = 0;
            float dc = 0;
            float cost = 0;

            foreach (var point in dataPoints)
            {
                float x = point.x;
                float z = point.z;
                float y = point.y;
                float prediction = a * x + b * z + c;
                float error = prediction - y;

                da += error * x;
                db += error * z;
                dc += error;
                cost += error * error;
            }

            da /= m;
            db /= m;
            dc /= m;
            cost /= (2 * m);

            // After calculating da, db, dc
            float gradientClipValue = 1.0f; // Example value, adjust as necessary
            da = Mathf.Clamp(da, -gradientClipValue, gradientClipValue);
            db = Mathf.Clamp(db, -gradientClipValue, gradientClipValue);
            dc = Mathf.Clamp(dc, -gradientClipValue, gradientClipValue);


            a -= learningRate * da;
            b -= learningRate * db;
            c -= learningRate * dc;

            if (planeEquationText != null)
            {
                planeEquationText.text = $"Plane Equation \n y = {a:F2}x + {b:F2}z + {c:F2}";
            }

            // Early stopping condition: Check if the absolute change in cost is less than the threshold
            if (Mathf.Abs(previousCost - cost) < convergenceThreshold)
            {
                convergenceCount++;
                if (convergenceCount >= 5) // Check for 5 consecutive non-significant changes
                {
                    Debug.Log("Convergence reached, stopping iterations.");
                    break; // Break out of the loop if the change is smaller than the threshold for 5 times
                }
            }
            else
            {
                convergenceCount = 0; // Reset if a significant change is found
            }
            previousCost = cost; // Update the previous cost
            if (i < 100)
            {
                yield return new WaitForSeconds(0.2f);
            }
            else if (i % 10 == 0 || i == iterations - 1)
            {
                Debug.Log($"Iteration {i}, Cost: {cost}, PrevCost : {previousCost}, a={a}, b={b}, c={c}");
                // Adjust the plane with the current coefficients

                // Optionally, wait for a short time to visually see the plane adjusting
                yield return new WaitForSeconds(0.01f);
            }
            AdjustPlane();
            
            
        }
        AdjustPlane(); // One final adjustment after the loop
    }


    public GameObject regressionPlane; // Assign in Inspector


    void AdjustPlane()
    {
        if (regressionPlane != null && dataPoints != null && dataPoints.Count > 0)
        {
            // Calculate the centroid of the data points
            Vector3 sum = Vector3.zero;
            foreach (var point in dataPoints)
            {
                
                sum += new Vector3(point.x, point.y, point.z);
            }
            Vector3 centroid = sum / dataPoints.Count;

            // Find the furthest distance from the centroid in x and z
            float maxDistanceX = 0f;
            float maxDistanceZ = 0f;
            foreach (var point in dataPoints)
            {
                float distanceX = Mathf.Abs(point.x - centroid.x);
                float distanceZ = Mathf.Abs(point.z - centroid.z);
                if (distanceX > maxDistanceX) maxDistanceX = distanceX;
                if (distanceZ > maxDistanceZ) maxDistanceZ = distanceZ;
            }
            float scaleFactor = .3f;
            // Assuming the plane's local up vector should align with the calculated normal
            Vector3 normal = new Vector3(a, -1, b).normalized;

            // Rotation: Calculate rotation to align plane's up vector with the normal
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
            regressionPlane.transform.rotation = rotation;

            // Position: Adjust so the plane's center is at the centroid of the data points
            regressionPlane.transform.position = new Vector3(centroid.x, centroid.y + c, centroid.z); // Offset by c along y-axis
            
            // Scale: Adjust plane size to cover the furthest data points
            regressionPlane.transform.localScale = new Vector3(maxDistanceX * scaleFactor, 1, maxDistanceZ * scaleFactor); // Multiply by 2 to cover both sides from the center
        }
        else
        {
            Debug.LogError("Regression plane or data points are not set.");
        }


    }


}
