using UnityEngine;
using UnityEngine.UI;

public class AxisLabelUpdater : MonoBehaviour
{
    public GameObject yAxisLabelPrefab; // Prefab for Y-Axis labels
    public GameObject canvas; // The canvas where the Text objects will be placed
    public float axisLength = 500f; // Length of the axis in canvas units
    public int numberOfLabels = 5; // Number of labels on the axis

    private void Start()
    {
        CreateYAxisLabels();
    }

    private void CreateYAxisLabels()
    {
        float step = axisLength / (numberOfLabels - 1); // Determine the space between labels

        for (int i = 0; i < numberOfLabels; i++)
        {
            GameObject labelObj = Instantiate(yAxisLabelPrefab, canvas.transform, false); // Instantiate the label
            Text labelText = labelObj.GetComponent<Text>();
            labelText.text = (i / (float)(numberOfLabels - 1)).ToString("0.0"); // Set the text to display the normalized value

            // Set the position of the label to be in line with yAxisLabelPrefab
            RectTransform labelRect = labelText.rectTransform;
            RectTransform originalLabelRect = yAxisLabelPrefab.GetComponent<RectTransform>();

            // Set the new label's position to match the original label's x position
            float yPos = step * i - (axisLength /1.8f); // Center the labels on the y-axis
            labelRect.anchoredPosition = new Vector2(originalLabelRect.anchoredPosition.x, yPos);

            // Align the pivot and anchors to the middle left, similar to the original label
            labelRect.pivot = originalLabelRect.pivot;
            labelRect.anchorMin = originalLabelRect.anchorMin;
            labelRect.anchorMax = originalLabelRect.anchorMax;
        }
    }





}
