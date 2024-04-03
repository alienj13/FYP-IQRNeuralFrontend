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

public class Main : MonoBehaviour
{
    public static List<Group> Groups = new List<Group>();
    public static List<Group> SourceGroups = new List<Group>();
    public bool load = false;
    public bool play = false;
    public bool done = false;
    public GameObject neuronPrefab;
    public GameObject SynapsePrefab;
    public GameObject dendritePrefab;
    public static Main Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public Vector3 GetNeuronPosition(int pos, float cubeSize, float cubeHeight)
    {
        if (pos == 0)
            return new Vector3(0, 0, 0);

        int level = 1;
        int totalCubesInPreviousLevels = 1;
        int cubesInCurrentLevel = (level + 1) * (level + 1);

        while (pos >= totalCubesInPreviousLevels + cubesInCurrentLevel)
        {
            totalCubesInPreviousLevels += cubesInCurrentLevel;
            level++;
            cubesInCurrentLevel = (level + 1) * (level + 1);
        }

        int posInCurrentLevel = pos - totalCubesInPreviousLevels;
        int rowInCurrentLevel = posInCurrentLevel / (level + 1);
        int colInCurrentLevel = posInCurrentLevel % (level + 1);

        float x = (colInCurrentLevel - level / 2f) * cubeSize;
        float y = level * cubeHeight;
        float z = (rowInCurrentLevel - level / 2f) * cubeSize;

        return new Vector3(x, y, z);
    }

    public GameObject CreateNeuron(int pos, int i, int j, int rows, int cols)
    {

        int posnew = pos * 1500;
        float phi = (j / (float)(cols - 1)) * 2 * Mathf.PI;
        float poleOffset = 0.9f;
        float theta = ((i + poleOffset) / ((float)(rows - 1) + (2 * poleOffset))) * Mathf.PI;

        Vector3 v = GetNeuronPosition(pos, 2000, 2000);
        Vector3 position;

        if (pos == 0)
        {
            position = new Vector3(
                500.0f * Mathf.Sin(theta) * Mathf.Cos(phi),
                500.0f * Mathf.Cos(theta),
                500.0f * Mathf.Sin(theta) * Mathf.Sin(phi)
            );
        }
        else
        {
            position = new Vector3(
                500.0f * Mathf.Sin(theta) * Mathf.Cos(phi),
                500.0f * Mathf.Cos(theta),
                500.0f * Mathf.Sin(theta) * Mathf.Sin(phi)
            );
        }

        GameObject neuron = Instantiate(neuronPrefab, position + v, Quaternion.identity, transform);
  
        Vector3 directionToCenter = (v - neuron.transform.position).normalized;
        neuron.transform.rotation = Quaternion.LookRotation(-directionToCenter);
        neuron.transform.Rotate(90f, 0f, 0f, Space.Self);
        return neuron;
    }

    public GameObject CreateSynapse(int sourcePos, int targetPos)
    {
        Vector3 sourcePosition = GetNeuronPosition(sourcePos, 2000, 2000);
        Vector3 targetPosition = GetNeuronPosition(targetPos, 2000, 2000);
        Vector3 midpoint = (sourcePosition + targetPosition) / 2f;

        GameObject connection = GameObject.Instantiate(SynapsePrefab, midpoint, Quaternion.identity);

        float distance = Vector3.Distance(sourcePosition, targetPosition);
        connection.transform.localScale = new Vector3(connection.transform.localScale.x / 2f, distance / 2f, connection.transform.localScale.z / 2f);
        connection.transform.up = targetPosition - sourcePosition;

        return connection;
    }

    public GameObject CreateDendrite(GameObject n, int targetPos)
    {
        Vector3 sourcePosition = n.transform.position;
        Vector3 targetPosition = GetNeuronPosition(targetPos, 2000, 2000);
        Vector3 midpoint = (sourcePosition + targetPosition) / 2f;

        GameObject dendrite = GameObject.Instantiate(dendritePrefab, midpoint, Quaternion.identity);
        float distance = Vector3.Distance(sourcePosition, targetPosition);
        dendrite.transform.localScale = new Vector3(dendrite.transform.localScale.x/2 , distance/2f , dendrite.transform.localScale.z/2);
        dendrite.transform.up =   sourcePosition- targetPosition;

        return dendrite;
    }


    void Update()
    {
        if (load && play && SourceGroups[0].getDataLength() > 0 && !done)
        {
            foreach (Group g in SourceGroups)
            {
                g.StartSimulation();
            }
          //  done = true;
        }
    }

    public void Upload()
    {

        UnityEditorTest();

        //FileUpload();
    }

    [DllImport("__Internal")]
    private static extern void FileUpload();

    public void Play()
    {
        if (load)
        {
            play = !play;
        }
    }

    public void UnityEditorTest()
    {
        Clear();

        try
        {

            string filePath = "C:/Users/junai/eclipse-workspace/FYP/data/Test.iqr"; // Update with the correct path
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNodeList groupNodes = xmlDoc.GetElementsByTagName("Group");
            int pos = 0;

            foreach (XmlNode groupNode in groupNodes)
            {
                XmlElement groupElement = (XmlElement)groupNode;

                Debug.Log("Group: ");
                Debug.Log("    name: " + groupElement.GetAttribute("name"));
                Debug.Log("    ID: " + groupElement.GetAttribute("id"));
                string Gid = groupElement.GetAttribute("id");
                string Gname = groupElement.GetAttribute("name");

                XmlElement topologyRect = (XmlElement)groupElement.GetElementsByTagName("TopologyRect")[0];
                Debug.Log("    Neuron X: " + topologyRect.GetAttribute("hcount"));
                Debug.Log("    Neuron Y: " + topologyRect.GetAttribute("vcount"));
                int xCount = int.Parse(topologyRect.GetAttribute("hcount"));
                string yCount = topologyRect.GetAttribute("vcount");

                XmlElement neurons = (XmlElement)groupElement.GetElementsByTagName("Neuron")[0];
                Debug.Log("    Neuron name: " + neurons.GetAttribute("name"));
                Groups.Add(new Group(Gid, Gname, xCount, int.Parse(yCount), pos));
                pos++;
            }

            groupNodes = xmlDoc.GetElementsByTagName("Connection");

            foreach (XmlNode groupNode in groupNodes)
            {
                XmlElement groupElement = (XmlElement)groupNode;
                Debug.Log("    ID: " + groupElement.GetAttribute("id"));
                string id = groupElement.GetAttribute("id");

                Debug.Log("    type: " + groupElement.GetAttribute("type"));
                string type = groupElement.GetAttribute("type");
                Debug.Log("    source: " + groupElement.GetAttribute("source"));
                Debug.Log("    target: " + groupElement.GetAttribute("target"));
                Group[] TS = FindGroups(groupElement.GetAttribute("source"), groupElement.GetAttribute("target"));

                Synapse s = new Synapse(id, type, TS[0], TS[1]);
                TS[0].addSynapse(s);
                TS[1].addSynapse(s);
                if (!SourceGroups.Contains(TS[0])) { 
                    SourceGroups.Add(TS[0]);
                }
            }
            Debug.Log(SourceGroups.Count);
            play = false;
            load = true;
        }
        catch (XmlException xmlEx)
        {
            Debug.LogError("XML Exception: " + xmlEx.Message);
        }
    }

    public void ReceiveFileContent(string Contents)
    {

        Clear();

        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Contents);
            XmlNodeList groupNodes = xmlDoc.GetElementsByTagName("Group");
            int pos = 0;

            foreach (XmlNode groupNode in groupNodes)
            {
                XmlElement groupElement = (XmlElement)groupNode;

                Debug.Log("Group: ");
                Debug.Log("    name: " + groupElement.GetAttribute("name"));
                Debug.Log("    ID: " + groupElement.GetAttribute("id"));
                string Gid = groupElement.GetAttribute("id");
                string Gname = groupElement.GetAttribute("name");

                XmlElement topologyRect = (XmlElement)groupElement.GetElementsByTagName("TopologyRect")[0];
                Debug.Log("    Neuron X: " + topologyRect.GetAttribute("hcount"));
                Debug.Log("    Neuron Y: " + topologyRect.GetAttribute("vcount"));
                int xCount = int.Parse(topologyRect.GetAttribute("hcount"));
                string yCount = topologyRect.GetAttribute("vcount");

                XmlElement neurons = (XmlElement)groupElement.GetElementsByTagName("Neuron")[0];
                Debug.Log("    Neuron name: " + neurons.GetAttribute("name"));
                Groups.Add(new Group(Gid, Gname, xCount, int.Parse(yCount), pos));
                pos++;
            }

            groupNodes = xmlDoc.GetElementsByTagName("Connection");

            foreach (XmlNode groupNode in groupNodes)
            {
                XmlElement groupElement = (XmlElement)groupNode;
                Debug.Log("    ID: " + groupElement.GetAttribute("id"));
                string id = groupElement.GetAttribute("id");

                Debug.Log("    type: " + groupElement.GetAttribute("type"));
                string type = groupElement.GetAttribute("type");
                Debug.Log("    source: " + groupElement.GetAttribute("source"));
                Debug.Log("    target: " + groupElement.GetAttribute("target"));
                Group[] TS = FindGroups(groupElement.GetAttribute("source"), groupElement.GetAttribute("target"));

                Synapse s = new Synapse(id, type, TS[0], TS[1]);
                TS[0].addSynapse(s);
                TS[1].addSynapse(s);
                if (!SourceGroups.Contains(TS[0]))
                {
                    SourceGroups.Add(TS[0]);
                }
            }
            play = false;
            load = true;
        }
        catch (XmlException xmlEx)
        {
            Debug.LogError("XML Exception: " + xmlEx.Message);
        }
    }

    public void SortData(string data)
    {
        List<double> tempData = new List<double>();

        string[] pairs = data.Split(';');

        for (int i = 0; i < pairs.Length - 1; i++)
        {
            string[] parts = pairs[i].Split(':');
            string id = parts[0];

            double value = double.Parse(parts[1]);
            if (value > 1)
            {
                value = 1;
            }
            foreach (Group g in Groups)
            {
                if (g.getID().Equals(id))
                {
                    g.addData(value);
                }
            }
        }
    }

    public Group[] FindGroups(string source, string target)
    {
        Group[] g = new Group[2];

        foreach (Group group in Groups)
        {
            if (group.getID().Equals(source))
            {
                g[0] = group;

            }
            else if (group.getID().Equals(target))
            {
                g[1] = group;
            }
        }
        return g;
    }

    public void Clear()
    {
        play = false;
        foreach (Group group in Groups)
        {
            //group.desroyGroup();
        }
        Groups = new List<Group>();

    }

    public void test() {
        Debug.Log("test");
    }
}
