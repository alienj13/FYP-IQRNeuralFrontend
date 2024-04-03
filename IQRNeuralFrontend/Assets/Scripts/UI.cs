using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    public RawImage display;
    public Group sp;

    void Awake()
    {
        Instance = this;
    }

    void Update() {

        if (sp != null)
        {
            display.texture = sp.GetSpacePlot();
        }

    }

    public void UpdateSpacePlot(Group g) {

        sp = g;
        
    }
}
