using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public abstract class GunBase : MonoBehaviour
{
    public abstract float Damage { get; protected set; }
    public abstract float BulletSpeed { get; protected set; }
    public abstract float ReloadSpeed { get; protected set; }
    public abstract int Capacity { get; protected set; }
    public abstract GunFireTypeEnum FireType { get; protected set; }
    public abstract Transform SpawnBulletPos { get; protected set; }
    public abstract GameObject BulletPrefab { get; protected set; }
    [SerializeField] private TextMeshProUGUI bulletsUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip reloadAudio;
    [SerializeField] private AudioClip shootAudio;
    [SerializeField] private AudioClip outOfAmmoAudio;
    public static event Action PistolShoot;
    public Animator GunAnimator;
    private bool isReloading = false;
    private int currCapacity;

    public abstract void Shoot();

    protected void SingleShoot()
    {
        if(currCapacity <= 0)
        {
            audioSource.clip = outOfAmmoAudio;
            audioSource.Play();
            
            return;
        }
        
        audioSource.clip = shootAudio;
        audioSource.Play();
        GunAnimator.Play("Shoot");
        //StopReloading();
        currCapacity--;
        var createBullet = Instantiate(BulletPrefab, SpawnBulletPos.position, SpawnBulletPos.rotation);
        createBullet.GetComponent<Rigidbody>().velocity = BulletSpeed * SpawnBulletPos.forward;
        createBullet.GetComponent<Bullet>().ParentGun = gameObject;
        Destroy(createBullet, 3f);
    }

    private void StopReloading()
    {
        StopAllCoroutines();
        isReloading = false;
    }

    private IEnumerator Reload()
    {
        //каждая следующая пуля заряжается быстрее
        for(var i = 0; i < Capacity; i++)
        {
            yield return new WaitForSeconds(ReloadSpeed / (i + 1));
            currCapacity++;
            audioSource.clip = reloadAudio;
            audioSource.Play();
        }

        isReloading = false;
    }

    private void UpdateBulletsText()
    {
        bulletsUI.text = string.Format($"{currCapacity}");
    }

    private void Start()
    {
        currCapacity = Capacity;
    }

    private void Update()
    {
        if(currCapacity <= 0)
        {
            if(!isReloading)
            {
                isReloading = true;
                StartCoroutine(Reload());
            }  
        }

        UpdateBulletsText();
    }
}