using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connectionremap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        // Renderer rendererToChange =GetComponent<Renderer>();
        // Material[] newOrder = new Material[7];
        // newOrder[0] = rendererToChange.materials[2];
        // newOrder[1] = rendererToChange.materials[1];
        // newOrder[2] = rendererToChange.materials[0];
        // newOrder[3] = rendererToChange.materials[4];
        // newOrder[4] = rendererToChange.materials[5];
        // newOrder[5] = rendererToChange.materials[6];
        // newOrder[6] = rendererToChange.materials[3];
        // rendererToChange.materials = newOrder;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        int[] triangles  = mesh.GetTriangles(0);
        int[] triangles1 = mesh.GetTriangles(1);
        int[] triangles2 = mesh.GetTriangles(2); 
        int[] triangles3 = mesh.GetTriangles(3); 
        int[] triangles4 = mesh.GetTriangles(4);
        int[] triangles5 = mesh.GetTriangles(5); 
        int[] triangles6 = mesh.GetTriangles(6); 

        mesh.SetTriangles(triangles2, 1);
        mesh.SetTriangles(triangles3, 2);
        mesh.SetTriangles(triangles4, 3);
        mesh.SetTriangles(triangles6, 4);
        mesh.SetTriangles(triangles5, 5);
        mesh.SetTriangles(triangles1, 6);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
