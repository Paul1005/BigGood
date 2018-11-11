using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;
    private Vector3 spinDirection;

    void Start ()
    {
        spinDirection = Random.insideUnitSphere.normalized;
    }

	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(spinDirection * Time.deltaTime * speed);
	}
}
