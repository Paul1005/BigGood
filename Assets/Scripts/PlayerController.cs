using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float time;
    public float fireWait;
    public float thrust;
    public float maxControlledVelocity;
    public float decelerationPerTick;
    public ObjectPool bulletPool;
    public Vector2[] barrels;
    private Rigidbody2D rb;
    private Vector2 aimDir;
	private Vector2 lastMousePos;
    private int barrelNum;
    private AudioSource gunSoundSource;
    private bool stickControlled;
    private Camera cam;
    private float camOffset;
    private EnergyManager energyManager;
    // input bools
    private bool fire1;

	private InputController playerInput;
	private GameManager gameManager;

	void Awake()
	{
        time = Time.time;
		playerInput = GameObject.FindGameObjectWithTag ("Player").GetComponent<InputController> ();
		gameManager = GameObject.FindGameObjectWithTag ("Player").GetComponent<GameManager> ();
	}

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gunSoundSource = GetComponents<AudioSource>()[0];
        barrelNum = 0;
        fire1 = false;
        aimDir = Vector2.up;
        lastMousePos = Input.mousePosition;
        stickControlled = false;
        cam = Camera.main;
        camOffset = -cam.transform.position.z;
        energyManager = GetComponent<EnergyManager>();
    }

    // Update is called once per frame
    void Update()
	{
		if (gameManager.CurrentState != GameManager.GameState.Gameplay) {
			return;
		}
		// grab both input types for aiming
		if (playerInput.CurrentInput.StickDir.magnitude != 0) // if a joystick is making input, prioritize it over mouse
        {
			aimDir = playerInput.CurrentInput.StickDir;
            stickControlled = true;
        }
		else if (!stickControlled || playerInput.CurrentInput.MousePos != lastMousePos) // if no joystick input is made and a mouse input IS made, check its position
        {
            // convert screen point to world point
			Vector3 viewportPoint = cam.ScreenToViewportPoint(playerInput.CurrentInput.MousePos);
            viewportPoint.z = camOffset;
            Vector3 worldPoint = cam.ViewportToWorldPoint(viewportPoint);
#if UNITY_EDITOR
            // draw a line on the debug screen showing where the mouse is
            Debug.DrawLine(rb.position, worldPoint);
#endif
            aimDir = (Vector2)worldPoint - rb.position;

            stickControlled = false;
        }

        // rotate the ship to face the aim direction
        rb.rotation = -Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;

        // set up historical mouse position for next frame
		lastMousePos = playerInput.CurrentInput.MousePos;

		// handle button inputs
		if (playerInput.CheckControlBinding("Fire1")) {
			fire1 = true;
		}
	}

    // FixedUpdate is called every physics tick
    void FixedUpdate()
    {
		float percent = playerInput.CurrentInput.MoveDir.magnitude;

		if (playerInput.CurrentInput.MoveDir.magnitude > 0)
        {
            percent = energyManager.SpendThrustEnergy(percent);
        }

        /* movement handling */
		if (playerInput.CurrentInput.MoveDir == Vector2.zero)
        {
            // decelerate if no input
			rb.velocity *= 1 - decelerationPerTick;
        }
        else
        {
			rb.AddForce(playerInput.CurrentInput.MoveDir * thrust * percent);

            // clamp velocity to max speed
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxControlledVelocity);
        }

        /* weapon handling */
        if (fire1 && Time.time >= (time + fireWait))
        {
            if (energyManager.SpendBulletEnergy())
            {
                gunSoundSource.Play();
                Poolable obj = bulletPool.Pop();
                BulletBehaviour bullet = obj.GetComponent<BulletBehaviour>();

                // sin and cosine for current aim direction
                float sin = Mathf.Sin(rb.rotation * Mathf.Deg2Rad);
                float cos = Mathf.Cos(rb.rotation * Mathf.Deg2Rad);

                // find offset for gun barrel
                Vector2 b = barrels[barrelNum];
                Vector2 offset = new Vector2(b.x * cos - b.y * sin,
                        b.x * sin + b.y * cos);
                if (++barrelNum >= barrels.Length)
                {
                    barrelNum = 0;
                }

                // fire bullet from offset position
                bullet.transform.position = rb.position + offset;
                bullet.Fire(aimDir);
                time = Time.time;
            }

            // reset input bool
            fire1 = false;
        }
    }
}