using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;
    float scaleVal;
    public RawImage background;
    public AspectRatioFitter fit;
    public Text txt;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        defaultBackground = background.texture;

        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("no camera detected");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("unable to find front camera");
            return;
        }

        backCam.Play();
        background.texture = backCam;

        camAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!camAvailable)
            return;
        float ratio;
        
        ratio = (float)backCam.width / (float)backCam.height;
        
        fit.aspectRatio = ratio;
        scaleVal = 1;
        float scaley = backCam.videoVerticallyMirrored ? -1f: 1f;
        int orient = -backCam.videoRotationAngle;

        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown || Input.deviceOrientation == DeviceOrientation.FaceUp || Input.deviceOrientation == DeviceOrientation.FaceDown)
        {
            orient += 180;
            scaleVal = 0.6f;
            //scaley = 1;
        }
        
        background.rectTransform.localScale = scaleVal * new Vector3(-1f, scaley, 1f);
        txt.text = orient.ToString();
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}
