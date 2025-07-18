using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointPlatformMove : MonoBehaviour
{

    [SerializeField] public float platformSpeed = 0.5f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.down * platformSpeed;
    }

}
