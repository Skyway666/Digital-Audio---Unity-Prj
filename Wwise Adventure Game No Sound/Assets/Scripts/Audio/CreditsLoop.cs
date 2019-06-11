using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsLoop : MonoBehaviour
{
    AudioSource source;

    float clip_started = 0;
    float clip_duration = 0;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        clip_duration = 16f;
        clip_started = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - clip_started >= clip_duration && clip_duration > 0)
        {
            clip_started = Time.time;
            source.Play();
        }
        if (Input.GetKey(KeyCode.Escape))
            clip_duration = -1;
        if (clip_duration == -1)
        {
            source.volume -= 0.0075f;
        }

    }
}
