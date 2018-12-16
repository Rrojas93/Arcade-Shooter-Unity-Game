using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Raul Rojas
//CSE 1302
//Project One
//6-23-2017

public class Enemy_Necro : Enemy_Base
{
    #region Fields

    public int spawnPosMin = -5;
    public int spawnPosMax = 6;
    
    public int cloneMax = 8;
    public int cloneTimer = 5;
    public GameObject necroPref;

    protected static int cloneCount = 0;

    private float spawnTime;
    private bool hasCloned = false;


    public int CloneCount
    {
        get { return cloneCount; }
        set { cloneCount = value; }
    }
    #endregion

    #region MonoBehaviours

    private new void Start()
    {
        base.Start();
        spawnTime = Time.time-0.1f;
        cloneTimer = getCloneTime(cloneTimer);
        //Debug.Log("Clone Count: " + cloneCount);
    }

    #endregion

    #region Public Methods

    //public override void move()
    //{
    //    Debug.Log("necro move method called.");
    //    agent.SetDestination(player.transform.position);
    //}
    #endregion

    #region Private Methods

    public void cloneCheck()
    {
        if ((cloneCount < cloneMax) && ((Time.time - spawnTime) >= (float)cloneTimer) && !hasCloned)
        {
            clone();
            hasCloned = true;
            spawnTime = Time.time;
        }
        
    }

    public static void resetCloneCount()
    {
        cloneCount = 0;
    }

    private int getCloneTime(int time)
    {
        System.Random r = new System.Random();
        return r.Next(time, (time * 2));
    }

    private void clone()
    {
        System.Random r = new System.Random();

        Vector3 spawnPosition = this.transform.position + new Vector3(r.Next(spawnPosMin, spawnPosMax), 0f, r.Next(spawnPosMin, spawnPosMax));

        /*GameObject temp = */Instantiate(necroPref, spawnPosition, Quaternion.identity);
        //temp.GetComponent<Enemy_Necro>().score = this.score * 0.5f;
        cloneCount++;
    }

    protected override void die()
    {
        //cloneCount--;
        base.die();
        
    }
    #endregion
}
