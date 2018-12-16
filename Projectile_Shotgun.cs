using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Raul Rojas
//CSE 1302
//Project One
//6-23-2017

public class Projectile_Shotgun : Projectile
{
    private float dispersion;
    private int shotCountMax;
    private static int shotCount = 0;
    private Quaternion originalRotation;
    public GameObject pellets;

    public override void Fire()
    {
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
        Gun_Shotgun gunScript = player.Inventory[player.CurrentWeapon].GetComponent<Gun_Shotgun>();
        shotCountMax = gunScript.pellets;
        dispersion = gunScript.dispersion;
        originalRotation = gunScript.OriginalRot;
        shotCount++;
        if (shotCount > shotCountMax)
        {
            shotCount = 1;
        }

        adjustAnchor();

        getDirection();
        this.GetComponent<Rigidbody>().AddForce(direction * 100f * bulletSpeed);

        
        GameObject.Find("ShotAnchor").transform.rotation = originalRotation;
    }

    void adjustAnchor()
    {
        float shotDirection = ((shotCountMax / 2) * -dispersion) + (dispersion*shotCount);
        GameObject.Find("ShotAnchor").transform.Rotate(0f, shotDirection, 0f);
    }
    
}
