using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimalInfoController : MonoBehaviour
{
    CanvasGroup cg;
    bool visible;
    public GameObject[] info;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < info.Length; i++)
        {
            if ("M2_Vanishing_Species_" + ChantecailleARManager.Instance.selectedAnimalName == info[i].name)
            {
                //info[i].SetActive(true);
            }else
            {
                Destroy(info[i]);
            }
        }
        
        cg = GetComponent<CanvasGroup>();
    }

    public void showInfo()
    {
        if (!visible) StartCoroutine(fadeIn(0.5f));
        else StartCoroutine(fadeOut(0.5f));
        visible = !visible;
    }

    public IEnumerator fadeIn(float fadeTime)
    {
        cg.blocksRaycasts = true;
        cg.interactable = true;
        cg.alpha = 0;
        for (float f = 0; f < 1; f += Time.deltaTime / fadeTime)
        {
            yield return null;
            cg.alpha = f;
        }
        cg.alpha = 1;
    }

    public IEnumerator fadeOut(float fadeTime)
    {
        cg.blocksRaycasts = false;
        cg.interactable = false;
        cg.alpha = 1;
        for (float f = 0; f < 1; f += Time.deltaTime / fadeTime)
        {
            yield return null;
            cg.alpha = 1 - f;
        }
        cg.alpha = 0;
    }
}
