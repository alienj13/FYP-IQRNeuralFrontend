using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axonremap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {



        Mesh mesh = GetComponent<MeshFilter>().mesh;
        int[] triangles  = mesh.GetTriangles(0);
        int[] triangles1 = mesh.GetTriangles(1);
        int[] triangles2 = mesh.GetTriangles(2); 
        int[] triangles3 = mesh.GetTriangles(3); 
        int[] triangles4 = mesh.GetTriangles(4);
        int[] triangles5 = mesh.GetTriangles(5); 
        int[] triangles6 = mesh.GetTriangles(6);
        int[] triangles7 = mesh.GetTriangles(7);
        int[] triangles8 = mesh.GetTriangles(8);
        int[] triangles9 = mesh.GetTriangles(9);
        int[] triangles10 = mesh.GetTriangles(10);
        int[] triangles11 = mesh.GetTriangles(11);
        int[] triangles12 = mesh.GetTriangles(12);
        int[] triangles13= mesh.GetTriangles(13);
        int[] triangles14 = mesh.GetTriangles(14);
        int[] triangles15 = mesh.GetTriangles(15);
        int[] triangles16 = mesh.GetTriangles(16);
        int[] triangles17 = mesh.GetTriangles(17);
        int[] triangles18 = mesh.GetTriangles(18);
        int[] triangles19 = mesh.GetTriangles(19);
     

        mesh.SetTriangles(triangles1, 0);
        mesh.SetTriangles(triangles, 1);
        mesh.SetTriangles(triangles3, 2);
        mesh.SetTriangles(triangles4, 3);
        mesh.SetTriangles(triangles5, 4);
        mesh.SetTriangles(triangles6, 5);
        mesh.SetTriangles(triangles7, 6);
        mesh.SetTriangles(triangles8, 7);
        mesh.SetTriangles(triangles9, 8);
        mesh.SetTriangles(triangles10, 9);
        mesh.SetTriangles(triangles11, 10);
        mesh.SetTriangles(triangles12, 11);
        mesh.SetTriangles(triangles13, 12);
        mesh.SetTriangles(triangles14, 13);
        mesh.SetTriangles(triangles15, 14);
        mesh.SetTriangles(triangles16, 15);
        mesh.SetTriangles(triangles17, 16);
        mesh.SetTriangles(triangles18, 17);
        mesh.SetTriangles(triangles19, 18);
        mesh.SetTriangles(triangles2, 19);
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
