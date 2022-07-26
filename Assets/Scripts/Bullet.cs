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

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.CompareTag("target"))
		{
			collision.gameObject.SetActive(false);
		}
		gameObject.SetActive(false);
	}
}
