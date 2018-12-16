using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Shotgun : GunBase
{
    public int pellets = 1;
    public float dispersion = 1;

    private Quaternion originalRot;

    public Quaternion OriginalRot
    {
        get { return this.originalRot; }
    }

    public override void shoot()
    {
        if (canShoot)
        {
            originalRot = GameObject.Find("ShotAnchor").transform.rotation;
            
            for (int i = 0; i < pellets; i++)
            {
                Instantiate(bulletPreFab, GameObject.Find("BulletSpawn").transform.position, Quaternion.identity);
                GameObject.Find("ShotAnchor").transform.rotation = GameObject.Find("Player").transform.rotation;
                base.shoot();
            }
            GameObject.Find("ShotAnchor").transform.rotation = GameObject.Find("Player").transform.rotation;
            lastShot = Time.time;
            clipAmmo--;
            //Debug.Log("Clip Ammo: " + clipAmmo + " total Ammo: " + ammo);
        }

    }
}
