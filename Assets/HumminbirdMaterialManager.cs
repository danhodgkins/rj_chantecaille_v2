using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumminbirdMaterialManager : MonoBehaviour
{
    public bool initialBird;

    public Material[] birdBody;
    public Material[] birdEyeBeak;
    public SkinnedMeshRenderer body;
    public MeshRenderer[] tailRenderers;
    public MeshRenderer[] eyesBeakRenderers;

    private void Start()
    {
        if (initialBird)
        {
            if (ChantecailleARManager.Instance.selectedAnimalName == "blackthroatedmango")
                SwitchBirdMaterial(0);
            else if (ChantecailleARManager.Instance.selectedAnimalName == "rufous")
                SwitchBirdMaterial(1);
        }
    }

    //Switches material based on bird type
    //birdtype 0 = BlackThroatedMango, type 1 = Rufous
    public void SwitchBirdMaterial(int birdType)
    {
        print("spawn bird: " + birdType);
        body.material = birdBody[birdType];
        for (int i = 0; i < tailRenderers.Length; i++)
            tailRenderers[i].material = birdBody[birdType];
        for (int i = 0; i < eyesBeakRenderers.Length; i++)
            eyesBeakRenderers[i].material = birdEyeBeak[birdType];
    }
}
