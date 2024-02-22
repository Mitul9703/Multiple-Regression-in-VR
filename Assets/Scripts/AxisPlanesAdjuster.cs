using UnityEngine;

public class AxisPlanesAdjuster : MonoBehaviour
{
    public GameObject xyPlane;
    public GameObject yzPlane;
    public GameObject xzPlane;
    public float planeSize = 10f; // Adjust this size as needed

    void Start()
    {
        // Adjust the YZ plane
        yzPlane.transform.rotation = Quaternion.Euler(0, 90, 0);
        yzPlane.transform.position = new Vector3(0, 0, 0); // Adjust as needed

        // Adjust the XZ plane
        xzPlane.transform.rotation = Quaternion.Euler(90, 0, 0);
        xzPlane.transform.position = new Vector3(0, 0, 0); // Adjust as needed

        // Scale the planes
        xyPlane.transform.localScale = new Vector3(planeSize, 1, planeSize);
        yzPlane.transform.localScale = new Vector3(planeSize, 1, planeSize);
        xzPlane.transform.localScale = new Vector3(planeSize, 1, planeSize);
    }
}
