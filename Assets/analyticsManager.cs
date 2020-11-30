using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class analyticsManager : MonoBehaviour
{
    GoogleAnalyticsV4 ga;
    private void Start()
    {
        ga = GetComponent<GoogleAnalyticsV4>();
    }
    // Start is called before the first frame update
    public void URLClicked(string urlName)
    {
        ga.LogEvent("URLVisited", urlName, "label", 1);
    }
    public void PhotoTaken()
    {
        ga.LogEvent("PhotoTaken", ChantecailleARManager.Instance.selectedAnimalName, "label", 1);
    }
    public void SelfieTaken()
    {
        ga.LogEvent("SelfieTaken", ChantecailleARManager.Instance.selectedAnimalName, "label", 1);
    }
}
