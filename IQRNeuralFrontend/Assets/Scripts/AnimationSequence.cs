using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class AnimationSequence : MonoBehaviour
{

    public static AnimationSequence Instance { get; private set; }

    void Awake()
    {
        Instance = this;

    }



    public void AnimationOn(GameObject neuron, string state)
    {
        AnimationEmissionController scriptInstance = neuron.GetComponent<AnimationEmissionController>();
        if (state.Equals("Source"))
        {
            StartCoroutine(scriptInstance.NeuronSendDataOn());
        }
        else
        {
            StartCoroutine(scriptInstance.NeuronReceieveDataOn());
        }
    }



    public void AnimationStart(List<Synapse> synapses, List<Group> Targets)
    {
        StartCoroutine(SequenceAnimation(synapses[0].getSource(), synapses, Targets));

    }

    public IEnumerator SequenceAnimation(Group source, List<Synapse> Synapses, List<Group> Target)
    {

        GroupAnimation(source,"Source");

        yield return new WaitForSeconds(2f);
        foreach (Synapse s in Synapses)
        {
             AnimationOn(s.getConnection(), "Target");
        }

        yield return new WaitForSeconds(2f);
        foreach (Group g in Target)
        {
            GroupAnimation(g, "Target");
        }

    }


    public void GroupAnimation(Group g, string state)
    {
        System.Random random = new System.Random();
        double probability = g.getData();
        GameObject[,] Neurons = g.getNeurons();
        int[,] probabilityGrid = new int[Neurons.GetLength(0), Neurons.GetLength(1)];
        Debug.Log(probability);
        
        int count = (int)(probabilityGrid.Length * probability);
        //Debug.Log("Number of neurons firing: " + count + "   " + id + "  " + probability);
        if (state.Equals("Source"))
        {
            while (count > 0)
            {
                int rand = random.Next(probabilityGrid.GetLength(1));
                int rand2 = random.Next(probabilityGrid.GetLength(1));

                if (probabilityGrid[rand, rand2] == 0)
                {
                    probabilityGrid[rand, rand2] = 1;
                    AnimationOn(Neurons[rand, rand2], state);
                    //Main.Instance.Turnon(dendrites[rand, rand2]);
                    count--;
                }
            }

            return;
        }
        else if (state.Equals("Target"))
        {
            while (count > 0)
            {
                int rand = random.Next(probabilityGrid.GetLength(1));
                int rand2 = random.Next(probabilityGrid.GetLength(1));

                if (probabilityGrid[rand, rand2] == 0)
                {
                    probabilityGrid[rand, rand2] = 1;
                    AnimationOn(Neurons[rand, rand2], state);
                    //Main.Instance.Turnon(dendrites[rand, rand2]);
                    count--;
                }
            }
            return;
        }

    }


}
