using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneManager : MonoBehaviour
{
    public bool DontDestroyARManager;
    private void Start()
    {
        Destroy(GameObject.Find("GAv4"));

        if (!DontDestroyARManager)
            Destroy(GameObject.Find("ChantecailleARManager"));
    }

    public void loadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
