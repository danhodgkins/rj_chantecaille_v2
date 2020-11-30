using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeController : MonoBehaviour
{
    CanvasGroup cg;
    public bool makeInteractable;
    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    public IEnumerator fadeIn(float fadeTime)
    {
        for (float f = 0; f < 1; f+= Time.deltaTime / fadeTime)
        {
            yield return null;
            cg.alpha = f;
        }
        cg.alpha = 1;
        if (makeInteractable)
            cg.interactable = true;
    }

    public IEnumerator fadeOut(float fadeTime)
    {
        for (float f = 0; f < 1; f+= Time.deltaTime / fadeTime)
        {
            yield return null;
            cg.alpha = 1 - f;
        }
        cg.alpha = 0;
    }
}
