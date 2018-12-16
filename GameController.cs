using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Raul Rojas
//CSE 1302
//Project
//7-4-2017

public class GameController : MonoBehaviour
{
    #region Fields

    #region Public
    //Grab Prefabs and/or Objects from unity inspector. 
    public PlayerController player;
    public Enemy_Base zombiePref;
    public Enemy_Base dynoRollsPref;
    public Enemy_Base necroPref;
    public GunBase pistolPref;
    public GunBase smgPref;
    public GunBase shotgunPref;

    //Inspector editable Variables
    public int waveDifficultyScalar = 1;
    public float waveRestTime = 5;
    public float weaponSpawnRate = 5;
    public float weaponScaleFactor = 1;
    #endregion

    #region Private
    private Enemy_Base[] enemyPrefs;
    private GunBase[] weaponPrefs;
    private GameObject[] spawnPoints;
    private GameObject[] weaponSpawnPoints;
    private int wave;
    private int perWaveEnemyCount;
    private System.Random r;
    private float waveEndTime;
    private bool isWaveOn;  //is the player currently battling a wave. if not (false) player is in rest state).
    private int timeToNextWave;
    private float lastWeaponSpawnTime;
    private int weaponCount;
    private int maxWeaponCount;
    private float waveScore;
    private float enemyKillScore;
    private int totalScore;
    private float accuracyFactor;
    private float timeBonus;
    private float waveStartTime;
    private int shotsFired;
    private int shotsLanded;
    private StreamReader sr;
    private GameObject[] spawnLights;
    #endregion

    #endregion

    #region Properties

    public bool IsWaveOn
    {
        get { return this.isWaveOn; }
    }

    public int Wave
    {
        get { return this.wave; }
    }

    public int TimeToNextWave
    {
        get { return this.timeToNextWave; }
    }

    public int ShotsLanded
    {
        get { return this.shotsLanded; }
        set { this.shotsLanded = value; }
    }

    public int ShotsFired
    {
        get { return this.shotsFired; }
        set { this.shotsFired = value; }
    }

    public float EnemyKillScore
    {
        get { return this.enemyKillScore; }
        set { this.enemyKillScore = value; }
    }

    public int TotalScore
    {
        get { return this.totalScore; }
    }

    #endregion

    #region MonoBehaviours

    private void Start()
    {
        if (PlayerPrefs.GetInt(GameStructs.PrefKeys.MusicOn) == 1)
        {
            AudioManager.instance.Play(GameStructs.ClipName.GameMusic);
        }
        r = new System.Random();
        getResources();
        setSpawns();    //get player and weapon spawn positions from file. 
        wave = 0;
        perWaveEnemyCount = 0;
        waveEndTime = Time.time;
        isWaveOn = true;
        lastWeaponSpawnTime = Time.time - weaponSpawnRate;
        weaponCount = 0;
        maxWeaponCount = GameObject.FindGameObjectsWithTag("Weapon Spawn Point").Length;
        waveScore = 0;
        enemyKillScore = 0;
        totalScore = 0;
        accuracyFactor = 1;
        timeBonus = 0;
        shotsFired = 0;
        shotsLanded = 0;
        
    }

    private void Update()
    {
        if (player.IsAlive)
        {
            waveControl();  //in charge of game wave control 
            weaponControl();    //in charge of weapons in world space (not held by player)
        }
    }

    #endregion

    #region Private Methods

    private void weaponControl()
    {
        GameObject[] liveWeapons = GameObject.FindGameObjectsWithTag("GunPickUp");
        weaponCount = liveWeapons.Length;
        spawn();
        rotate(liveWeapons);
        pickUp(liveWeapons);
    }

    private void pickUp(GameObject[] liveWeapons)
    {
        foreach (GameObject gun in liveWeapons)
        {
            if ((Vector3.Distance(gun.transform.position, player.gameObject.transform.position) < 3))
            {
                player.addToInventory(gun.GetComponent<GunBase>());
                //Destroy(gun);
                break;
            }
        }
    }

    private void rotate(GameObject[] liveWeapons)
    {
        if (liveWeapons.Length == 0)
        {
            return;
        }
        else
            foreach (GameObject weapon in liveWeapons)
            {
                weapon.GetComponent<GunBase>().rotate();
            }
    }

    private void spawn()
    {
        if (((Time.time - lastWeaponSpawnTime) >= weaponSpawnRate) && weaponCount < maxWeaponCount)
        {
            //Debug.Log("spawned weapon");
            int randomLoc = 0;
            GameObject spawn = null;
            for (int i = 0; i < weaponSpawnPoints.Length; i++)
            {
                randomLoc = r.Next(maxWeaponCount);
                if (weaponSpawnPoints[randomLoc].transform.childCount <= 1)
                {
                    spawn = weaponSpawnPoints[randomLoc];
                    break;
                }
            }
            //do
            //{
            //    randomLoc = r.Next(maxWeaponCount);
            //    if (weaponSpawnPoints[randomLoc].transform.childCount <= 1)
            //    {
            //        spawn = weaponSpawnPoints[randomLoc];
            //    }
            //    else
            //        spawn = null;                
            //} while (spawn == null);
            if (spawn != null)
            {
                spawnLights[randomLoc].SetActive(true);
                GameObject tempObj = Instantiate(weaponPrefs[r.Next(3)].gameObject, spawn.transform.position, Quaternion.identity);
                tempObj.tag = "GunPickUp";
                tempObj.transform.SetParent(spawn.transform);
                tempObj.transform.localScale = new Vector3(weaponScaleFactor, weaponScaleFactor, weaponScaleFactor);
                weaponCount++;
                lastWeaponSpawnTime = Time.time;
            }
        }
    }

    void waveControl()
    {
        if (isWaveOn)   //if currently battling a wave
        {
            
            if (isWaveClear())  //if the player has finished the wave
            {
                //Debug.Log("Wave " + wave + " has been cleared.");
                waveEndTime = Time.time;    //store time of wave finish
                isWaveOn = false;   //switch state
                if(wave>0)
                    addToScore();
            }
            else
                enemyControl(); //in charge of enemy attacks and movement
        }
        else  //player is currently resting 
        {
            timeToNextWave = (int)((waveEndTime + waveRestTime) - Time.time);
            //if(not the same frame as wave finished frame AND appropriate time elapsed)
            if (Time.time != waveEndTime && (Time.time - waveEndTime) >= waveRestTime)
            {
                wave++; //increment wave count to next wave
                perWaveEnemyCount = wave * waveDifficultyScalar;  //per wave difficulty formula.
                isWaveOn = true;
                //Debug.Log("Beginning Next Wave: " + wave);
                Enemy_Necro.resetCloneCount();
                waveStartTime = Time.time;
                shotsLanded = 0;
                ShotsFired = 0;
            }
        }

        waveSpawning(); 

    }

    void enemyControl()
    {
        GameObject[] aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            
        if (player.IsAlive)
        {
            foreach(GameObject e in aliveEnemies)
            {
                //Debug.Log(e.name);
                e.GetComponent<Enemy_Base>().move();
            }
        }
        List<Enemy_Necro> liveNecTypes = new List<Enemy_Necro>();

        foreach(GameObject en in aliveEnemies)
        {
            if (en.GetComponent<Enemy_Base>() is Enemy_Necro)
            {
                liveNecTypes.Add(en.GetComponent<Enemy_Necro>());
            }
        }

        if (liveNecTypes.Count > 0)
        {
            necroCloneControl(liveNecTypes);
        }

        for (int i = 0; i < aliveEnemies.Length; i++)
        {
            if (Vector3.Distance(player.gameObject.transform.position, aliveEnemies[i].transform.position) <= 3)
            {
                aliveEnemies[i].GetComponent<Enemy_Base>().attack();
            }
        }
        
    }

    private void necroCloneControl(List<Enemy_Necro> necList)
    {
        foreach(Enemy_Necro necro in necList)
        {
            necro.cloneCheck();
        }
    }

    void waveSpawning()
    {
        if (perWaveEnemyCount > 0)
        {
            List<GameObject> availableSpawnPoints = new List<GameObject>();
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].transform.childCount>0)
                {
                    if (Vector3.Distance(spawnPoints[i].transform.position, spawnPoints[i].transform.GetChild(0).position) > 3)
                    {
                        spawnPoints[i].transform.DetachChildren();
                    }
                }
                if (spawnPoints[i].transform.childCount == 0)
                {
                    availableSpawnPoints.Add(spawnPoints[i]);
                }
            }

            while ((availableSpawnPoints.Count > 0) && (perWaveEnemyCount > 0))
            {
                GameObject newEnemy = enemyPrefs[r.Next(3)].gameObject;
                //GameObject newEnemy = necroPref.gameObject;                 //DEBUGGING FIX HERE

                int selectSpawn = r.Next(availableSpawnPoints.Count);
                newEnemy = Instantiate(newEnemy, availableSpawnPoints[selectSpawn].transform.position, Quaternion.identity);
                newEnemy.transform.parent = availableSpawnPoints[selectSpawn].transform;
                availableSpawnPoints.RemoveAt(selectSpawn);
                perWaveEnemyCount--;
            }
        }
    }

    bool isWaveClear()
    {
        GameObject[] aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (aliveEnemies.Length == 0 && perWaveEnemyCount <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void getResources()
    {
        enemyPrefs = new Enemy_Base[3];
        enemyPrefs[0] = zombiePref;
        enemyPrefs[1] = dynoRollsPref;
        enemyPrefs[2] = necroPref;

        weaponPrefs = new GunBase[3];
        weaponPrefs[0] = pistolPref;
        weaponPrefs[1] = smgPref;
        weaponPrefs[2] = shotgunPref;

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
        weaponSpawnPoints = GameObject.FindGameObjectsWithTag("Weapon Spawn Point");

        spawnLights = new GameObject[weaponSpawnPoints.Length];
        for (int i = 0; i < weaponSpawnPoints.Length; i++)
        {
            spawnLights[i] = weaponSpawnPoints[i].transform.GetChild(0).gameObject;
        }
        foreach(GameObject light in spawnLights)
        {
            light.SetActive(false);
        }
    }

    void addToScore()
    {
        accuracyFactor = ((float)shotsLanded / shotsFired);
        timeBonus = (float)(1 / (Time.time - waveStartTime));
        waveScore = ((wave * 10) * accuracyFactor * (1 + timeBonus));
        totalScore += (int)(waveScore + enemyKillScore);
        Debug.Log("TotalScore: " + totalScore + "\nWave: " + wave + "\nAccuracy: " + accuracyFactor + "\nTimeBonus: " + timeBonus + "\nWaveScore: " + waveScore + "\nEnemyKillScore: " + enemyKillScore);
        enemyKillScore = 0;        
    }

    void setSpawns()
    {
        bool complete = false;
        int backUpCount = 0;
        do
        {
            backUpCount++;
            if (backUpCount >= 2)   //couldnt find a file, attempted to make the file but failed to complete anyway.
            {
                break;
            }
            try
            {
                sr = new StreamReader("SpawnPoints.txt");
                string temp = sr.ReadLine();
                if(temp == null)
                {
                    Debug.Log("temp started at null. (GameController.cs/setSpawns())");
                }
                string[] lineData;
                char[] delims = { '_', ':', ',' };

                while (temp != null)
                {
                    lineData = temp.Split(delims);
                    if (lineData[0] == "Player")
                    {
                        player.transform.position = new Vector3(float.Parse(lineData[1]), float.Parse(lineData[2]), float.Parse(lineData[3]));
                    }
                    else
                    {
                        weaponSpawnPoints[int.Parse(lineData[1])].transform.localPosition = new Vector3(float.Parse(lineData[2]), float.Parse(lineData[3]), float.Parse(lineData[4]));
                    }
                    temp = sr.ReadLine();
                }
                complete = true;
            }
            catch (FileNotFoundException fnfe)
            {
                Debug.Log("Could not find SpawnPoints.txt file: " + fnfe.Message);
                StreamWriter swTemp = new StreamWriter("SpawnPoints.txt");  //create Missing File.
                swTemp.WriteLine("Player:0,1,0");
                swTemp.WriteLine("gunSpawn_0:0,0,15");
                swTemp.WriteLine("gunSpawn_1:15,0,0");
                swTemp.WriteLine("gunSpawn_2:0,0,-15");
                swTemp.WriteLine("gunSpawn_3:-15,0,0");
                swTemp.Close();
            }
            catch (ArgumentException ae)
            {
                Debug.Log("A problem occured. Possible file content formatting issue:" + ae.Message);
            }
            catch (FormatException fe)
            {
                Debug.Log("An error occured while parsing data: " + fe);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if (complete)
                {
                    sr.Close();
                }
            }

        } while (!complete);

    }
    #endregion
}
