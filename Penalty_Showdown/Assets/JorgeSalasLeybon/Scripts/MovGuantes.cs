using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovGuantes : MonoBehaviour
{
    public float Speed;
    public float Rango;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float yPos = Mathf.PingPong(Time.time * Speed, Rango);
        transform.position = new Vector3(startPosition.x, startPosition.y + yPos, startPosition.z);
    }
}
