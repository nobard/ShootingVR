using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTarget : MonoBehaviour
{
    public float HealthPoints = 1f;
    public Animator TargetAnimator;
    public ScenarioManager Manager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spawnAudio;
    [SerializeField] private AudioClip shotAudio;

    public void OnHit(float amount)
    {
        HealthPoints -= amount;
        audioSource.clip = shotAudio;
        audioSource.Play();

        if(HealthPoints <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if(Manager == null) Debug.Log("Не установлен менеджер на мишень");
        //звук
        TargetAnimator.Play("TargetFall");
        Manager.PlayMainScenario();
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<Bullet>(out var bullet);
        OnHit(bullet.ParentGun.GetComponent<GunBase>().Damage);
    }
}