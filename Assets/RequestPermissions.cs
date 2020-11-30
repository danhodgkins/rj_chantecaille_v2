using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

#if UNITY_IOS
using UnityEngine.iOS;
#endif



public class RequestPermissions : MonoBehaviour
{

    // Start is called before the first frame update
    IEnumerator Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        yield return new WaitForEndOfFrame();
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        yield return new WaitForEndOfFrame();
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        
#endif
        yield return null;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
