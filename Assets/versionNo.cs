using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class versionNo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "v. " + Application.version.ToString();       
    }
}
