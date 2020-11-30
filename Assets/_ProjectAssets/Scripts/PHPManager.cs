using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Security.Cryptography.X509Certificates;


// Based on https://www.owasp.org/index.php/Certificate_and_Public_Key_Pinning#.Net
class AcceptAllCertificatesSignedWithASpecificPublicKey : CertificateHandler
{
    // Encoded RSAPublicKey
    private static string PUB_KEY = "3082010A0282010100C0C6A43B14FD8CBD47E0B4C35324080FACC7AA84AB3FEF5C430027129616801AFD5CDF7482A79C578F61C31249E6B41591623A9E54F9557A59517C56A31A03BDD671AC0ECD9A924AFD60E0FCDB42111139E2C60646CC5619F73B38F8E334B0294ADA745F30FB314CCE0E2572FDC03A80CFB88120D7C33E2637688A7512DAD859E506376575797F41B58C1B0F3A74EFF95ED2ABA456291D4FEDFD6E8904DBC653B32757F318636E23524542494867C9103456CF4D772A81D52D43AF4658F30AC64F7DC6A14DEE27622B97538F44EE293F32EE9D6FFB28ED2A78246906DB16366CB2F7EBAB26A7C42BC7887C649AF67092B0DEC47ABD6D44EB8574108FC5CC6BCF0203010001";

    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
        Debug.Log("ValidateCertificate: " + pk);
        if (pk.Equals(PUB_KEY))
            return true;

        // Bad dog
        return false;
    }
}

public class PHPManager : MonoBehaviour
{
    private string secretKey = "mySecretKey"; // Edit this value and make sure it's the same as the one stored on the server
    string addScoreURL = "https://ramjam.co.uk/chantecaille/setPhotoCount2.php?"; //be sure to add a ? to your url
    string highscoreURL = "https://ramjam.co.uk/chantecaille/getPhotoCount.php";
    public string[] animalCountstring;
    public int[] animalCountInt;
    public TextMeshProUGUI[] animalCountText;
    public barGraph bargraph;
    bool gotScores;
    //Text to display the result on
    public Text statusText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(GetScores());
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("posting");
            StartCoroutine(PostScores("Pangolin", int.Parse(animalCountstring[5])+ 1) );
        }
    }

    void Start()
    {
        StartCoroutine(GetScores());
    }

    // remember to use StartCoroutine when calling this function!
    public IEnumerator PostScores(string Animal, int Viewed)
    {
        if (gotScores)
        {
            if (Animal == "Cheetah")
            {
                Viewed += animalCountInt[1];
                animalCountInt[1]++;
            }
            if (Animal == "Elephant") {
                Viewed += animalCountInt[2];
                animalCountInt[2]++;
            }
            if (Animal == "Giraffe") {
                Viewed += animalCountInt[3];
                animalCountInt[3]++;
            }
            if (Animal == "Lion") {
                Viewed += animalCountInt[4];
                animalCountInt[4]++;
            }
            if (Animal == "Pangolin") {
                Viewed += animalCountInt[5];
                animalCountInt[5]++;
            }
            if (Animal == "Rhino") {
                Viewed += animalCountInt[6];
                animalCountInt[6]++;
            }


            if (Viewed > 1)
            {
                //This connects to a server side php script that will add the name and score to a MySQL DB.
                // Supply it with a string representing the players name and the players score.
                string hash = Md5Sum(Animal + Viewed + secretKey);

                string post_url = addScoreURL + "Animals=" + UnityWebRequest.EscapeURL(Animal) + "&Viewed=" + Viewed + "&hash=" + hash;
                //print(post_url);
                // Post the URL to the site and create a download object to get the result.
                UnityWebRequest hs_post = new UnityWebRequest(post_url);
                hs_post.certificateHandler = new AcceptAllCertificatesSignedWithASpecificPublicKey();
                //UnityWebRequest hs_post = UnityWebRequest.Get(post_url);
                yield return hs_post.SendWebRequest(); // Wait until the download is done

                if (hs_post.error != null)
                {
                    //statusText.text = "failed posting score";
                    print("There was an error posting the high score: " + hs_post.error);
                }
                else
                {
                    print("posting score successful");
                }
            }
            
            else
            {
                print("somethingNotRight");
            }
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(GetScores());
    }

    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    IEnumerator GetScores()
    {
       
        UnityWebRequest hs_get = UnityWebRequest.Get(highscoreURL);
        hs_get.certificateHandler = new AcceptAllCertificatesSignedWithASpecificPublicKey();

        yield return hs_get.SendWebRequest();
        
        if (hs_get.isNetworkError)
        {
            print("There was an error getting the high score: " + hs_get.error);
            for (int i = 0; i < animalCountText.Length; i++)
            {
                animalCountText[i].text = "" + animalCountstring[i + 1];
            }
        }
        else
        {
            
            gotScores = true;
            string AnimalNumbers = hs_get.downloadHandler.text.ToString();
            print(AnimalNumbers);
            animalCountstring = AnimalNumbers.Split('|');
            yield return null;
            animalCountInt = new int[animalCountstring.Length];
            for (int i = 0; i < animalCountstring.Length; i++)
            {
                if (animalCountstring[i] == "")
                    animalCountInt[i] = 0;
                else 
                    animalCountInt[i] = int.Parse(animalCountstring[i]);
            }
            //StartCoroutine(GetScores());
            if (animalCountText.Length > 0 && animalCountText != null)
            {
                for (int i = 0; i < animalCountText.Length; i++)
                {
                    animalCountText[i].text = animalCountstring[i + 1];
                }
                
            }
            if (bargraph)
            {
                //fillGraph();
            }
            //statusText.text = hs_get.text; // this is a GUIText that will display the scores in game.
        }
        
    }

    public void fillGraph()
    {
        int[] count = new int[6];
        for (int i = 0; i < count.Length; i++)
        {
            count[i] = int.Parse(animalCountstring[i + 1]);
        }
        bargraph.generateGraph(count);
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}
