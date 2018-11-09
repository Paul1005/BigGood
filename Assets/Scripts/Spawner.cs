using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnPeriod;
    public ObjectPool pool;
    private Camera cam;
    private float camOffset;

    void Start()
    {
        cam = Camera.main;
        camOffset = -cam.transform.position.z;
        StartCoroutine(SpawnCycle());
    }

    IEnumerator SpawnCycle()
    {
        // TODO: check that the player isn't in the spawn position
        while (true)
        {
            // get a random point on screen
            Vector3 point = new Vector3(Random.value, Random.value, camOffset);
            Vector3 spawnPos = Camera.main.ViewportToWorldPoint(point);

            // get an object and place it there
            Poolable obj = pool.Pop();
            obj.transform.position = spawnPos;

            yield return new WaitForSeconds(spawnPeriod);
        }
    }
}
