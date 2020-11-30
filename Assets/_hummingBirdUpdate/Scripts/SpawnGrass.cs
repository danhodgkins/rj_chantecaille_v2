using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrass : MonoBehaviour
{
    
    Camera cam;
    public GameObject grass;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2));
        RaycastHit raycasthit;
        if (Physics.Raycast(ray, out raycasthit))
        {
            if (raycasthit.transform.tag == "floor")
            {
                
                RaycastHit sphereRayHit;
                if (Physics.SphereCast(raycasthit.point + (Vector3.up * 0.3f),0.3f,transform.up * -1,out sphereRayHit)){
                    
                    if (raycasthit.transform.tag != "grass")
                    {
                        GameObject grassClone = Instantiate(grass,raycasthit.point,Quaternion.identity);
                        grassClone.transform.localScale = Vector3.one * Random.Range(0.75f, 1.25f);
                        grassClone.transform.localRotation = Quaternion.Euler(Random.insideUnitSphere * 20);
                        grassClone.GetComponent<SphereCollider>().radius = Random.Range(0.1f, 0.3f);
                    }
                }
            }
        }
    }
}
