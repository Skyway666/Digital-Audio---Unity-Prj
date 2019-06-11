using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public string zoneEntered;
    MusicSwitcher cameraRefScript;
    // Start is called before the first frame update
    void Start()
    {
        cameraRefScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MusicSwitcher>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (zoneEntered == "forest")
            {
                cameraRefScript.forestTrig = true;
            }

            else if (zoneEntered == "cave")
            {
                cameraRefScript.caveTrig = true;
            }

            else if (zoneEntered == "desert")
            {
                cameraRefScript.desertTrig = true;
            }

            else if (zoneEntered == "volcanic")
            {
                cameraRefScript.volcanicTrig = true;
            }
        }
    }
}