using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class debugText : MonoBehaviour
{
    Text text;
    public Transform cam, sessionOrigin, session;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "cam :" + Camera.main.transform.position.ToString();// + " \nsessionOrigin: " + sessionOrigin.position.ToString() + " \nsession: " + session.position.ToString();
    }
}
