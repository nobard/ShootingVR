using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpingTarget : TargetBase
{
    [SerializeField]
    private float healthPoints = 0f;
    public override float HealthPoints { get => healthPoints; protected set => healthPoints = value; }

    [SerializeField]
    private int pointsForHit = 10;
    public override int PointsForHit { get => pointsForHit; protected set => pointsForHit = value; }

    [SerializeField]
    private float timeReward = 2f;
    public override float TimeReward { get => timeReward; protected set => timeReward = value; }

    [SerializeField]
    private TargetMovementPath movementPath;
    public override TargetMovementPath MovementPath { get => movementPath; set => movementPath = value; }

    [SerializeField]
    private Animator targetAnimator;
    public override Animator TargetAnimator { get => targetAnimator; protected set => targetAnimator = value; }

    protected override void MoveTarget()
    {
        transform.position = Vector3.Lerp(transform.position, pointInPath.Current.position, Time.deltaTime * Speed);
    }
}