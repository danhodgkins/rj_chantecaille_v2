using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource AS;
    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    public void fadeAudio(bool on)
    {
        AS.Play();
        StartCoroutine(fadeAudioSequence(on));
    }

    IEnumerator fadeAudioSequence(bool on)
    {
        AS.volume = on ? 0 : 1;
        for (float f = 0; f < 1; f+= Time.deltaTime / 4)
        {
            AS.volume = on ? f: 1 - f;
            yield return null;
        }
        AS.volume = on ? 1 : 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) fadeAudio(true);
    }
}
