using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFact : MonoBehaviour
{
    public RectTransform button, factBox;
    public Transform targetBone;
    public string Fact;
    bool factVisible;
    bool animating;
    public FactGenerator generator;
    public int factNo;
    // Start is called before the first frame update
    void Awake()
    {
        factBox.transform.localScale = Vector3.zero;
    }

    public void PressButton()
    {
        if (!factVisible)        
            StartCoroutine(showFact());
        else
            StartCoroutine(hideFact());
    }
    IEnumerator showFact()
    {
        generator.hideAllFacts(factNo);
        if (!animating)
        {
            animating = true;
            factVisible = true;

            for (float f = 0; f < 1; f += Time.deltaTime / 0.3f)
            {                
                yield return null;
                button.transform.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(Vector3.forward * -45), f);
                factBox.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, f);

            }
            button.transform.localRotation = Quaternion.Euler(Vector3.forward * -45);
            factBox.transform.localScale = Vector3.one;
            animating = false;
        }
        
    }
    public IEnumerator hideFact()
    {
        if (!animating && factVisible)
        {
            animating = true;
            factVisible = false;

            for (float f = 0; f < 1; f += Time.deltaTime / 0.5f)
            {                
                yield return null;
                button.transform.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(Vector3.forward * 45), 1 - f);
                factBox.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, 1 - f);
            }
            button.transform.localRotation = Quaternion.Euler(Vector3.zero);
            factBox.transform.localScale = Vector3.zero;
            animating = false;
        }
        
    }

    void Update()
    {
        
    }
}
