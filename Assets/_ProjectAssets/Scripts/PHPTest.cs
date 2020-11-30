using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PHPTest : MonoBehaviour
{
    string highscoreURL = "https://ramjam.co.uk/chantecaille/getPhotoCount.php";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetScores());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetScores()
    {

        UnityWebRequest hs_get = UnityWebRequest.Get(highscoreURL);

        yield return hs_get.SendWebRequest();

        if (hs_get.isNetworkError)
        {
            Debug.Log("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            string AnimalNumbers = hs_get.downloadHandler.text.ToString();
            Debug.Log("success: " + AnimalNumbers);           
        }

    }

}
