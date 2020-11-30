using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactGenerator : MonoBehaviour
{
    public Transform cam;
    public GameObject fact;
    Transform[] factPosition;
    AnimalFact[] facts;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        factPosition = GetComponentsInChildren<Transform>();
        facts = new AnimalFact[factPosition.Length];
        for (int i = 0; i < factPosition.Length; i++)
        {
            
            facts[i] = GameObject.Instantiate(fact).GetComponent<AnimalFact>() ;
            facts[i].generator = this;
            facts[i].factNo = i;
        }
    }

    public void hideAllFacts(int factNo)
    {
        for (int i = 0; i < facts.Length; i++)
        {
            if (factNo != i){
                StartCoroutine(facts[i].hideFact());
            }
        }
    }

    public void showIcons()
    {

    }

    public void hideIcons()
    {

    }

    void Update()
    {
        for (int i = 0; i < factPosition.Length; i++)
        {
            facts[i].transform.position = factPosition[i].position;
            facts[i].transform.rotation = Quaternion.LookRotation(cam.position - facts[i].transform.position, Vector3.up);
            facts[i].transform.rotation*= Quaternion.Euler(Vector3.up * 180);
        }
    }
}
