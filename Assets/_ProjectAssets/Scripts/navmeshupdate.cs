using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class navmeshupdate : MonoBehaviour
{
    NavMeshSurface[] surfaces;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        surfaces = GetComponentsInChildren<NavMeshSurface>();
        yield return null;  
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
}
