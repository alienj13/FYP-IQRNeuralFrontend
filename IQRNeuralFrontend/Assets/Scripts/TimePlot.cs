using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimePlot : MonoBehaviour
{
    public int textureWidth = 500; // The width of the texture (and hence, the plot)
    public int textureHeight = 100; // The height of the texture
    public RawImage displayImage; // The RawImage component to display the plot
    //public Color backgroundColor = Color.black; // Background color

    private Texture2D plotTexture;
    private Color[] previousFrame;
    private int lastDataHeight = 0; // Set to baseline of the plot
    private float timeSinceLastData = 0f;
    private float dataInterval = 2f; // Time in seconds between data points
    public float MinValue { get; private set; }
    public float MaxValue { get; private set; }

    void Start()
    {
        MinValue = 0f; // Example: the minimum value of your data
        MaxValue = 1f;
        plotTexture = new Texture2D(textureWidth, textureHeight);
        displayImage.texture = plotTexture;
        previousFrame = new Color[textureWidth * textureHeight];

        for (int i = 0; i < previousFrame.Length; i++)
            previousFrame[i] = Color.black; // Initialize the plot with the background color

        plotTexture.filterMode = FilterMode.Point;
        // Start updating the plot at a regular interval
        StartCoroutine(UpdatePlotRoutine());
    }

    private IEnumerator UpdatePlotRoutine()
    {
        while (true)
        {
            ScrollTimeline(); // This will scroll the timeline regardless of new data
            yield return new WaitForSeconds(0.1f); // Update rate of the plot (10 times per second)

            timeSinceLastData += 0.1f;
            if (timeSinceLastData >= dataInterval)
            {
                // Add new data point and reset the timer
                AddDataPoint(Random.value);
                timeSinceLastData = 0f;
            }
        }
    }

    private void ScrollTimeline()
    {
        // Shift the texture to the left by one pixel
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 1; x < textureWidth; x++)
            {
                previousFrame[(x - 1) + y * textureWidth] = previousFrame[x + y * textureWidth];
            }
        }

        // Clear the rightmost pixel column with the background color
        for (int y = 0; y < textureHeight; y++)
        {
            previousFrame[(textureWidth - 1) + y * textureWidth] = Color.black;
        }

        // Draw a baseline from the last data point to the current edge of the plot to simulate continuity
        if (lastDataHeight >= 0)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                if (y == lastDataHeight)
                {
                    previousFrame[(textureWidth - 1) + y * textureWidth] = Color.red;
                }
                else
                {
                    previousFrame[(textureWidth - 1) + y * textureWidth] = Color.black;
                }
            }
        }

        // Update the texture
        plotTexture.SetPixels(previousFrame);
        plotTexture.Apply();
    }


    private void AddDataPoint(float data)
    {
        // Calculate the new data point's height
        int dataHeight = Mathf.Clamp((int)(data * textureHeight), 0, textureHeight - 1);

        // Draw a line from the last data point to the new data point
        DrawLine(textureWidth - 2, lastDataHeight, textureWidth - 1, dataHeight, Color.red);

        // Update the last data height
        lastDataHeight = dataHeight;
    }

    // Bresenham's line algorithm implementation in C#
    private void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;

        for (; ; )
        {
            if (x0 >= 0 && x0 < textureWidth && y0 >= 0 && y0 < textureHeight)
                previousFrame[x0 + y0 * textureWidth] = color;
            if (x0 == x1 && y0 == y1) break;
            e2 = 2 * err;
            if (e2 > dy) { err += dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }
    }
}
