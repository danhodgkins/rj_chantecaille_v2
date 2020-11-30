using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadAnimal : MonoBehaviour
{
    public Transform animalSpawner;
    // Start is called before the first frame update
    void Start()
    {
        GameObject clone = GameObject.Instantiate(ChantecailleARManager.Instance.selectedAnimal,animalSpawner);
        clone.transform.localEulerAngles = Vector3.up * 180;
        clone.GetComponentInChildren<shadowPlane>().gameObject.SetActive(false);
        //position fixes
        if (clone.name == "AnimalControllergiraffe(Clone)")
            animalSpawner.transform.position = new Vector3(1.04f, -3.43f, 0);
        if (clone.name == "AnimalControllerelephant(Clone)")
            animalSpawner.transform.position = new Vector3(1.57f, -2.83f, 0);
        if (clone.name == "AnimalControllerrhino(Clone)")
            animalSpawner.transform.position = new Vector3(0.91f, -0.96f, 0);
    }
}
