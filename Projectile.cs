using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    #region Fields
    public int damage;
    public float bulletSpeed = 1;
    public float bulletRange = 50;

    protected Vector3 direction;

    private Transform bulletSpawn;

    #endregion

    #region MonoBehaviours

    void Start()
    {
        bulletSpawn = GameObject.Find("BulletSpawn").GetComponent<Transform>();
        Fire();
    }

    private void Update()
    {
        rangeCheck();
    }

    #endregion

    #region Public Methods
    
    abstract public void Fire();

    #endregion

    #region Private Methods

    private void rangeCheck()
    {
        if (Vector3.Distance(bulletSpawn.position, this.transform.position) > bulletRange)
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name != this.name)
        {
            Destroy(this.gameObject);
        }
        
    }
    #endregion

    #region Protected Methods

    protected virtual void getDirection()
    {
        direction = -GameObject.Find("ShotAnchor").transform.position + GameObject.Find("ShotDirection").transform.position;
        direction = direction.normalized;
    }

    #endregion
    
}
