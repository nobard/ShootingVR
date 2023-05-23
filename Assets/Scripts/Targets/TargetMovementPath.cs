using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}