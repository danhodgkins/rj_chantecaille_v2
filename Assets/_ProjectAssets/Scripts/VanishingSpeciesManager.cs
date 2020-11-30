using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VanishingSpeciesManager : MonoBehaviour
{
    public Button prev, next;
    public GameObject[] AnimalInfo;
    int currentAnimal;
    // Start is called before the first frame update
    void Start()
    {
        switchAnimal(0);
    }

    public void switchAnimal(int val)
    {        
        currentAnimal = Mathf.Clamp(currentAnimal += val, 0, AnimalInfo.Length);
        for (int i = 0; i < AnimalInfo.Length; i++)
        {
            if (i == currentAnimal)
            {
                AnimalInfo[i].SetActive(true);
                //print(AnimalInfo[i].GetComponentInChildren<ScrollRect>().normalizedPosition);
                AnimalInfo[i].GetComponentInChildren<ScrollRect>().normalizedPosition = Vector2.up;
            }else
            {
                AnimalInfo[i].SetActive(false);
            }
        }
        if (currentAnimal == 0)
        {
            prev.interactable = false;
        }else
        {
            prev.interactable = true;
        }
        if (currentAnimal == AnimalInfo.Length - 1)
        {
            next.interactable = false;
        }else
        {
            next.interactable = true;
        }
    }

    void showAnimal()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
