using UnityEngine;
using System.Collections.Generic;

public class DataController : MonoBehaviour
{
    void Start()
    {
        LoadAndProcessData();
    }

    void LoadAndProcessData()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("loss_surface_data");
        foreach (var row in data)
        {
            // Example of accessing the values. Convert to the appropriate type as needed.
            float m = float.Parse(row["m"].ToString());
            float b = float.Parse(row["b"].ToString());
            float loss = float.Parse(row["loss"].ToString());

            // Now, you can use m, b, and loss for whatever processing you need, like visualization.
            Debug.Log($"m: {m}, b: {b}, loss: {loss}");
        }
    }
}
