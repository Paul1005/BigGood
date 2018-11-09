using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed;
    public float maxspeed = 1;
    public float shootingDelay;//time between shots in seconds
    public Transform player;
    public GameObject bullet;
    public float lastTimes = 0f;
    public float bulletSpeed;
    public float maxbulletSpeed = 250;
    public MeshRenderer meshrenderer;
    public Collider2D col;
    public bool disable; // true when disable
    public float timebeforespaning;
    float levelstarttime;
    public Transform startposition;
    public int health = 3;
    private int maxhealth = 3;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        levelstarttime = Time.time;
        timebeforespaning = Random.Range(3f, 5f);
        Invoke("Enable", timebeforespaning);
        Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (disable)
        {
            return;
        }
        if (Time.time > lastTimes + shootingDelay)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject newBullet = Instantiate(bullet, transform.position, q);

            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, bulletSpeed));
            lastTimes = Time.time;
        }
    }
    void FixedUpdate()
    {
        if (disable)
        {
            return;
        }
        direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
    void Enable()
    {
        transform.position = startposition.position;
        col.enabled = true;
        meshrenderer.enabled = true;
        disable = false;
    }
    void Disable()
    {
        col.enabled = false;
        meshrenderer.enabled = false;
        disable = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            health--;
            if (health <= 0)
            {
                Disable();
                maxhealth++;
                health = maxhealth;
                maxspeed *= 2;
                speed = maxspeed;
                maxbulletSpeed *= 2;
                bulletSpeed = maxbulletSpeed;
                Invoke("Enable", 6);
            }
        }
    }
}
