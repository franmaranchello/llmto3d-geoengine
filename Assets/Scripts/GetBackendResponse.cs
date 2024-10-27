using System;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Text;
using Palmmedia.ReportGenerator.Core.Common;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class GetBackendResponse : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();

    // The local endpoint URL (adjust as needed)
    private string endpointUrl = "http://127.0.0.1:8000/get-building";

    // The file path where the JSON response will be saved (Desktop example)
    private string outputPath;

    public void StartNow(string prompt)
    {
        // Save to the user's Desktop folder for easy access
        outputPath = Path.Combine(Application.dataPath, "response.json");
        
        // Prepare the JSON payload
        string jsonPayload = $"{{\"message\": \"{prompt}\"}}";
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Make the HTTP request using Unity's async API
        using (var request = client.PostAsync(endpointUrl, content).Result)
        {
                // Read the JSON response
                string jsonResponse = request.Content.ReadAsStringAsync().Result;
                jsonResponse = jsonResponse.Replace("\\n", "").Replace("\\", "");
                jsonResponse = jsonResponse.Substring(1, jsonResponse.Length - 2);
                Debug.Log($"Received response: {jsonResponse}");
                // Save the JSON response to a file
                File.WriteAllText(outputPath, jsonResponse);
        }
    }
}