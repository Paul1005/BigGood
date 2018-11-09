using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    private Rigidbody2D rb;
	private float horizontalEdge;
	private float verticalEdge;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

		horizontalEdge = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, -Camera.main.transform.position.z)).x;
		verticalEdge = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, -Camera.main.transform.position.z)).y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = rb.position;

		if (rb.transform.position.x > horizontalEdge) {
			newPosition.x = -horizontalEdge;
		} else if (rb.transform.position.x < -horizontalEdge) {
			newPosition.x = horizontalEdge;
		} else if (rb.transform.position.y > verticalEdge) {
			newPosition.y = -verticalEdge;
		} else if (rb.transform.position.y < -verticalEdge) {
			newPosition.y = verticalEdge;
		}

        rb.position = newPosition;
    }
}
