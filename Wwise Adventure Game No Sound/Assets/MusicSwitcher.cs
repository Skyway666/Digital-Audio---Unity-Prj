using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    public AudioClip forest;
    public AudioClip cave;
    public AudioClip desert;
    public AudioClip volcanic;

    public bool forestTrig;
    public bool caveTrig;
    public bool desertTrig;
    public bool volcanicTrig;

    public enum currentPlace
    {
        forestPlace,
        cavePlace,
        desertPlace,
        volcanicPlace
    };

    currentPlace whereAmI;

    private AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = gameObject.GetComponent<AudioSource>();
        whereAmI = currentPlace.forestPlace;
    }

    // Update is called once per frame
    void Update()
    {
        if (forestTrig && whereAmI != currentPlace.forestPlace)
        {
            forestTrig = false;
            audiosource.clip = forest;
            audiosource.Play();
            whereAmI = currentPlace.forestPlace;
        }

        else if (caveTrig && whereAmI != currentPlace.cavePlace)
        {
            caveTrig = false;
            audiosource.clip = cave;
            audiosource.Play();
            whereAmI = currentPlace.cavePlace;
        }

        else if (desertTrig && whereAmI != currentPlace.desertPlace)
        {
            desertTrig = false;
            audiosource.clip = desert;
            audiosource.Play();
            whereAmI = currentPlace.desertPlace;
        }

        else if (volcanicTrig && whereAmI != currentPlace.volcanicPlace)
        {
            volcanicTrig = false;
            audiosource.clip = volcanic;
            audiosource.Play();
            whereAmI = currentPlace.volcanicPlace;
        }
    }
}
