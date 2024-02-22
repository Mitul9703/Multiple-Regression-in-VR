using UnityEngine;

public class AxesDrawer : MonoBehaviour
{
    public DataManager dataManager; // Reference to your DataManager script
    public Material lineMaterial; // Assign a material in the Inspector

    private void Start()
    {
        DrawAxes();
    }

    public void DrawAxes()
    {
        // Check if dataManager is set
        if (dataManager == null) return;

        // Assuming dataManager has already calculated the data bounds
        float maxX = dataManager.maxX;
        float maxY = dataManager.maxY;
        float maxZ = dataManager.maxZ;

        // Create X Axis
        DrawLine(new Vector3(0, 0, 0), new Vector3(maxX, 0, 0), Color.red);

        // Create Y Axis
        DrawLine(new Vector3(0, 0, 0), new Vector3(0, maxY, 0), Color.green);

        // Create Z Axis
        DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, maxZ), Color.blue);
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.SetParent(transform); // Set as child of this GameObject
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }


}
