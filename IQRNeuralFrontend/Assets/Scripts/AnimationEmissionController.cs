using System.Collections;
using UnityEngine;

public class AnimationEmissionController : MonoBehaviour
{
    private float duration = 0.3f;
    public float targetEmissionIntensity;

    public IEnumerator NeuronReceieveDataOn()
    {
        Renderer rendererToChange = GetComponent<Renderer>();
        Material[] materialsToChange = rendererToChange.materials;

        for (int i = materialsToChange.Length - 1; i >= 0; i--)
        {
            //Debug.Log(i);
            Material materialToChange = materialsToChange[i];
            if (materialToChange == null) continue;

            float elapsedTime = 0;
            Color baseEmissionColor = materialToChange.GetColor("_EmissionColor");
            Color targetColour = new Color(1f, 0.235f, 0.235f);
            if (i == materialsToChange.Length / 2) { StartCoroutine(NeuronReceieveDataOff(targetColour, baseEmissionColor, 0)); }
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float fraction = elapsedTime / duration;
                float newIntensity = Mathf.Lerp(0, targetEmissionIntensity, fraction);
                Color newColour = Color.Lerp(Color.black, targetColour, fraction);

                newColour *= newIntensity;
                materialToChange.SetColor("_EmissionColor", newColour);
                DynamicGI.SetEmissive(rendererToChange, newColour);

                yield return null;
            }

            Color finalColor = targetColour * targetEmissionIntensity;
            materialToChange.SetColor("_EmissionColor", finalColor);
            DynamicGI.SetEmissive(rendererToChange, finalColor);
        }
    }




    private IEnumerator NeuronReceieveDataOff(Color targetcol, Color baseCol, float baseintensity)
    {
        Renderer rendererToChange = GetComponent<Renderer>();
        Material[] materialsToChange = rendererToChange.materials;

        for (int i = materialsToChange.Length - 1; i >= 0; i--)
        {

            float originalIntensity = materialsToChange[i].GetColor("_EmissionColor").maxColorComponent;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float fraction = elapsedTime / duration;
                float newIntensity = Mathf.Lerp(originalIntensity, baseintensity, fraction);
                Color newColour = Color.Lerp(targetcol, baseCol, fraction);
                newColour *= newIntensity;
                materialsToChange[i].SetColor("_EmissionColor", newColour);
                DynamicGI.SetEmissive(rendererToChange, newColour);

                yield return null;
            }
        }

    }



    public IEnumerator NeuronSendDataOn()
    {
        Renderer rendererToChange = GetComponent<Renderer>();
        Material[] materialsToChange = rendererToChange.materials;

        for (int i = 0; i < materialsToChange.Length; i++)
        {
            Material materialToChange = materialsToChange[i];
            if (materialToChange == null) continue;

            float elapsedTime = 0;
            Color baseEmissionColor = materialToChange.GetColor("_EmissionColor");
            Color targetColour = new Color(1f, 0.235f, 0.235f);
            if (i == materialsToChange.Length / 2) { StartCoroutine(NeuronSendDataOff(targetColour, baseEmissionColor, 0)); }
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float fraction = elapsedTime / duration;
                float newIntensity = Mathf.Lerp(0, targetEmissionIntensity, fraction);
                Color newColour = Color.Lerp(Color.black, targetColour, fraction);

                newColour *= newIntensity;
                materialToChange.SetColor("_EmissionColor", newColour);
                DynamicGI.SetEmissive(rendererToChange, newColour);

                yield return null;
            }

            Color finalColor = targetColour * targetEmissionIntensity;
            materialToChange.SetColor("_EmissionColor", finalColor);
            DynamicGI.SetEmissive(rendererToChange, finalColor);
        }
    }




    private IEnumerator NeuronSendDataOff(Color targetcol, Color baseCol, float baseintensity)
    {
        Renderer rendererToChange = GetComponent<Renderer>();
        Material[] materialsToChange = rendererToChange.materials;

        for (int i = 0; i < materialsToChange.Length; i++)
        {

            float originalIntensity = materialsToChange[i].GetColor("_EmissionColor").maxColorComponent;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float fraction = elapsedTime / duration;
                float newIntensity = Mathf.Lerp(originalIntensity, baseintensity, fraction);
                Color newColour = Color.Lerp(targetcol, baseCol, fraction);
                newColour *= newIntensity;
                materialsToChange[i].SetColor("_EmissionColor", newColour);
                DynamicGI.SetEmissive(rendererToChange, newColour);

                yield return null;
            }
        }


    }

}
