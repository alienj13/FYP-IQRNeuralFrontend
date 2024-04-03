using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NeuronClickDetector : MonoBehaviour, IPointerClickHandler
{
    public Camera renderCamera; // The camera that renders to the RenderTexture
    public RawImage rawImage; // The RawImage displaying the RenderTexture

    public void OnPointerClick(PointerEventData eventData)
    {
        // Convert click position to viewport space
        Vector2 clickPos = eventData.position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, clickPos, null, out Vector2 localCursor);
        Vector2 normalizedPos = Rect.PointToNormalized(rawImage.rectTransform.rect, localCursor);

        // Raycast into the 3D scene
        Ray ray = renderCamera.ViewportPointToRay(normalizedPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object is a neuron
            Neuron neuron = hit.collider.GetComponent<Neuron>();
            if (neuron != null)
            {
                neuron.UpdatePlot(); // Call the function on the Neuron script
            }
        }
    }
}
