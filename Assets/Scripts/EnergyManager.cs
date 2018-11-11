using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnergyManager : MonoBehaviour
{
    /// <summary>
    /// Amount of energy at start.
    /// </summary>
    public float maxEnergy;
	/// <summary>
	/// What amount of energy is considered "low" enough to be worth warning the player about?
	/// </summary>
	public float lowEnergyWarningThreshold;
	/// <summary>
	/// What amount of energy is considered "low" enough to be dangerous?
	/// </summary>
	public float lowEnergyDangerThreshold;
    /// <summary>
    /// Energy regenerated per second.
    /// </summary>
    public float regenPerSecond;
    /// <summary>
    /// Delay in seconds before energy starts regenerating after last use.
    /// </summary>
    public float delaySeconds;
	/// <summary>
	/// Should RegenPerSecond be scaled by current remaining energy?
	/// </summary>
	public bool scaleRegenByRemaining;
	/// <summary>
	/// Should DelaySeconds be penalized if below warning/danger thresholds?
	/// </summary>
	public bool penalizeDelayByRemaining;
    /// <summary>
    /// Amount of energy spent per bullet.
    /// </summary>
    public float bulletCost;
    /// <summary>
    /// Initially represents amount of energy spent per second of thrust.
    /// Value is changed during runtime to represent amount of energy spent per tick of thrust.
    /// </summary>
    public float thrustCost;
    /// <summary>
    /// Amount of energy spent on collision with an enemy.
    /// </summary>
    public float collisionCost;

    public float energy;
    private bool regen;
    private Coroutine regenCoroutine;
    private AudioSource explosionSource;
    private AudioSource collisionSource;
    private AudioSource powerUpPickup;

	private GameManager gameManager;

    // Use this for initialization
    private void Start()
    {
        energy = maxEnergy;
        regen = true;
        collisionSource = GetComponents<AudioSource>()[1];
        powerUpPickup = GetComponents<AudioSource>()[2];
        explosionSource = GameObject.Find("ExplosionManager").GetComponents<AudioSource>()[0];
        thrustCost *= Time.fixedDeltaTime;
		gameManager = GameObject.FindGameObjectWithTag ("Player").GetComponent<GameManager> ();
    }

    private void Update()
    {
		if (regen) {
			if (energy < maxEnergy) {
				float tempRegenPerSecond = regenPerSecond;
				if (scaleRegenByRemaining) {
					tempRegenPerSecond = tempRegenPerSecond * (energy / maxEnergy);
				}
				// Modify further with additional conditional blocks here if desired.
				tempRegenPerSecond *= Time.deltaTime;

				energy += tempRegenPerSecond;
			}
		}

		if (energy > maxEnergy) {
			energy = maxEnergy; // Just a guard against having too much, for now. (Powerups?)
		}
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag.Equals("Asteroid"))
        {
            collisionSource.Play();
            energy -= collisionCost;
        }

        if (energy <= 0)
        {
            explosionSource.Play();
            GetComponent<ParticleSystem>().Play(); // player blows up

            GameObject.Find("PlayerMesh").SetActive(false); // TEMP BEHAVIOUR
            GetComponent<PolygonCollider2D>().enabled = false;

            gameManager.CurrentState = GameManager.GameState.GameOver;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        powerUpPickup.Play();
        if (col.CompareTag("EnergyPowerUp"))
        {
            EnergyPowerUpBehavior powerUpBehaviour = col.GetComponent<EnergyPowerUpBehavior>();
            energy += powerUpBehaviour.healAmt; 
            if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }
            powerUpBehaviour.GetPickedUp();
        }
        else if (col.CompareTag("SpeedPowerUp"))
        {
            SpeedPowerUpBehavior powerUpBehaviour = col.GetComponent<SpeedPowerUpBehavior>();
            StartCoroutine(powerUpBehaviour.Pickup(gameObject.GetComponent<Collider2D>()));
            powerUpBehaviour.GetPickedUp();
        }
        else if (col.CompareTag("ROFPowerUp"))
        {
            ROFPowerUpBehavior powerUpBehaviour = col.GetComponent<ROFPowerUpBehavior>();
            powerUpBehaviour.Effect(gameObject);
            powerUpBehaviour.GetPickedUp();
        }
        else if (col.CompareTag("beam"))
        {
            collisionSource.Play();
            energy -= collisionCost;
        }
    }

    /// <summary>
    /// Checks if a bullet can be fired, then spends the energy amount.
    /// </summary>
    /// <returns>Whether the energy cost of a bullet was available.</returns>
    public bool SpendBulletEnergy()
    {
        // guard clause: return false if not enough energy available, prevents regen delay from restarting
        if (energy < bulletCost)
        {
            return false;
        }

        StartRegenDelay();

        energy -= bulletCost;
        if (energy < 0)
        {
            energy = 0;
        }

        return true;
    }

    /// <summary>
    /// Uses up an amount of thrust energy, based off of the percentage of thrust
    /// requested and amount of fuel available.
    /// </summary>
    /// <param name="percent">Percentage of maximum thrust requested.</param>
    /// <returns>Percentage of maximum thrust possible.</returns>
    public float SpendThrustEnergy(float percent)
    {
        // guard clause: return 0 if no energy available, prevents regen delay from restarting
        if (energy == 0)
        {
            return 0;
        }

        StartRegenDelay();

        if (energy < thrustCost)
        {
            energy = 0;
            return energy / thrustCost * percent;
        }
        else
        {
            energy -= thrustCost * percent;
            return 1;
        }
    }

    /// <summary>
    /// Starts the regen delay coroutine, which temporarily stops energy from regenerating.
    /// </summary>
    private void StartRegenDelay()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }
        regenCoroutine = StartCoroutine(RegenDelay());
    }

    /// <summary>
    /// Disables energy regeneration for the number of seconds in delaySeconds.
    /// </summary>
    /// <returns>IEnumerator used by coroutine.</returns>
    private IEnumerator RegenDelay ()
    {
        regen = false;
		float tempDelayPerSecond = delaySeconds;
		if (penalizeDelayByRemaining) {
			// Use if/else here, don't double-apply these penalties.
			if(energy < lowEnergyDangerThreshold) {
				tempDelayPerSecond *= 2.0f;
			} else if (energy < lowEnergyWarningThreshold) {
				tempDelayPerSecond *= 1.25f;
			}
		}
		// Modify further with additional conditional blocks here if desired.

		yield return new WaitForSeconds(tempDelayPerSecond);
        regen = true;
    }
}
