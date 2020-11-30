using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class ChantecailleARManager : MonoBehaviour
{
    public TMP_FontAsset chineseFont;
    public static ChantecailleARManager Instance { get; private set; }
    public string language = "";
    public bool
        LionUnlocked,
        ElephantUnlocked,
        CheetahUnlocked,
        GiraffeUnlocked,
        PangolinUnlocked,
        RhinoUnlocked;

    public GoogleAnalyticsV4 googleanalytics;
    public GameObject[] animals;
    public string selectedAnimalName;
    public GameObject selectedAnimal;
    public Texture lidImg;
    public Color makeupColor;
    public bool photoTaken = false;
    public bool introSeen = false;
    public Material boxMaterial;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("lang"))
        {
            language = PlayerPrefs.GetString("lang");
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        UnlockAll();
        loadUserProgression();
        GameObject.Find("GAv4").GetComponent<GoogleAnalyticsV4>().StartSession();
    }

    private void Start()
    {
        // Enable line below to enable logging if you are having issues setting up OneSignal. (logLevel, visualLogLevel)
        //OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.INFO, OneSignal.LOG_LEVEL.INFO);

        OneSignal.StartInit("ac670699-cf58-4590-9a17-bf21f4656ab4")
          .HandleNotificationOpened(HandleNotificationOpened)
          .EndInit();

        OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
        OneSignal.permissionObserver += OneSignal_permissionObserver;
        OneSignal.subscriptionObserver += OneSignal_subscriptionObserver;
        OneSignal.emailSubscriptionObserver += OneSignal_emailSubscriptionObserver;
    }

    private void OneSignal_subscriptionObserver(OSSubscriptionStateChanges stateChanges)
    {
        Debug.Log("SUBSCRIPTION stateChanges: " + stateChanges);
        Debug.Log("SUBSCRIPTION stateChanges.to.userId: " + stateChanges.to.userId);
        Debug.Log("SUBSCRIPTION stateChanges.to.subscribed: " + stateChanges.to.subscribed);
    }

    private void OneSignal_permissionObserver(OSPermissionStateChanges stateChanges)
    {
        Debug.Log("PERMISSION stateChanges.from.status: " + stateChanges.from.status);
        Debug.Log("PERMISSION stateChanges.to.status: " + stateChanges.to.status);
    }

    private void OneSignal_emailSubscriptionObserver(OSEmailSubscriptionStateChanges stateChanges)
    {
        Debug.Log("EMAIL stateChanges.from.status: " + stateChanges.from.emailUserId + ", " + stateChanges.from.emailAddress);
        Debug.Log("EMAIL stateChanges.to.status: " + stateChanges.to.emailUserId + ", " + stateChanges.to.emailAddress);
    }

    // Gets called when the player opens the notification.
    private static void HandleNotificationOpened(OSNotificationOpenedResult result)
    {
    }

    void UnlockAll ()
    {
        unlockAnimal("lion");
        unlockAnimal("elephant");
        unlockAnimal("cheetah");
        unlockAnimal("giraffe");
        unlockAnimal("pangolin");
        unlockAnimal("rhino");
    }

    public void unlockAnimal(string animal)
    {
        PlayerPrefs.SetInt(animal, 1);
    }

    public void LoadAR()
    {
        GameObject.Find("GAv4").GetComponent<GoogleAnalyticsV4>().LogEvent("animalSelected", selectedAnimalName, "label", 1);
        SceneManager.LoadScene("AR");
    }
    public void LoadBirdAR()
    {
        GameObject.Find("GAv4").GetComponent<GoogleAnalyticsV4>().LogEvent("animalSelected", "bird", "label", 1);
        SceneManager.LoadScene("ARHummingBird");
    }

    private void loadUserProgression()
    {
        LionUnlocked = PlayerPrefs.GetInt("lion") != 0;
        ElephantUnlocked = PlayerPrefs.GetInt("elephant") != 0;
        CheetahUnlocked = PlayerPrefs.GetInt("cheetah") != 0;
        GiraffeUnlocked = PlayerPrefs.GetInt("giraffe") != 0;
        PangolinUnlocked = PlayerPrefs.GetInt("pangolin") != 0;
        RhinoUnlocked = PlayerPrefs.GetInt("pangolin") != 0;
    }
}
