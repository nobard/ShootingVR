using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusLight : MonoBehaviour
{
    public float RotateSpeed = 5f;
    private void Update()
    {
        gameObject.transform.Rotate(0f, RotateSpeed * Time.deltaTime, 0f);
    }
}
