using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Raul Rojas
//CSE 1302
//Project One
//6-23-2017

public abstract class GunBase: MonoBehaviour, System.IEquatable<GunBase>
{
    #region Fields
    
    #region Public
    public string gunName = "";
    public float fireRate = 1;
    public GameObject bulletPreFab;
    public bool isAutomatic;
    public int maxAmmo;
    public int clipSize;
    public float reloadTime = 1;
    //public ParticleSystem gunFireParticles;
    #endregion

    #region Protected
    protected float lastShot;
    protected bool canShoot;
    protected int ammo;
    protected int clipAmmo;
    protected float reloadFinishTime;
    protected bool isReloading;
    #endregion

    #region Private
    private int reloadState;
    private bool pressedReload;
    #endregion

    #endregion

    #region Properties

    public int Ammo
    {
        get { return this.ammo; }
        set { this.ammo = value; }
    }

    public int ClipAmmo
    {
        get { return this.clipAmmo; }
    }

    public bool IsReloading
    {
        get { return this.isReloading; }
    }

    public float ReloadFinishTime
    {
        get { return this.reloadFinishTime; }
    }

    public bool PressedReload
    {
        get { return this.pressedReload; }
        set { this.pressedReload = value; }
    }

    #endregion

    #region MonoBehaviours

    private void Start()
    {
        lastShot = -fireRate;
        canShoot = true;
        isReloading = false;
        ammo = 0;
        clipAmmo = clipSize;
        reloadState = 0;
    }
    
    private void Update()
    {
        if (((Time.time - lastShot) >= fireRate) && clipAmmo > 0 && !isReloading)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }
        reloadControl();
    }

    #endregion
    
    #region Public Methods

    public bool Equals(GunBase other)
    {
        if (this.gunName == other.gunName)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void shoot()
    {
        GameController gameControl = GameObject.Find("GameController").GetComponent<GameController>();
        gameControl.ShotsFired++;
        playFireSound();
        //Debug.Log("Incrimented ShotsFired: " + gameControl.ShotsFired);
    }

    public void rotate()
    {
        this.gameObject.transform.Rotate(new Vector3(0, 1, 0), Space.World);
    }
    
    public void reload()
    {
        int diff = clipSize - clipAmmo;
        if (diff > ammo)
        {
            clipAmmo += ammo;
            ammo = 0;
        }
        else
        {
            clipAmmo = clipSize;
            ammo -= diff;
        }
    }

    public void reloadControl()
    {
        switch(reloadState)
        {
            case 0:
                if ((clipAmmo <= 0 || pressedReload) && ammo > 0 && (clipAmmo < clipSize))
                {
                    isReloading = true;
                    canShoot = false;
                    reloadFinishTime = Time.time + reloadTime;
                    reloadState = 1;
                    playReloadSound();
                }
                else pressedReload = false;
                break;
            case 1:
                if (Time.time >= reloadFinishTime)
                {
                    isReloading = false;
                    canShoot = true;
                    pressedReload = false;
                    reloadState = 0;
                    reload();
                }
                break;
            default:
                reloadState = 0;
                break;
        }
    }
    #endregion

    #region Private Methods

    private void playReloadSound()
    {
        if(this is Gun_Shotgun)
        {
            AudioManager.instance.Play(GameStructs.ClipName.ShotgunReload);
        }
        else
        {
            AudioManager.instance.Play(GameStructs.ClipName.SmgReload);
        }
    }

    private void playFireSound()
    {
        if (this is Gun_Pistol)
        {
            AudioManager.instance.Play(GameStructs.ClipName.PistolFire);
        }
        else if(this is Gun_Shotgun)
        {
            AudioManager.instance.Play(GameStructs.ClipName.ShotgunFire);
        }
        else
        {
            AudioManager.instance.Play(GameStructs.ClipName.PistolFire);
        }
    }

    #endregion
}
