///----------------------------------------------------------------------------------
///  ShootProjectil.cs
///  Description       : Is attached to BulletSpawnPoint gameObject in the scene.
///						 Creates a list of Bullets to reuse them.
///----------------------------------------------------------------------------------
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
    // Finds an inactinve bullet and sets the position and direction of the bullet
    // based on the gameobject transforms
    private void Fire()
	{
        int ind = FindBullet();
        if (ind>=0)
		{
            isFiring = true;
            Bullets[ind].SetActive(true);
            Bullets[ind].transform.position = transform.position;
            Bullets[ind].transform.rotation = transform.rotation;
            Bullets[ind].GetComponent<Bullet>().direction = camera.transform.forward;
            Invoke(nameof(cancelFiring), shootingCoolDown);
		}
	}
    // Iterates through the list of bullets and find a disabled bullet.
    // retruns -1 if doesn't find anything
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
