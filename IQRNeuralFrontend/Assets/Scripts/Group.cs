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
public class Group 
{
    private string Name;

   private GameObject[,] Neurons;//neuron gameobjects
    private GameObject[,] dendrites;
    private string id;//identify unique groups

    private List<int[,]> NeuronProbabilityMatrix = new List<int[,]>();
    private List<double> Data = new List<double>();
    private List<Texture2D> SpacePlots = new List<Texture2D>();
    private List<int[,]> AxonProbabilityMatrix = new List<int[,]>();
    private List<Synapse> Synapses = new List<Synapse>();
    private List<Group> Targets = new List<Group>();
    private int xcount;
    private int ycount;
    System.Random random = new System.Random();
    private int Position;
    private Texture2D CurrentSpacePlot;
    private double CurrentData;



    public Group(string id, string name, int xcount, int ycount, int Position)
    {
        Neurons = new GameObject[xcount, ycount];
         dendrites = new GameObject[xcount, ycount];

        this.Position = Position;
        this.id = id;
        this.Name = name;
        this.xcount = xcount;
        this.ycount = ycount;

        for (int i = 0; i < xcount; i++)
        {
            for (int j = 0; j < ycount; j++)
            {
                GameObject neuronObj = Main.Instance.CreateNeuron(Position, i, j, xcount, ycount);
                Neuron neuronScript = neuronObj.GetComponent<Neuron>(); // Adds the Neuron component
                neuronScript.Initialize(this); // Passes the current group instance to the Neuron
                Neurons[i, j] = neuronObj;
                dendrites[i, j] = Main.Instance.CreateDendrite(Neurons[i,j],Position);
            }
        }
    }


    public string getID()
    {
        return id;
    }

    public int getPos()
    {
        return Position;
    }

    public string getName()
    {
        return Name;
    }

    public void addData(double d)
    {
        Data.Add(d);
    Texture2D SpacePlot = new Texture2D(Neurons.GetLength(0), Neurons.GetLength(1));
    int[,] probabilityGrid = new int[Neurons.GetLength(0), Neurons.GetLength(1)];
        System.Random random = new System.Random();
        double probability = d;
        int count = (int)(probabilityGrid.Length * probability);

        while (count > 0)
        {
            int rand = random.Next(probabilityGrid.GetLength(1));
            int rand2 = random.Next(probabilityGrid.GetLength(1));

            if (probabilityGrid[rand, rand2] == 0)
            {
                probabilityGrid[rand, rand2] = 1;
                count--;
                SpacePlot.SetPixel(rand, rand2, Color.red );
            }
        }


        for (int i = 0; i < probabilityGrid.GetLength(0); i++)
        {
            for (int j = 0; j < probabilityGrid.GetLength(1); j++)
            {
                if (probabilityGrid[i, j] == 0)
                {
                    SpacePlot.SetPixel(i, j, Color.black);
                }
            }
        }
        SpacePlot.Apply();
        SpacePlot.filterMode = FilterMode.Point;

        SpacePlots.Add(SpacePlot);
        NeuronProbabilityMatrix.Add(probabilityGrid);
        AxonProbabilityMatrix.Add(probabilityGrid); ;

    }


    public int getDataLength()
    {
        return NeuronProbabilityMatrix.Count;
    }

    public Texture2D GetSpacePlot() {
        return CurrentSpacePlot;
    }

    public double GetData()
    {
        return CurrentData;
    }

    public int[,] getNeuronMatrix()
    {
        CurrentData = Data[0];
        Data.RemoveAt(0);
        int[,] matrix = NeuronProbabilityMatrix[0];
        NeuronProbabilityMatrix.RemoveAt(0);
        CurrentSpacePlot = SpacePlots[0];
        SpacePlots.RemoveAt(0);
        return matrix;
    }

    public int[,] getAxonMatrix()
    {
        int[,] matrix = AxonProbabilityMatrix[0];
        AxonProbabilityMatrix.RemoveAt(0);
        return matrix;
    }

    public void addSynapse(Synapse s)
    {
        Synapses.Add(s);
        Targets.Add(s.getTarget());
    }

    public GameObject[,] getNeurons()
    {
        return Neurons;
    }

    public GameObject[,] getDendrites()
    {
        return dendrites;
    }

    public void StartSimulation()
    {
        List<Synapse> synapses = new List<Synapse>();
        foreach (Synapse s in Synapses)
        {
            if (s.getSource().getID().Equals(id))
            {
                synapses.Add(s);
                
            }
        }
        AnimationSequence.Instance.AnimationStart(synapses,Targets);
    }

    
}
