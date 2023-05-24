using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class GunBase : MonoBehaviour
{
    public abstract float Damage { get; protected set; }
    public abstract float BulletSpeed { get; protected set; }
    public abstract int Capacity { get; protected set; }
    public abstract GunFireTypeEnum FireType { get; protected set; }
    public abstract Transform SpawnBulletPos { get; protected set; }
    public abstract GameObject BulletPrefab { get; protected set; }
    public static event Action PistolShoot;

    public void Shoot()
    {
        //GetComponent<AudioSource>().Play();
        var createBullet = Instantiate(BulletPrefab, SpawnBulletPos.position, SpawnBulletPos.rotation);
        createBullet.GetComponent<Rigidbody>().velocity = BulletSpeed * SpawnBulletPos.forward;
        createBullet.GetComponent<Bullet>().ParentGun = gameObject;
        Destroy(createBullet, 10f);
        PistolShoot?.Invoke();   
    }
}