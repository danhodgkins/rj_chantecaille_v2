using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if PLATFORM_IOS
using UnityEngine.iOS;
using UnityEngine.Apple.ReplayKit;
#endif
#if PLATFORM_ANDROID
using UnityEngine.Android;
using Recorder;
#endif


public class RecordingManager : MonoBehaviour
{
    bool permissionsAllowed;
    public bool recording;
    public GameObject preparingImage;
    public GameObject recordButton;
    public GameObject stopButton;
    public GameObject requestMenu;
    public GameObject permissionMic, permissionStorageWrite, permissionStorageRead, permissionCamera;
#if UNITY_ANDROID
    public RecordManager recordManager;
    
#endif
    public GameObject[] hideUIItems;
    private void Start()
    {
#if UNITY_IOS
            if (!ReplayKit.APIAvailable){
                print ("no replay kit");
            }
#endif
#if UNITY_ANDROID
        if (!recordManager)
            recordManager = GameObject.Find("ScreenRecorder").GetComponent<RecordManager>();
        checkPermissions();        
#endif
    }

    private void Update()
    {   
#if UNITY_ANDROID
        if (!permissionsAllowed && requestMenu.activeSelf)
            checkPermissions();
#endif
    }

    private void checkPermissions()
    {
#if UNITY_ANDROID
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            permissionCamera.SetActive(false);
        }
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            permissionStorageWrite.SetActive(false);
        }
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            permissionStorageRead.SetActive(false);
        }
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            permissionMic.SetActive(false);
        }
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone) && Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead ) && Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            permissionsAllowed = true;
            requestMenu.SetActive(false);
        }
#endif

    }

    public void startRecording()
    {
        if (!recording)
        {
#if UNITY_IOS
            recordButton.SetActive(false);
            recording = true;
            iosStartRecording();
            stopButton.SetActive(true);
#endif
#if UNITY_ANDROID
            if (permissionsAllowed)
            {
                recordButton.SetActive(false);
                recording = true;
                androidStartRecording();
                stopButton.SetActive(true);
            }
            else
            {
                requestMenu.SetActive(true);
            }
#endif
        }
        if (recording)
            hideUI(false);
    }

    void hideUI(bool hide)
    {
        for (int i = 0; i < hideUIItems.Length; i++)
        {
            hideUIItems[i].SetActive(hide);
        }
    }

    public void setupForRecord()
    {

    }

    public void SaveVideo()
    {
        if (recording)
        {
            stopButton.SetActive(false);
            hideUI(true);
            recordButton.SetActive(true);
            recording = false;
#if UNITY_IOS
                StartCoroutine(iosStopRecording());
#endif
#if UNITY_ANDROID
                androidStopRecording();
#endif
            
            
        }
    }

#if UNITY_IOS
    private void iosStartRecording()
    {
        ReplayKit.StartRecording();
    }

    private IEnumerator iosStopRecording()
    {
        ReplayKit.StopRecording();
        preparingImage.SetActive(true);
        yield return new WaitForSeconds (2f);
        preparingImage.SetActive(false);
        ReplayKit.Preview();
    }
#endif

#if UNITY_ANDROID
    private void androidStartRecording()
    {
        recordManager.StartRecord();        
    }

    private void androidStopRecording()
    {
        recordManager.StopRecord();
    }
    //androidRequests---------------------------------------------------
    public void requestMic()
    {
        Permission.RequestUserPermission(Permission.Microphone);
    }
    public void requestCam()
    {
        Permission.RequestUserPermission(Permission.Camera);
    }
    public void requestStorage()
    {
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
    }
    public void requestreadStorage()
    {
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
    }
    //--------------------------------------------------------------------
#endif
     

}
