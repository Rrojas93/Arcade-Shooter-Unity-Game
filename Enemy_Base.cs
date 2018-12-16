using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Raul Rojas
//CSE 1302
//Project One
//6-23-2017

public abstract class Enemy_Base : MonoBehaviour
{
    #region Fields
    //public string name;
    public int maxHealth = 100;
    public float speed = 1;
    public int damage = 1;
    public float score = 1;

    protected int health;
    private NavMeshAgent agent;
    private float attackRate = 2;
    private float lastAttackTime;
    private GameController gameControl;
    protected static GameObject player;
    #endregion

    #region Properties

    public int Health
    {
        get { return this.health; }
    }



    #endregion

    #region MonoBehaviours

    private void Awake()
    {
        player = GameObject.Find("Player");        
    }
    protected void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        health = maxHealth;
        lastAttackTime = Time.time;
        gameControl = GameObject.Find("GameController").GetComponent<GameController>();
    }

    #endregion

    #region Public Methods

    public virtual void move()
    {
        Vector3 destination = player.transform.position;
        agent.SetDestination(destination);
    }

    public virtual void attack()
    {
        if (lastAttackTime != Time.time && ((Time.time - lastAttackTime) > attackRate)) 
        {
            GameObject.Find("Player").GetComponent<PlayerController>().damage(damage);
            lastAttackTime = Time.time;
            if (this is Enemy_DynoRolls)
            {
                playDeathSound();
                Destroy(this.gameObject);
            }
        }
    }

    public void takeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            die();
        }
    }

    public void heal(int healAmt)
    {
        health += healAmt;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    #endregion

    #region Private Methods

    protected virtual void die()
    {
        gameControl.EnemyKillScore += this.score;
        playDeathSound();
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BlueBullet")
        {
            int damage = getDamage(other);
            takeDamage(damage);
            gameControl.ShotsLanded++;
            //Debug.Log("Incrimented ShotsLanded: " + gameControl.ShotsLanded);
        }
    }

    int getDamage(Collider other)
    {
        //using polymorphism to get damage variable from base class of the gun projectiles
        return other.GetComponent<Projectile>().damage; 
    }

    private void playDeathSound()
    {
        if (this is Enemy_DynoRolls)
        {
            AudioManager.instance.Play(GameStructs.ClipName.DynoDeath);
        }
        else if(this is Enemy_Necro)
        {
            AudioManager.instance.Play(GameStructs.ClipName.NecroDeath);
        }
        else
        {
            AudioManager.instance.Play(GameStructs.ClipName.ZombieDeath);
        }
    }
    #endregion

}
