using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//Raul Rojas 
//CSE 1302
//Project
//File Created: 7-11-2017

public class TitleControl : MonoBehaviour
{
    
    public Button Play;
    public Button HowToPlay;
    public Button HighScores;
    public Button Options;
    public Button Credits;
    public Button Exit;

    private void Start()
    {
        Play.onClick.AddListener(pressedPlay);
        HowToPlay.onClick.AddListener(pressedHowToPlay);
        HighScores.onClick.AddListener(pressedHighScores);
        Options.onClick.AddListener(pressedOptions);
        Credits.onClick.AddListener(pressedCredits);
        Exit.onClick.AddListener(pressedExit);
        if (PlayerPrefs.GetInt(GameStructs.PrefKeys.MusicOn) == 1)
        {
            if (!AudioManager.instance.isPlaying(GameStructs.ClipName.TitleMusic))
            {
                AudioManager.instance.Play(GameStructs.ClipName.TitleMusic);
            }
        }
    }

    void pressedPlay()
    {
        //Debug.Log("You Pressed Play Button.");
        AudioManager.instance.StopAll();
        AudioManager.instance.Play(GameStructs.ClipName.ButtonClick);
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);        
    }

    void pressedHowToPlay()
    {
        //Debug.Log("You Pressed How to Play Button. ");
        AudioManager.instance.Play(GameStructs.ClipName.ButtonClick);
        SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
    }

    void pressedHighScores()
    {
        //Debug.Log("You Pressed HighScores Button. ");
        AudioManager.instance.Play(GameStructs.ClipName.ButtonClick);
        SceneManager.LoadScene("HighScores", LoadSceneMode.Single);
    }

    void pressedOptions()
    {
        //Debug.Log("You Pressed Options Button. ");
        AudioManager.instance.Play(GameStructs.ClipName.ButtonClick);
        SceneManager.LoadScene("Options", LoadSceneMode.Single);
    }

    void pressedCredits()
    {
        AudioManager.instance.Play(GameStructs.ClipName.ButtonClick);
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    void pressedExit()
    {
        AudioManager.instance.StopAll();
        AudioManager.instance.Play(GameStructs.ClipName.ButtonClick);
        Application.Quit();
    }

}
