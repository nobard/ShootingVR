using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTarget : MonoBehaviour
{
    public float HealthPoints = 1f;
    public Animator TargetAnimator;
    public LevelManager Manager;
    public int PointsForHit = 5;

    public void OnHit(float amount)
    {
        HealthPoints -= amount;
        if(HealthPoints <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if(Manager == null) Debug.Log("Не установлен менеджер на мишень");
        Manager.CurrScores += PointsForHit;
        //звук
        TargetAnimator.enabled = true;
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<Bullet>(out var bullet);
        OnHit(bullet.ParentGun.GetComponent<GunBase>().Damage);
    }
}