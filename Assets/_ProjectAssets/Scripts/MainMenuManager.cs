using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public Button[] buttons;
    public GameObject[] menus;
    public RectTransform slider;
    public int currentMenu = 0;
    bool isSwitching;
    public GameObject introImage;
    public CanvasGroup splashScreen;
    bool splashScreenPressed;
    public GameObject languageScreen;
    // Start is called before the first frame update
    public bool skipLanguage;
    public void loadScene(string animalToLoad)
    {
        StartCoroutine(loadSceneSequence(animalToLoad));
    }

    IEnumerator loadSceneSequence(string animalToLoad)
    {
        if (animalToLoad == "bird")
        {
            ChantecailleARManager.Instance.LoadBirdAR();
        }
        else
        {
            yield return null;
            for (int i = 0; i < ChantecailleARManager.Instance.animals.Length; i++)
            {
                if ("AnimalController" + animalToLoad == ChantecailleARManager.Instance.animals[i].name)
                {
                    GameObject animal = Instantiate(ChantecailleARManager.Instance.animals[i]);
                    yield return null;
                    ChantecailleARManager.Instance.lidImg = animal.GetComponent<animalPackagingDetails>().lidImg;
                    ChantecailleARManager.Instance.makeupColor = animal.GetComponent<animalPackagingDetails>().makeupColor;
                    ChantecailleARManager.Instance.selectedAnimalName = animalToLoad;
                    ChantecailleARManager.Instance.selectedAnimal = ChantecailleARManager.Instance.animals[i];
                    yield return new WaitForSeconds (0.2f);
                    ChantecailleARManager.Instance.LoadAR();
                }
                else
                {

                }
            }
        }
        
    }

    IEnumerator Start()
    {
        print("startingMenu");
        Screen.orientation = ScreenOrientation.Portrait;
        StartCoroutine("introSequence");
        yield return null;
        iTween.ValueTo(gameObject, iTween.Hash("from", slider.anchoredPosition.x, "to", buttons[0].GetComponent<RectTransform>().anchoredPosition.x, "easeType", iTween.EaseType.easeOutSine, "onUpdate", "updateSlider", "time", 0.05f));
        if (skipLanguage)
            selectLanguage("eng");
    }

    IEnumerator introSequence()
    {
        PlayerPrefs.SetString("introSeen", "true");
        if (ChantecailleARManager.Instance.language == "" || ChantecailleARManager.Instance.language == null)
        {
            languageScreen.SetActive(true);
        }
        while (ChantecailleARManager.Instance.language == "" || ChantecailleARManager.Instance.language == null)
        {
            yield return null;
        }
        if (ChantecailleARManager.Instance.introSeen)
        {
            splashScreen.gameObject.SetActive(false);
            introImage.SetActive(false);

            iTween.ValueTo(gameObject, iTween.Hash("from", slider.anchoredPosition.x, "to", buttons[0].GetComponent<RectTransform>().anchoredPosition.x, "easeType", iTween.EaseType.easeOutSine, "onUpdate", "updateSlider", "time", 0.05f));

        }
        else
        {
            if (!PlayerPrefs.HasKey("introSeen"))
            {
                PlayerPrefs.SetString("introSeen", "true");
                ChantecailleARManager.Instance.introSeen = true;
                splashScreen.blocksRaycasts = true;
                yield return new WaitForSeconds(3.5f);
                for (float f = 0; f < 1; f += Time.deltaTime)
                {
                    splashScreen.alpha = f;
                    yield return null;
                }
                splashScreen.alpha = 1;
                introImage.SetActive(false);
                while (!splashScreenPressed)
                    yield return null;

                iTween.ValueTo(gameObject, iTween.Hash("from", slider.anchoredPosition.x, "to", buttons[0].GetComponent<RectTransform>().anchoredPosition.x, "easeType", iTween.EaseType.easeOutSine, "onUpdate", "updateSlider", "time", 0.05f));

                for (float f = 0; f < 1; f += Time.deltaTime)
                {
                    splashScreen.alpha = 1 - f;
                    yield return null;
                }
               
                splashScreen.alpha = 0;
                splashScreen.blocksRaycasts = false;
            }
            else
            {
                splashScreen.gameObject.SetActive(false);
                ChantecailleARManager.Instance.introSeen = true;
                splashScreen.blocksRaycasts = true;
                yield return new WaitForSeconds(3.5f);
                CanvasGroup cg = introImage.GetComponent<CanvasGroup>();

                iTween.ValueTo(gameObject, iTween.Hash("from", slider.anchoredPosition.x, "to", buttons[0].GetComponent<RectTransform>().anchoredPosition.x, "easeType", iTween.EaseType.easeOutSine, "onUpdate", "updateSlider", "time", 0.05f));

                for (float f = 0; f < 1; f += Time.deltaTime)
                {
                    cg.alpha = 1 - f;
                    yield return null;
                }
                
                cg.alpha = 0;
                cg.blocksRaycasts = false;
            }
        }
    }

    public void SplashScreenOK()
    {
        splashScreenPressed = true;
    }

    
    
    public void switchMenu(int menuNo)
    {
        /*
        for (int i = 0; i < menus.Length; i++)  
        {
            if (menuNo == i) menus[i].SetActive(true);
            else menus[i].SetActive(false);
        }
        */
        if (!isSwitching)
        {
            StartCoroutine(fadeMenus(currentMenu, menuNo));
            iTween.ValueTo(gameObject, iTween.Hash("from", slider.anchoredPosition.x, "to", buttons[menuNo].GetComponent<RectTransform>().anchoredPosition.x, "easeType", iTween.EaseType.easeOutSine, "onUpdate", "updateSlider", "time", 0.25f));
        }
    }

    IEnumerator fadeMenus(int prev, int current)
    {
        if (prev != current)
        {
            CanvasGroup prevCanvas = menus[prev].GetComponent<CanvasGroup>();
            CanvasGroup currentCanvas = menus[current].GetComponent<CanvasGroup>();

            menus[current].SetActive(true);
            currentCanvas.alpha = 0;
            isSwitching = true;

            for (float f = 0; f < 1; f += Time.deltaTime / 0.4f)
            {
                prevCanvas.alpha = 1 - f;
                yield return null;
            }
            menus[prev].SetActive(false);

            for (float f = 0; f < 1; f += Time.deltaTime / 0.4f)
            {
                currentCanvas.alpha = f;
                yield return null;
            }
            currentMenu = current;
            isSwitching = false;
        }
    }

    void updateSlider(float xVal)
    {
        slider.anchoredPosition = new Vector2(xVal, -30);
    }

    public void selectLanguage(string language)
    {
        PlayerPrefs.SetString("lang", language);
        ChantecailleARManager.Instance.language = language;
        languageScreen.SetActive(false);
    }

    public void resetLanguage()
    {
        PlayerPrefs.DeleteKey("lang");
        Destroy(ChantecailleARManager.Instance);
        SceneManager.LoadScene("SelectTheme");
    }


   
}
