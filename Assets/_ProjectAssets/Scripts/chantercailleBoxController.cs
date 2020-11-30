using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chantercailleBoxController : MonoBehaviour
{
    public GameObject shadow;
    public GameObject lid;
    public GameObject image;
    public GameObject[] makeup;
    public GameObject glowRing, glowDisc;
    public bool birdBox;
    bool isOpen;

    private void Start()
    {
        image.GetComponent<Renderer>().material.SetTexture("_MainTex", ChantecailleARManager.Instance.lidImg);
        if (birdBox)
        {
            makeup[0].GetComponent<Renderer>().material = ChantecailleARManager.Instance.boxMaterial;
            makeup[1].GetComponent<Renderer>().material = ChantecailleARManager.Instance.boxMaterial;
        }
        //iTween.MoveBy(gameObject, iTween.Hash("name","hover","y", 0.1f, "easeType", iTween.EaseType.easeInOutSine, "loopType", iTween.LoopType.pingPong));
    }

    public void Drop()
    {
        //iTween.StopByName("hover");
        float parentYPos = this.transform.parent.position.y;
        this.transform.parent = null;
        shadow.transform.parent = null;
        iTween.MoveTo(this.gameObject, iTween.Hash("y", parentYPos, "easeType", iTween.EaseType.easeOutSine, "time", 0.5f));
    }

    public void LidOpen(float time)
    {
        if (isOpen)
        {
            
        }
        else
        {
            iTween.RotateTo(lid, iTween.Hash("x", -140, "time", time, "easetype", iTween.EaseType.easeInOutSine, "isLocal", true));
            iTween.ScaleTo(glowRing, iTween.Hash("delay", 0.5f, "y", 2.25f, "time", time , "easeType", iTween.EaseType.linear, "isLocal", true));
            iTween.ScaleTo(glowDisc, iTween.Hash("delay", 1f, "scale", Vector3.one * 15, "time", time * 2, "easeType", iTween.EaseType.linear, "isLocal", true));
            
        }
        isOpen = !isOpen;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            Drop();
        if (Input.GetKeyDown(KeyCode.Space))
            LidOpen(4f);
    }


}
