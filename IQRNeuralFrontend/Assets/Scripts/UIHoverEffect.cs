using UnityEngine;
using UnityEngine.EventSystems; // Required when using Event data.
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class UIHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverOffset = new Vector3(0.1f, 0.1f, 0.1f); // Offset when hovered
    private Vector3 originalPosition;

    void Start()
    {
        // Store the original position of the panel
        originalPosition = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Raise the panel by the hover offset when the mouse hovers over
        transform.localScale = originalPosition + hoverOffset;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Return the panel to its original position when the mouse leaves t
        transform.localScale = originalPosition;
    }
    public void OnButtonPress()
    {
        // Return the panel to its original position when the mouse leaves t
        transform.localScale = originalPosition+ hoverOffset;
    }

     public void OnButtonPress2()
    {
        // Return the panel to its original position when the mouse leaves t
        transform.localScale = originalPosition;
    }
}
