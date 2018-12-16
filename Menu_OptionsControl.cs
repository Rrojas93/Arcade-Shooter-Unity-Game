using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_OptionsControl : MonoBehaviour
{
    public Toggle musicToggle;

    private bool isMusicOn;

    void Start()
    {
        if (PlayerPrefs.GetInt(GameStructs.PrefKeys.MusicOn) == 1)
        {
            musicToggle.isOn = true;
            isMusicOn = true;
        }
        else
        {
            musicToggle.isOn = false;
            isMusicOn = false;
        }
    }

    void Update()
    {
        if (musicToggle.isOn)
        {
            if (isMusicOn == false)
            {
                AudioManager.instance.Play(GameStructs.ClipName.ButtonClick);
                isMusicOn = true;
                PlayerPrefs.SetInt(GameStructs.PrefKeys.MusicOn, 1);
                AudioManager.instance.Play(GameStructs.ClipName.TitleMusic); 
            }
        }
        else
        {
            if (isMusicOn == true)
            {
                AudioManager.instance.StopAll();
                AudioManager.instance.Play(GameStructs.ClipName.ButtonClick);
                isMusicOn = false;
                PlayerPrefs.SetInt(GameStructs.PrefKeys.MusicOn, 0);
                //stop music if any 
                
            }
        }
    }
}
