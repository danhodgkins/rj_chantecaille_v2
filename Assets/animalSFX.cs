using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animalSFX : MonoBehaviour
{
    public float timer;
    public AudioClip sfx;
    AudioSource AS;
    animalController AC;

    bool timerEnabled = true;

    void Start()
    {
        AC = GetComponentInParent<animalController>();
        AS = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (AC.lerpSpeed < 0.1f)
        {
            
            timer += Time.deltaTime;
        }
        else
        {
            if (AS.isPlaying)
            {
                AS.Stop();
            }
            timer = 0;
        }
    }

    public void PlaySFX()
    {
        if (timerEnabled)
        {
            if (timer > 5)
                AS.PlayOneShot(sfx);
        }else
        {
            AS.PlayOneShot(sfx);
        }
    }
}
