using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : Poolable
{
    public float speed;
    public float lifetime;
    public int damage;
    private Rigidbody2D rb;
    private Coroutine despawnCoroutine;

    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (despawnCoroutine != null)
        {
            StopCoroutine(despawnCoroutine);
            despawnCoroutine = null;
        }

        // TODO: maybe damageable objects should have a tag? string comparison may be faster than getting component and null checking
        IDamageable enemy = col.gameObject.GetComponent<IDamageable>();
        if (enemy != null)
        {
            enemy.InflictDamage(damage);
        }

        Pool.Push(this);
    }

    public void Fire (Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
        despawnCoroutine = StartCoroutine(DespawnTimer());
    }

    // disappear after the bullet's lifetime is up and return it to its pool
    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(lifetime);
        Pool.Push(this);
    }
}
