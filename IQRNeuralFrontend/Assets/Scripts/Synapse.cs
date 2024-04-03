using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synapse
{
    private string id;
    private Group Target;
    private Group Source;
    public GameObject connection;


    public Synapse(string id, string type, Group source, Group target)
    {
        this.id = id;
        this.Target = target;
        this.Source = source;
        connection = Main.Instance.CreateSynapse(source.getPos(), target.getPos());
    }

    public GameObject getConnection()
    {
        return connection;
    }

    public Group getSource()
    {
        return Source;
    }

    public Group getTarget()
    {
        return Target;
    }

   

}
