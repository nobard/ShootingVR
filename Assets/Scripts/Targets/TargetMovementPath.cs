using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetMovementPath : MonoBehaviour
{
    public TargetPathTypeEnum PathType;
    public Transform[] PathElements;
    public GameObject CenterPoint;
    
    public void OnDrawGizmos()
    {
        if(PathElements == null || PathElements.Length < 2) return;

        for(var i = 1; i < PathElements.Length; i++)
        {
            Gizmos.DrawLine(PathElements[i - 1].position, PathElements[i].position);
        }

        if(PathType == TargetPathTypeEnum.Loop)
        {
            Gizmos.DrawLine(PathElements[0].position, PathElements[PathElements.Length - 1].position);
        }
    }

    /// <summary>
    /// Спавнит и возвращает объект мишени
    /// </summary>
    /// <param name="target"> Префаб мишени </param>
    public TargetBase SpawnAndGetTarget(TargetBase target, int targetQueue)
    {
        if(PathType != TargetPathTypeEnum.Loop) return SpawnTarget(target, GetRandomSpawnPoint(), targetQueue);

        return SpawnTarget(target, 0, targetQueue);
    }

    /// <summary>
    /// Возвращает новую мишень
    /// </summary>
    /// <param name="target"> Префаб мишени </param>
    /// <param name="spawnPoint"> Индекс точки для спавна </param>
    private TargetBase SpawnTarget(TargetBase target, int spawnPoint, int targetQueue) 
    {
        var newTarget = Instantiate(target, PathElements[spawnPoint].position, target.transform.rotation);
        newTarget.MovementPath = this;
        newTarget.MovingTo = spawnPoint;
        newTarget.FixedLookAt(CenterPoint.transform);
        newTarget.PlaySpawnAnimation();
        //newTarget.StopByTime(Random.Range(2f, 3f) * targetQueue);
        
        return newTarget;
    }

    /// <summary>
    /// Возвращает индекс точки для мишени(первая или последняя)
    /// </summary>
    private int GetRandomSpawnPoint()
    {
        var a = Random.Range(0, 2);
        if(a == 0) return 0;

        return PathElements.Length - 1;
    }
}