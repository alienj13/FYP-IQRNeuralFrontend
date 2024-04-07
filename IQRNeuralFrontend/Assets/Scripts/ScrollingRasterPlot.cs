using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollingRasterPlot : MonoBehaviour
{
    public int neuronsCount = 25; // Total number of neurons
    private int rasterHeight; // Height of the raster plot texture
    private int rasterWidth; // Width of the raster plot texture
    private Texture2D rasterTexture; // Texture to draw the raster plot on
    public RawImage rasterDisplay;
    private int[,] neuronMatrix; // 2D array for neuron firing data
    private Color[] RasterPixels; // Array to hold the pixel data for the texture

    void Start()
    {
        // Set the raster dimensions based on the number of neurons
        rasterHeight = neuronsCount * 3;
        rasterWidth = neuronsCount * 3; // Set a fixed width for the texture

        InitializeNeuronMatrix();
        InitializeTexture();
        rasterTexture.filterMode = FilterMode.Point;
        rasterDisplay.texture = rasterTexture;
        StartCoroutine(UpdateRasterPlotRoutine());
    }

    void InitializeNeuronMatrix()
    {
        // Initialize the neuron matrix with just one row which will be scrolled
        neuronMatrix = new int[1, neuronsCount];
    }

    void InitializeTexture()
    {
        rasterTexture = new Texture2D(rasterWidth, rasterHeight, TextureFormat.RGBA32, false);
        ClearTexture();
    }

    IEnumerator UpdateRasterPlotRoutine()
    {
        while (true)
        {
            AddNewDataToMatrix(); // Add new data for each frame
            ScrollRasterPlot(); // Update the plot with new data
            yield return new WaitForSeconds(0.1f); // Update the plot every 0.1 seconds
        }
    }

    void AddNewDataToMatrix()
    {
        // Generate new firing data
        for (int i = 0; i < neuronsCount; i++)
        {
            neuronMatrix[0, i] = Random.Range(0, 2); // Random firing data
        }
    }

    void ScrollRasterPlot()
    {
        // Shift everything to the left by one pixel column
        for (int y = 0; y < rasterHeight; y++)
        {
            for (int x = 0; x < rasterWidth - 1; x++)
            {
                RasterPixels[y * rasterWidth + x] = RasterPixels[y * rasterWidth + x + 1];
            }
        }

        // Add new data to the last column
        for (int i = 0; i < neuronsCount; i++)
        {
            // Calculate the Y position for each neuron
            int baseY = i * 3; // Multiply by 3 to account for spacing
            Color newColor = neuronMatrix[0, i] == 1 ? Color.red : Color.black;

            // Update the last column with new data
            RasterPixels[baseY * rasterWidth + rasterWidth - 1] = newColor;
            // Ensure spacing between neurons
            RasterPixels[(baseY + 1) * rasterWidth + rasterWidth - 1] = Color.black;
            RasterPixels[(baseY + 2) * rasterWidth + rasterWidth - 1] = Color.black;
        }

        // Apply the updated pixel data to the texture
        rasterTexture.SetPixels(RasterPixels);
        rasterTexture.Apply();
    }

    void ClearTexture()
    {
        RasterPixels = new Color[rasterWidth * rasterHeight];
        for (int i = 0; i < RasterPixels.Length; i++)
        {
            RasterPixels[i] = Color.black;
        }
        rasterTexture.SetPixels(RasterPixels);
        rasterTexture.Apply();
    }
}
