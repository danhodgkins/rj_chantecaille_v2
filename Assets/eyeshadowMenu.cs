using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class eyeshadowMenu : MonoBehaviour
{
    public Image eyeshadowImage;
    public GameObject[] nextBackButtons;
    int currentImage;
    Sprite[] SelectedSprites;
    public Sprite[]  CheetahSprites, ElephantSprites,  GiraffeSprites, LionSprites, PangolinSprites, RhinoSprites;
    CanvasGroup cg;
    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        hideMenu();
    }

    public void setupMenu(string animalSelected)
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        if (animalSelected == "cheetah") SelectedSprites = CheetahSprites;
        if (animalSelected == "elephant") SelectedSprites = ElephantSprites;
        if (animalSelected == "giraffe") SelectedSprites = GiraffeSprites;
        if (animalSelected == "lion") SelectedSprites = LionSprites;
        if (animalSelected == "pangolin") SelectedSprites = PangolinSprites;
        if (animalSelected == "rhino") SelectedSprites = RhinoSprites;

        for (int i = 0; i < nextBackButtons.Length; i++)
        {
            nextBackButtons[i].SetActive(SelectedSprites.Length > 1 ? true : false);
        }

        currentImage = 0;
        eyeshadowImage.sprite = SelectedSprites[0];
        
    }

    public void hideMenu()
    {
        cg.alpha = 0;
        cg.blocksRaycasts = false;
    }

    public void scrollImages(int val)
    {
        currentImage += val;
        eyeshadowImage.sprite = SelectedSprites[currentImage %  SelectedSprites.Length];
    }
}
