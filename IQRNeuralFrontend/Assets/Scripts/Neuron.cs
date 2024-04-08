using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron : MonoBehaviour
{
    public Group group { get; private set; }

    public void Initialize(Group group)
    {
            this.group = group;
    }

    public void UpdatePlot()
    {
        if (this.group != null)
        {
            // Update the plot
            Debug.Log("Updating plot for group: " + group.getName());
            UI.Instance.UpdateGroup(group);
        }
    }
}

