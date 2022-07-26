///----------------------------------------------------------------------------------
///  Bullet.cs
///  Description       : Is attached to Bullet prefabs.
///						 Once the clone bullet is activated, moves the bullet to the
///						 given direction. 
///						 Deactivates the bullet and destorys the spawnObjects if it
///						 collides with the spawnd objects.
///						 Deactivates the bullet if the distance from starting points 
///						 is greater than a specific amount
///----------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private float Speed;
	public Vector3 direction;
	private Vector3 startPos;
	private void Start()
	{
		startPos = transform.position;
	}
	private void Update()
	{
		Move();
		if(Vector3.Magnitude(startPos-transform.position)>200)
		{
			gameObject.SetActive(false);
		}
	}

	private void Move()
	{
		transform.position += direction * Speed;
	}

	private void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.CompareTag("target"))
		{
			Object.Destroy(collision.gameObject);
		}
		gameObject.SetActive(false);
	}
}
