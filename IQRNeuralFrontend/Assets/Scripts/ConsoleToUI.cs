using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConsoleToUI : MonoBehaviour
{
    public Text consoleText; // Assign this in the inspector to your UI Text element
    private Queue<string> logQueue = new Queue<string>();
    private string currentText = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // You can ignore the stackTrace and type if you don't want them to be displayed
        logQueue.Enqueue(logString + "\n");

        // Optional: Choose how many lines of log you want to display
        if (logQueue.Count > 10) // For example, limit to 10 lines
        {
            logQueue.Dequeue(); // Remove the oldest line
        }

        currentText = string.Join("", logQueue.ToArray()); // Concatenate the strings into one
        consoleText.text = currentText; // Display the text in the UI
    }

    // Optional: Implement methods to clear the log, save it to a file, etc.
}
