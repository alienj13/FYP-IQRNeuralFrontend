using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron : MonoBehaviour
{
    public Group group { get; private set; }

    public void Initialize(Group group)
    {
        if (group == null)
        {
            Debug.LogError("Passed group instance is null upon initialization.");
        }
        else
        {
            this.group = group;
            Debug.Log("Neuron initialized with group: " + group.getName());
        }
    }

    public void UpdatePlot()
    {
        if (this.group == null)
        {
            Debug.LogError("Group is null when trying to update plot.");
        }
        else
        {
            // Update the plot
            Debug.Log("Updating plot for group: " + group.getName());
            UI.Instance.UpdateGroup(group);
        }
    }
}

