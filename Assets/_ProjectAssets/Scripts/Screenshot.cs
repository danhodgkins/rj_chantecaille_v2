using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
public class Screenshot : MonoBehaviour
{
    public fadeController cameraFlash;
    public GameObject ui;
    PHPManager php;
    public GameObject afterPhoto;
    public GameObject photoFrame;
    // Start is called before the first frame update

    private void Start()
    {
        php = GetComponent<PHPManager>();
        photoFrame = GameObject.Find("PhotoFrame");
        photoFrame.SetActive(false);
    }

    public void takeScreenshot()
    {
        StartCoroutine("screenshot");
    }

    IEnumerator screenshot()
    {
        Scene scene = SceneManager.GetActiveScene();
        yield return null;
        if (scene.name == "AR" || scene.name == "ARHummingBird")  
            GameObject.Find("GAv4").GetComponent<analyticsManager>().PhotoTaken();
        else 
            GameObject.Find("GAv4").GetComponent<analyticsManager>().SelfieTaken();

        ui.SetActive(false);

        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }

        photoFrame.SetActive(true);
       
        yield return null;

        string imgFileName = 
            "chantecaille_"
            + System.DateTime.Now.Year
            + System.DateTime.Now.Month.ToString("00")
            + System.DateTime.Now.Day.ToString("00")
            + System.DateTime.Now.Hour.ToString("00")
            + System.DateTime.Now.Minute.ToString("00")
            + System.DateTime.Now.Second.ToString("00")
            + ".png";

        yield return new WaitForEndOfFrame();
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        //save screenshot to gallery
        Debug.Log("permission result:" + NativeGallery.SaveImageToGallery(ss, "Chantecaille", "chantecaille_{0}.png"));

        Destroy(ss);
        
        yield return new WaitForSeconds(0.1f);
        photoFrame.SetActive(false);
        //StartCoroutine(CameraFlash());
        yield return new WaitForSeconds(0.1f);
        if (ChantecailleARManager.Instance.selectedAnimal != null)
            StartCoroutine(php.PostScores(ChantecailleARManager.Instance.lidImg.name, 1));
        ui.SetActive(true);
        yield return new WaitForSeconds(1f);
        showAfterPhoto();
    }

    private void showAfterPhoto()
    {
        if (!ChantecailleARManager.Instance.photoTaken)
        {
            ChantecailleARManager.Instance.photoTaken = true;
            afterPhoto.SetActive(true);
        }
    }

    IEnumerator CameraFlash()
    {
        if (cameraFlash)
        {
            yield return StartCoroutine(cameraFlash.fadeIn(0.2f));
            yield return StartCoroutine(cameraFlash.fadeOut(1f));
        }
    }
}
