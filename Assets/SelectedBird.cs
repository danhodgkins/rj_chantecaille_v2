using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedBird   : MonoBehaviour
{
    public Texture[] birdLidImg;
    public Material[] compactBoxColor;
    // Start is called before the first frame update
    public void selectBird(int selectedBird)
    {
        if (selectedBird == 0)
            ChantecailleARManager.Instance.selectedAnimalName = "blackthroatedmango";
        else
            ChantecailleARManager.Instance.selectedAnimalName = "rufous";
        ChantecailleARManager.Instance.lidImg = birdLidImg[selectedBird];
        ChantecailleARManager.Instance.boxMaterial = compactBoxColor[selectedBird];
    }
}
