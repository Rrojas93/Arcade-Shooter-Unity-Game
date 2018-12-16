using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Author: Raul Rojas
//CSE 1302
//Project One
//7-13-2017

public class PlayerUI : MonoBehaviour
{
    #region Fields
    public Slider healthBar;
    public Text healthText;
    public Text weaponInfoText;
    public Text waveInfoText;
    public Slider reloadSlider;
    public Text scoreText;
    public Text finalScoreText;
    public GameObject gameOverPanel;

    PlayerController player;
    GameController gameControl;
    #endregion

    #region MonoBehaviours

    private void Start()
    {
        player = this.gameObject.GetComponent<PlayerController>();
        gameControl = GameObject.Find("GameController").GetComponent<GameController>();
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (player.IsAlive)
        {
            updateUI();
        }
        else
        {
            updateHealth();
            gameOverGUI();
        }
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    void updateUI()
    {
        updateHealth();
        updateWeaponInfo();
        updateWaveInfo();
        updateReloadSlider();
        updateScoreInfo();
    }
    
    void updateHealth()
    {
        if (player.IsAlive)
        {
            healthBar.value = player.Health;
            healthText.text = player.Health + "/100";
        }
        else
        {
            healthBar.value = 0;
            healthText.text = "You're Dead.";
        }
    }

    void updateWeaponInfo()
    {
        if (player.Inventory.Count > 0)
        {
            GunBase temp = player.Inventory[player.CurrentWeapon];

            if (!temp.IsReloading)
            {
                weaponInfoText.text = temp.gunName + ": " + temp.ClipAmmo + "/" + temp.Ammo;
            }
        }
        else
            weaponInfoText.text = "";
    }

    void updateWaveInfo()
    {
        string waveInfo;
        if (gameControl.IsWaveOn)
        {
            waveInfo = "Wave: " + gameControl.Wave + "\nEnemies Left: " + GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
        else
        {
            waveInfo = "Wave: " + gameControl.Wave + "\nTime To Next Wave: " + gameControl.TimeToNextWave;
        }

        waveInfoText.text = waveInfo;
    }

    void updateReloadSlider()
    {
        if (player.Inventory.Count > 0)
        {
            if (player.Inventory[player.CurrentWeapon].IsReloading)
            {
                reloadSlider.gameObject.SetActive(true);
                float timeToFinish = 1 - ((player.Inventory[player.CurrentWeapon].ReloadFinishTime - Time.time) / player.Inventory[player.CurrentWeapon].reloadTime);
                reloadSlider.value = timeToFinish;
            }
            else
            {
                reloadSlider.gameObject.SetActive(false);
            }

        }
    }

    void updateScoreInfo()
    {
        int total = gameControl.TotalScore;
        int killScore = (int)gameControl.EnemyKillScore;

        scoreText.text ="Current Wave: " + killScore + "\nTotal: " + total;
    } 

    void gameOverGUI()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Total Score: " + gameControl.TotalScore;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.StopAll();
            PlayerPrefs.SetInt(GameStructs.PrefKeys.PlayerScore, gameControl.TotalScore);
            PlayerPrefs.SetInt(GameStructs.PrefKeys.FromGameScene, 1);
            SceneManager.LoadScene("HighScores");
        }

    }
    #endregion
}
