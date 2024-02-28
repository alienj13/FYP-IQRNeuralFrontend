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
public class Group : MonoBehaviour
{
    private string Name { get; }
    // private Neuron[][] neurons;
    private int[,] test;
    private GameObject[,] cubes;
    private GameObject[,] dendrites;
    private string id;
    private List<double> data = new List<double>();
    private List<double> Animationdata = new List<double>();
    private List<Synapse> connections = new List<Synapse>();
    private List<Group> Targets = new List<Group>();
    private int xcount;
    private int ycount;
    System.Random random = new System.Random();
    private int pos;
    private Vector3 centerPosition;



    public Group(string id, string name, int xcount, int ycount, int pos)
    {
        cubes = new GameObject[xcount, ycount];
        // dendrites = new GameObject[xcount, ycount];

        this.pos = pos;
        this.id = id;
        this.Name = name;
        // neurons = new Neuron[xcount][ycount];
        test = new int[xcount, ycount];
        this.xcount = xcount;
        this.ycount = ycount;

        for (int i = 0; i < xcount; i++)
        {
            for (int j = 0; j < ycount; j++)
            {
                cubes[i, j] = Main.Instance.createNeuron(pos, i, j, xcount, ycount);
                //dendrites[i, j] = Main.Instance.CreateDendrite(cubes[i,j],pos);
            }
        }

    }


    public string getID()
    {
        return id;
    }

    public int getPos()
    {
        return pos;
    }

    public string getName()
    {
        return Name;
    }

    public void addData(double d)
    {
        data.Add(d);
    }

    public int getDataLength()
    {
        return data.Count;
    }

    public double getData()
    {
        double probability = data[0];
        data.RemoveAt(0);
        return probability;
    }

    public double getAnimationData()
    {
        double probability = data[0];
        data.RemoveAt(0);
        return probability;
    }

    public void addConnection(Synapse s)
    {
        connections.Add(s);
        Targets.Add(s.getTarget());
    }

    public GameObject[,] getNeurons()
    {
        return cubes;
    }

    public void StartSimulation()
    {
        List<Synapse> synapses = new List<Synapse>();
        foreach (Synapse s in connections)
        {
            if (s.getSource().getID().Equals(id))
            {
                synapses.Add(s);
                
            }
        }
        AnimationSequence.Instance.AnimationStart(synapses,Targets);
    }

    public void desroyGroup()
    {

        foreach (Synapse obj in connections)
        {
            Destroy(obj.getConnection());
        }
        foreach (GameObject obj in cubes)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in dendrites)
        {
            Destroy(obj);
        }

    }
}
