using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public SpriteRenderer screenFader;
    public bool isFaderOn = false;
    public void OpenFader()
    {
        isFaderOn = true;
        screenFader.enabled = true;
    }

    public void CloseFader()
    {
        isFaderOn = false;
        screenFader.enabled = false;
    }

}
