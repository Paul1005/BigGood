using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : Poolable
{
    public float maxInitialSpeed;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        rb.velocity = Random.insideUnitCircle * maxInitialSpeed;
    }

    public void GetPickedUp ()
    {
        Pool.Push(this);
    }
}
