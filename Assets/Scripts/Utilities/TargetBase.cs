using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TargetBase : MonoBehaviour
{
    public abstract float HealthPoints { get; protected set; }
    public abstract int PointsForHit { get; protected set; }
    public abstract TargetMovementPath MovementPath { get; set; }
    public abstract Animator TargetAnimator { get; protected set; }
    public float Speed = 1f;
    public float MaxDistance = 0.1f;
    [HideInInspector] public int MovingTo = 0;
    [HideInInspector] public int MovementDirection = 1;
    [HideInInspector] public LevelManager Manager;
    protected IEnumerator<Transform> pointInPath;
    public static event Action TargetShooted;
    private bool isHitted = false;
    private bool isStopped = false;
    
    protected abstract void MoveTarget();

    public void OnHit(float amount)
    {
        HealthPoints -= amount;
        if(HealthPoints <= 0f)
        {
            isHitted = true;
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
        Manager.currLvlTargets--;
    }

    // public void StopByTime(float seconds)
    // {
    //     StartCoroutine(StopByTimeCoroutine(seconds));
    // }

    // private IEnumerator StopByTimeCoroutine(float seconds)
    // {
    //     isStopped = true;

    //     yield return new WaitForSeconds(seconds);

    //     isStopped = false;
    // }

    private void Start()
    {
        if(MovementPath == null)
        {
            Debug.Log("Примени путь");
            return;
        }

        pointInPath = GetNextPathPoint();
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
        if(pointInPath == null || pointInPath.Current == null || isStopped) return;

        if(MovementPath.CenterPoint != null)
        {
            transform.LookAt(MovementPath.CenterPoint.transform);
            // фикс форвард оси
            transform.Rotate(0f, 90f, 180f);
        }

        if(!isHitted) MoveTarget();

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

    public IEnumerator<Transform> GetNextPathPoint()
    {
        if(MovementPath.PathElements == null || MovementPath.PathElements.Length < 1) yield break;

        while(true)
        {
            yield return MovementPath.PathElements[MovingTo];

            if(MovementPath.PathElements.Length == 1) continue;

            if(MovementPath.PathType == TargetPathTypeEnum.Linear)
            {
                if(MovingTo <= 0)
                {
                    MovementDirection = 1;
                }
                else if(MovingTo >= MovementPath.PathElements.Length - 1)
                {
                    MovementDirection = -1;
                }
            }

            MovingTo += MovementDirection;

            if(MovementPath.PathType == TargetPathTypeEnum.Loop)
            {
                if(MovingTo >= MovementPath.PathElements.Length)
                {
                    MovingTo = 0;
                }
                
                if(MovingTo < 0)
                {
                    MovingTo = MovementPath.PathElements.Length - 1;
                }
            }
        }
    }
}