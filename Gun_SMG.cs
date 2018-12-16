using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Raul Rojas
//CSE 1302
//Project One
//6-23-2017

public class Gun_SMG : GunBase
{
    public override void shoot()
    {
        if(canShoot)
        {
            Instantiate(bulletPreFab, GameObject.Find("BulletSpawn").transform.position, Quaternion.identity);
            lastShot = Time.time;
            clipAmmo--;
            base.shoot();
            //Debug.Log("Clip Ammo: " + clipAmmo + " total Ammo: " + ammo);
        }

    }

}
