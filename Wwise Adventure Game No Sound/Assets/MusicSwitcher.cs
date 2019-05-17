using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    public AudioClip forest;
    public AudioClip cave;
    public AudioClip desert;
    public AudioClip volcanic;

    public bool forestBool;
    public bool caveBool;
    public bool desertBool;
    public bool volcanicBool;

    private AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
