using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectiles : MonoBehaviour
{
    [SerializeField] private float shootingCoolDown;
    [SerializeField] private int NumberOfBullets;
    [SerializeField] private bool isFiring = false;

    public GameObject bulletPrefab;
    public Camera camera;

    public List<GameObject> Bullets = new List<GameObject>();
    //Creates a pool of bullets
    void Start()
    {
		for (int i = 0; i < NumberOfBullets; i++)
		{
            Bullets.Add(Instantiate(bulletPrefab));
            Bullets[i].SetActive(false);
		}
    }

    //if space is pressed fire a bullet
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)&& !isFiring)
		{
            Fire();
		}
    }

     private void Fire()
	{
        int ind = FindBullet();
        if (ind>=0)
		{
            isFiring = true;
            Bullets[ind].transform.position = transform.position;
            Bullets[ind].transform.rotation = transform.rotation;
            Bullets[ind].GetComponent<Bullet>().direction = transform.forward;

            Bullets[ind].SetActive(true);
            Invoke(nameof(cancelFiring), shootingCoolDown);
		}
	}
   
    private int FindBullet()
	{
        for (int i = 0; i < NumberOfBullets; i++)
        {
            if (!Bullets[i].activeSelf)
                return i;
        }
        return -1;
    }

    private void cancelFiring()
	{
        isFiring = false;
	}
}
