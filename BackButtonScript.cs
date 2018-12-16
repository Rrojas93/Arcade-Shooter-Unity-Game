using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButtonScript : MonoBehaviour
{
    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(pressedBackButton);
    }
    void pressedBackButton()
    {
        AudioManager.instance.Play(GameStructs.ClipName.ButtonClick);
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }
}
