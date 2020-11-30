using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class barGraph : MonoBehaviour
{
    public Image[] barCharts;
    public Image[] chartImages;
    int[] animalValues;
    public PHPManager phpmanager;
    public void Start()
    {
        if(phpmanager !=null )phpmanager.fillGraph();
    }

    public void generateGraph(int[] values)
    {
        animalValues = values;
        StartCoroutine(fillBars());
    }

    IEnumerator fillBars()
    {
        for (int i = 0; i < barCharts.Length; i++)
        {
            barCharts[i].fillAmount = 0;
            chartImages[i].rectTransform.localPosition = Vector3.zero;
        }
        yield return new WaitForSeconds(0.8f);
        
        var x = maxVal();
        yield return null;
        for (float t = 0; t < 1; t += Time.deltaTime / 30)
        {
            for (int i = 0; i < animalValues.Length; i++)
            {
                float fill = ((float)animalValues[i] / x) * 0.6f;
                barCharts[i].fillAmount = Mathf.Lerp(barCharts[i].fillAmount, 0.15f + fill,t);
                float yHeight = barCharts[i].rectTransform.rect.height;
                chartImages[i].rectTransform.localPosition = new Vector3(0, Mathf.Lerp(0, yHeight, barCharts[i].fillAmount),0);
            }
            yield return null;
        }
    }

    private int maxVal()
    {
        int max = 0;
        for (int i = 0; i < animalValues.Length; i++)
        {
            if (animalValues[i] > max)
                max = animalValues[i];
        }
        return max;
    }

}
