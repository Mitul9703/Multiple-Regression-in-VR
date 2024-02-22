using UnityEngine;



public class Elevate : MonoBehaviour
{
    public float elevationHeight = 20.0f; // Set this to the desired elevation height
    public GameObject Total;
    // Start is called before the first frame update
    void Start()
    {
        ElevateSystem();
    }

    private void ElevateSystem()
    {
        // This will move the GameObject and all of its children up by elevationHeight units on the Y-axis.
        Total.transform.position = new Vector3(Total.transform.position.x, elevationHeight, Total.transform.position.z);
    }
}
