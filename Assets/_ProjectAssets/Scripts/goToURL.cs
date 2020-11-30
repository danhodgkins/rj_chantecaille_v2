using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goToURL : MonoBehaviour
{
    public void loadURL(string url)
    {
        GameObject.Find("GAv4").GetComponent<analyticsManager>().URLClicked(url);
        Application.OpenURL(url);
    }
}
