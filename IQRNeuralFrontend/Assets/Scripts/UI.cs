using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    public RawImage display;
    public Group sp;

    public int textureWidth = 500; // The width of the texture (and hence, the plot)
    public int textureHeight = 100; // The height of the texture
    public RawImage displayImage; // The RawImage component to display the plot
    private Texture2D plotTexture;

    private Color[] previousFrame;
    private int lastDataHeight = 0; // Set to baseline of the plot
    private float timeSinceLastData = 0f;
    private float dataInterval = 2f; // Time in seconds between data points
    public float MinValue { get; private set; }
    public float MaxValue { get; private set; }

    private int neuronsCount;
    private int rasterHeight; // Height of the raster plot texture
    private int rasterWidth; // Width of the raster plot texture
    private Texture2D rasterTexture; // Texture to draw the raster plot on
    public RawImage rasterDisplay;
    private int[,] neuronMatrix; // 2D array for neuron firing data
    private Color[] RasterPixels; // Array to hold the pixel data for the texture

    void Start()
    {
        MinValue = 0f;
        MaxValue = 1f;
        plotTexture = new Texture2D(textureWidth, textureHeight);
        displayImage.texture = plotTexture;
        previousFrame = new Color[textureWidth * textureHeight];

        for (int i = 0; i < previousFrame.Length; i++)
            previousFrame[i] = Color.black;
    }

    void Awake()
    {
        Instance = this;
    }

    void Update() {

        if (sp != null)
        {
           
            display.texture = sp.GetSpacePlot();
            UpdatePlotRoutine();

            neuronsCount = sp.GetX() * sp.GetY();
            rasterHeight = neuronsCount * 3;
            rasterWidth = neuronsCount * 3; // Set a fixed width for the texture
            initaliseNeuronMatrix();
            
            rasterTexture.filterMode = FilterMode.Point;
            rasterDisplay.texture = rasterTexture;
            AddNewDataToMatrix(); // Add new data for each frame
            ScrollRasterPlot(); // Update the plot with new data

        }
    }

    public void UpdateGroup(Group g) {
        
        sp = g;
        plotTexture = new Texture2D(textureWidth, textureHeight);
        displayImage.texture = plotTexture;
        previousFrame = new Color[textureWidth * textureHeight];

        for (int i = 0; i < previousFrame.Length; i++)
            previousFrame[i] = Color.black;


        //raster
        neuronsCount = sp.GetX() * sp.GetY();
        rasterHeight = neuronsCount * 3;
        rasterWidth = neuronsCount * 3; // Set a fixed width for the texture
        initaliseNeuronMatrix();
        InitializeTexture();
        rasterTexture.filterMode = FilterMode.Point;
        rasterDisplay.texture = rasterTexture;
    }



    private void UpdatePlotRoutine()
    {
            ScrollTimeline(); // This will scroll the timeline regardless of new data

            timeSinceLastData += 0.1f;
            if (timeSinceLastData >= dataInterval)
            {
                AddDataPoint(sp.GetData());
                timeSinceLastData = 0f;
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

        plotTexture.SetPixels(previousFrame);
        plotTexture.Apply();
    }


    private void AddDataPoint(double data)
    {
        int dataHeight = Mathf.Clamp((int)(data * textureHeight), 0, textureHeight - 1);
        DrawLine(textureWidth - 2, lastDataHeight, textureWidth - 1, dataHeight, Color.red);
        lastDataHeight = dataHeight;
    }

   
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


    //raster

    void initaliseNeuronMatrix()
    {
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
            
            yield return new WaitForSeconds(0.1f); // Update the plot every 0.1 seconds
        }
    }

    void AddNewDataToMatrix()
    {
        // Generate new firing data
        int[,] matrix2 = sp.getCurrentMatrix();
        for (int i = 0; i < matrix2.GetLength(0); i++)
        {
            for (int j = 0; j < matrix2.GetLength(1); j++)
            {
                int index = i * matrix2.GetLength(1) + j; // Calculate the flat index
                neuronMatrix[0,index] += matrix2[i, j];
            }
        }
    }

    void ScrollRasterPlot()
    {
        

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

        // Shift everything to the left by one pixel column
        for (int y = 0; y < rasterHeight; y++)
        {
            for (int x = 0; x < rasterWidth - 1; x++)
            {
                RasterPixels[y * rasterWidth + x] = RasterPixels[y * rasterWidth + x + 1];
            }
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
