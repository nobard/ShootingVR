using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunBase
{
    [SerializeField]
    private float damage = 10f;
    public override float Damage { get => damage; protected set => damage = value; }

    [SerializeField]
    private float bulletSpeed = 50f;
    public override float BulletSpeed { get => bulletSpeed; protected set => bulletSpeed = value; }

    [SerializeField]
    private float reloadSpeed = 1f;
    public override float ReloadSpeed { get => reloadSpeed; protected set => reloadSpeed = value; }

    [SerializeField]
    private int capacity;
    public override int Capacity { get => capacity; protected set => capacity = value; }

    [SerializeField]
    private GunFireTypeEnum fireType;
    public override GunFireTypeEnum FireType { get => fireType; protected set => fireType = value; }

    [SerializeField]
    private Transform spawnBulletPos;
    public override Transform SpawnBulletPos { get => spawnBulletPos; protected set => spawnBulletPos = value; }

    [SerializeField]
    private GameObject bulletPrefab;
    public override GameObject BulletPrefab { get => bulletPrefab; protected set => bulletPrefab = value; }

    public override void Shoot()
    {
        SingleShoot();
    }
}