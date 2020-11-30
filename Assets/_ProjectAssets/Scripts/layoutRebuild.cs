using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class layoutRebuild : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        GetComponent<HorizontalLayoutGroup>().enabled = false;
        yield return null;
        GetComponent<HorizontalLayoutGroup>().enabled = true;
    }

}
