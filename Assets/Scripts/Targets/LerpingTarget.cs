using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpingTarget : TargetBase
{
    [SerializeField]
    private float healthPoints;
    public override float HealthPoints { get => healthPoints; protected set => healthPoints = value; }

    [SerializeField]
    private TargetMovementPath movementPath;
    public override TargetMovementPath MovementPath { get => movementPath; protected set => movementPath = value; }

    [SerializeField]
    private Animator targetAnimator;
    public override Animator TargetAnimator { get => targetAnimator; protected set => targetAnimator = value; }

    protected override void MoveTarget()
    {
        transform.position = Vector3.Lerp(transform.position, pointInPath.Current.position, Time.deltaTime * Speed);
    }
}