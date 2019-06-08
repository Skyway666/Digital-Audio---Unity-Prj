using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLoop : MonoBehaviour
{
    AudioSource source;


    float clip_started = 0;
    float clip_duration = 0;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        clip_duration = 82;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - clip_started >= clip_duration)
        {
            clip_started = Time.time;
            source.time = 6;
            source.Play();
            clip_duration = 76;
        }
    }
}
