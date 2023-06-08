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
        
        if(HealthPoints <= 0f)
        {
            audioSource.clip = shotAudio;
            audioSource.Play();
            Die();
        }
    }

    private void Die()
    {
        if(Manager == null) Debug.Log("Не установлен менеджер на мишень");
        
        TargetAnimator.Play("TargetFall");
        Manager.PlayMainScenario();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Bullet>(out var bullet))
        {
            OnHit(bullet.ParentGun.GetComponent<GunBase>().Damage);
            Destroy(collision.gameObject);
        }
    }
}