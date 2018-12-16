using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Raul Rojas
//CSE 1302
//Project One
//6-23-2017

public class Projectile_SMG : Projectile
{
    public override void Fire()
    {
        getDirection();
        this.GetComponent<Rigidbody>().AddForce(direction * 100f * bulletSpeed);
    }
}
