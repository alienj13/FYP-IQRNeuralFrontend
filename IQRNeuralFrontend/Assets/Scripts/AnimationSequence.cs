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

    public void Play(GameObject neuron, string state)
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

        NeuronAnimation(source, "Source");
        yield return new WaitForSeconds(1f);
        AxonAnimation(source, "Source");

        yield return new WaitForSeconds(1f);
        foreach (Synapse s in Synapses)
        {
            Play(s.getConnection(), "Target");
        }

        yield return new WaitForSeconds(1f);

        foreach (Group g in Target)
        {
            AxonAnimation(g, "Target");
        }
        yield return new WaitForSeconds(1f);
        foreach (Group g in Target)
        {
            NeuronAnimation(g, "Target");
        }

    }

    public void NeuronAnimation(Group g, string state)
    {
        GameObject[,] Neurons = g.getNeurons();
        int[,] probabilityGrid = g.getNeuronMatrix();

        for (int i = 0; i < probabilityGrid.GetLength(0); i++)
        {
            for (int j = 0; j < probabilityGrid.GetLength(1); j++)
            {
                if (probabilityGrid[i, j] == 1)
                {

                    Play(Neurons[i, j], state);
                }
            }
        }
    }

    public void AxonAnimation(Group g, string state)
    {
        GameObject[,] Dendrites = g.getDendrites();
        int[,] probabilityGrid = g.getAxonMatrix();

        for (int i = 0; i < probabilityGrid.GetLength(0); i++)
        {
            for (int j = 0; j < probabilityGrid.GetLength(1); j++)
            { 
                if (probabilityGrid[i, j] == 1)
                {
                    Play(Dendrites[i, j], state);
                }
            }
        }
    }
}
