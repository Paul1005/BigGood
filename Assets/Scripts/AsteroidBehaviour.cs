using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : Poolable, IDamageable
{
    public int maxHealth;
    public float maxInitialSpeed;
    public int maxIterations;
    public float shrinkFactor;
    private int health;
    protected int level;
    private Rigidbody2D rb;
    private AudioSource explosionSound;
    private GameObject asteroidExplosion;
    void Awake ()
    {
        asteroidExplosion = GameObject.FindGameObjectWithTag("AsteroidExplosion");
        explosionSound = GameObject.Find("ExplosionManager").GetComponents<AudioSource>()[1];
        rb = GetComponent<Rigidbody2D>();
        level = 1;
    }

    void OnEnable ()
    {
        health = maxHealth;
        rb.velocity = Random.insideUnitCircle * maxInitialSpeed;
        level = 1;
    }
    
    public void InflictDamage(int value)
    {
        health -= value;
        if (health <= 0)
        {
            // move explosion to asteroid location and play
            asteroidExplosion.transform.position = transform.position;
            asteroidExplosion.GetComponent<ParticleSystem>().Play();

            explosionSound.Play();

            if (level++ < maxIterations)
            {
                // reset values of this current asteroid
                health = maxHealth;
                rb.velocity = Random.insideUnitCircle * maxInitialSpeed;

                // get other asteroid and place it on current
                AsteroidBehaviour other = Pool.Pop().GetComponent<AsteroidBehaviour>();
                other.transform.position = transform.position;
                other.level = level;

                // shrink both
                other.transform.localScale = transform.localScale *= shrinkFactor;
            }
            else
            {
                // if asteroid has reached its final iteration and was destroyed, put it back in the pool
                Pool.Push(this);
            }
        }
    }
}
