using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TargetBase : MonoBehaviour
{
    public abstract float HealthPoints { get; protected set; }
    public abstract TargetMovementPath MovementPath { get; protected set; }
    public abstract Animator TargetAnimator { get; protected set; }
    public float Speed = 1f;
    public float MaxDistance = 0.1f;
    protected IEnumerator<Transform> pointInPath;
    public static event Action TargetShooted;

    protected abstract void MoveTarget();

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
        //звук
        TargetAnimator.enabled = true;
        Destroy(gameObject, 5f);
    }

    private void Start()
    {
        if(MovementPath == null)
        {
            Debug.Log("Примени путь");
            return;
        }

        pointInPath = MovementPath.GetNextPathPoint();
        pointInPath.MoveNext();

        if(pointInPath.Current == null)
        {
            Debug.Log("Нужны точки");
            return;
        }

        transform.position = pointInPath.Current.position;
    }

    private void Update()
    {
        if(pointInPath == null || pointInPath.Current == null) return;

        MoveTarget();

        var distSquare = (transform.position - pointInPath.Current.position).sqrMagnitude;

        if(distSquare < MaxDistance * MaxDistance)
        {
            pointInPath.MoveNext();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<Bullet>(out var bullet);
        OnHit(bullet.ParentGun.GetComponent<GunBase>().Damage);
    }
}