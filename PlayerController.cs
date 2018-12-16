using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.AI;

//Author: Raul Rojas
//CSE 1302
//Project One
//6-23-2017

public class PlayerController : MonoBehaviour
{
    #region Fields

    #region Public
    public float speed = 1;
    public RaycastHit aimpoint;
    public KeyCode Fire;
    public KeyCode changeWeapon;
    public KeyCode reload;
    public int maxHealth = 100;
    public bool godMode;
    #endregion

    #region Private
    private List<GunBase> inventory;
    private int currentWeapon;
    private int health;
    private bool isAlive;
    private bool pressedReload;
    #endregion

    #endregion

    #region Properties

    public List<GunBase> Inventory
    {
        get { return inventory; }
    }

    public int Health
    {
        get { return this.health; }
        set { this.health = value; }
    }

    public int CurrentWeapon
    {
        get { return this.currentWeapon; }
    }

    public bool IsAlive
    {
        get { return this.isAlive; }
    }

    #endregion

    #region MonoBehaviors

    void Start()
    {
        inventory = new List<GunBase>();
        currentWeapon = 0;
        health = maxHealth;
        isAlive = true;
    }

    void Update()
    {
        if (isAlive)
        {
            move();
            look();
            shoot();
            weaponCycle();
        }
    }

    #endregion

    #region Public Methods

    public void damage(int dmg)
    {
        if (!godMode && isAlive)
        {
            health -= dmg;
            if (health <= 0)
            {
                health = 0;
                isAlive = false;
                AudioManager.instance.Stop(GameStructs.ClipName.GameMusic);
                AudioManager.instance.Play(GameStructs.ClipName.PlayerDeathMusic);
                AudioManager.instance.Play(GameStructs.ClipName.PlayerDeath);
                
            }
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

    public void addToInventory(GunBase gun)
    {
        AudioManager.instance.Play(GameStructs.ClipName.HolsterWeapon);
        gun.transform.parent.GetChild(0).gameObject.SetActive(false);
        if (inventory.Contains(gun))
        {
            //Debug.Log("inventory already contains this weapon.");
            foreach (GunBase storedGun in inventory)
            {
                if (storedGun.Equals(gun))
                {
                    storedGun.Ammo += storedGun.clipSize;
                    if (storedGun.Ammo > storedGun.maxAmmo)
                    {
                        storedGun.Ammo = storedGun.maxAmmo;
                    }
                }
            }

            Destroy(gun.gameObject);
        }
        else
        {
            //Debug.Log("added weapon to inventory.");
            GameObject gunSpawn = GameObject.Find("GunSpawn");
            inventory.Add(gun);
            gun.gameObject.transform.SetParent(gunSpawn.transform);
            gun.gameObject.tag = "PlayerOwned";
            gun.transform.rotation = gunSpawn.transform.rotation;
            gun.transform.localScale = Vector3.one;
            gun.transform.position = gunSpawn.transform.position;
            if (inventory.Count > 1)
            {
                //Debug.Log("SetActive to FALSE. inventory count: " + inventory.Count);
                gun.gameObject.SetActive(false);
            }
            else
                gun.gameObject.SetActive(true);
        }
    }

    #endregion

    #region Private Methods

    void move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = Vector3.Normalize(new Vector3(moveHorizontal, 0.0f, moveVertical));
        this.GetComponent<Rigidbody>().MovePosition(this.transform.position + speed * Time.deltaTime * movement);
        //agent.Move(movement);
    }

    void weaponCycle()
    {
        if (Input.GetKeyDown(changeWeapon) && (inventory.Count>1))
        {
            AudioManager.instance.Play(GameStructs.ClipName.HolsterWeapon);
            inventory[currentWeapon].gameObject.SetActive(false);
            currentWeapon = (currentWeapon == (inventory.Count - 1)) ? 0 : currentWeapon + 1;
            inventory[currentWeapon].gameObject.SetActive(true);

        }
    }

    void look()
    {
        Ray aim = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

        Vector3 lookPoint = new Vector3();
        if (Physics.Raycast(aim, out aimpoint, 100f))
        {
            lookPoint = aimpoint.point;
            lookPoint.y = this.transform.position.y;
            transform.LookAt(lookPoint);
        }
    }

    void shoot()
    {
        if (inventory.Count>0)
        {
            if (inventory[currentWeapon].isAutomatic)
            {
                if (Input.GetKey(Fire))
                {
                    inventory[currentWeapon].shoot();
                }
            }
            else
            {
                if (Input.GetKeyDown(Fire))
                {
                    inventory[currentWeapon].shoot();
                }
            }

        }

        if (Input.GetKeyDown(reload))
        {
            inventory[currentWeapon].PressedReload = true;
        }
    }

    #endregion
}
