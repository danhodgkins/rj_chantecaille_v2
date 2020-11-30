using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{
    public string 
        Cheetah, 
        Elephant, 
        Giraffe, 
        Lion, 
        Pangolin, 
        Rhino;
    RawImage rawImage;
    VideoPlayer videoPlayer;
    AudioSource audioSource;
    CanvasGroup cg;
    public GameObject logo;
    public Texture bgStrip;
    public Sprite playImg, pauseImg;
    public Image playPauseButton;

    bool Paused;

    // Start is called before the first frame update
    void Start()
    {
        SetupComponents();
        cg.interactable = false;
    }

    public void StartVideo(String SelectedAnimal)
    {
        StartCoroutine(PlayVideo(SelectedAnimal));
    }

    private void SetupComponents()
    {
        rawImage = GetComponentInChildren<RawImage>();
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        audioSource = GetComponentInChildren<AudioSource>();
        cg = GetComponent<CanvasGroup>();
    }

    IEnumerator PlayVideo(String SelectedAnimal)
    {
        playPauseButton.sprite = pauseImg;

        rawImage.texture = bgStrip;
        logo.SetActive(true);
        videoPlayer.url = GetURL(SelectedAnimal);
        yield return null;
        videoPlayer.Prepare();
        yield return null;
        StartCoroutine(fadeVideo(true));
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.1f);
        while (!videoPlayer.isPrepared)
        {
            yield return null;
            //break;
        }
        logo.SetActive(false);
        
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
        yield return waitForSeconds;
        while (videoPlayer.isPlaying || Paused)
        {
            yield return null;
        }
        print("videoDone");
        StartCoroutine(fadeVideo(false));
    }

    IEnumerator fadeVideo(bool fade)
    {
        float fadeMultiplier = 2;
        if (fade)
        {
            cg.blocksRaycasts = true;
            cg.alpha = 0;
            for (float f = 0; f < 1; f += fadeMultiplier * Time.deltaTime)
            {
                cg.alpha = f;
                yield return null;
            }
            cg.interactable = true;
            cg.alpha = 1;
        }
        else
        {
            cg.blocksRaycasts = false;
            cg.interactable = false;
            cg.alpha = 1;
            for (float f = 0; f < 1; f += fadeMultiplier * Time.deltaTime)
            {
                cg.alpha = 1-f;
                yield return null;
            }
            cg.alpha = 0;
        }
    }

    public void PlayPause()
    {
        if (videoPlayer.isPlaying)
        {
            playPauseButton.sprite = playImg;
            Paused = true;
            videoPlayer.Pause();
        }
        else
        {
            playPauseButton.sprite = pauseImg;
            Paused = false;
            videoPlayer.Play();
        }
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
        StartCoroutine(fadeVideo(false));
    }

    public string GetURL(string Animal)
    {
        switch (Animal)
        {
            case "Cheetah":
                return Cheetah;
            case "Elephant":
                return Elephant;
            case "Giraffe":
                return Giraffe;
            case "Lion":
                return Lion;
            case "Pangolin":
                return Pangolin;
            case "Rhino":
                return Rhino;
            default:
                return Cheetah;
        }
    }


}
