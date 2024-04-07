using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpacePlot : MonoBehaviour
{
    public int width = 10;  // Width of the grid
    public int height = 10; // Height of the grid
    public RawImage display; // The UI element to display the space plot

    void Update()
    {
        
            GenerateSpacePlot();
        
    }

    public void GenerateSpacePlot()
    {
        // Create a new texture with the specified width and height
        Texture2D spacePlotTexture = new Texture2D(width, height);

        // Loop over every pixel in the texture
        for (int i = 0; i < spacePlotTexture.width; i++)
        {
            for (int j = 0; j < spacePlotTexture.height; j++)
            {
                // Determine whether the neuron at this grid position is firing
                bool isFiring = ShouldNeuronFire(i, j); // You'll replace this with your actual data

                // Color the pixel red if firing, black if not
                spacePlotTexture.SetPixel(i, j, isFiring ? Color.red : Color.black);
            }
        }

        // Apply all SetPixel changes
        spacePlotTexture.Apply();

        // Set the texture on the display RawImage
        spacePlotTexture.filterMode = FilterMode.Point;
        display.texture = spacePlotTexture;
    }

    private bool ShouldNeuronFire(int x, int y)
    {
        // Placeholder for your neuron data check
        // Return true if the neuron at grid position (x, y) is firing, false otherwise
        return Random.Range(0, 2) == 0; // Randomly decide if a neuron is firing for this example
    }
}
