using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animate : MonoBehaviour
{
    public static bool done = true;
    public GameObject j;
    AnimationEmissionController scriptInstance;
    
    void Start()
    {   

        GameObject neuron = Instantiate(j, new Vector3(0,0,0), Quaternion.identity, transform);
          scriptInstance = neuron.GetComponent<AnimationEmissionController>();
           //StartCoroutine(scriptInstance.ChangeEmissionIntensity());
         
    }

    // Update is called once per frame
    void Update()
    {
          
    }
}
