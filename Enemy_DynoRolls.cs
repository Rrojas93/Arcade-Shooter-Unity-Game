using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Raul Rojas
//CSE 1302
//Project One
//6-23-2017

public class Enemy_DynoRolls : Enemy_Base
{
    public float radius = 5;
    public override void move()
    {   
        GameObject player = GameObject.Find("Player");
        Vector3 moveDirection = player.transform.position - this.transform.position;
        moveDirection = Vector3.Normalize(moveDirection) + player.GetComponent<Rigidbody>().velocity;
        if (Vector3.Distance(player.transform.position,this.transform.position) > radius)
        {
            this.GetComponent<Rigidbody>().AddForce(100 *speed * Time.deltaTime * moveDirection);
        }
    }
}
