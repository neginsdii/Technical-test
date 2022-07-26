///----------------------------------------------------------------------------------
///  TeleportPlayer.cs
///  Description       : Teleports the player to the point there is a block game object.
///                      Spawns a sphere game object at the end of teleporter.
///                      Contains a function for destorying the spawned objects once the 
///                      remove button is pressed.
///                      Contains a function to change the color of the spawned objects
///                      upon pressing the change color button
///----------------------------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TeleportPlayer : MonoBehaviour
{
    [Header("Teleporting")]
    [SerializeField] private float teleportingCoolDown;
    public GameObject camera;
    public GameObject hitPoint;
    private bool Teleporting = false;
    private bool IsBlocked;

    [Header("Spawned object")]
    [SerializeField] private float destroydelayTime;
    public GameObject SpawnObjectPrefab;
    public Transform SpawnedObjectParent;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    [Header("UI references")]
    public GameObject ChangeColorButton;
    public GameObject DestroySpawnedObjectsButton;
    public TextMeshProUGUI pointedObject;
    public Image Pointer;
    void Start()
    {
        ChangeColorButton.GetComponent<Button>().onClick.AddListener(ChangeSpawnedObjectsColor);
        DestroySpawnedObjectsButton.GetComponent<Button>().onClick.AddListener(RemoveSpawnedObjectsColor);

    }

    //Teleports the player. Sets the x and z components of the velocity to zero.
    //Removes all the destoryed objects from the spawnedObject list.
    void Update()
    {
        Teleport();

        GetComponent<Rigidbody>().velocity =new Vector3(0, GetComponent<Rigidbody>().velocity.y,0);

        spawnedObjects.RemoveAll(obj => obj == null);

    }
    //Teleporting player if the ray hits a game object with the "Block" tag and mouse right button is pressed.
    //if the game object is a spawned obejct with a "target" and mouse right button is pressed destroy that object.
    private void Teleport()
    {
        RaycastHit hit;
        hitPoint.GetComponent<Renderer>().material.color = Color.clear;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit,200))
		{
           
            GameObject tmp = hit.collider.gameObject;
            if(Input.GetKeyDown(KeyCode.Mouse0))
			{
                pointedObject.text = hit.collider.gameObject.name;
			}
            if (tmp.CompareTag("target"))
            {
                Pointer.color = Color.yellow;

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (spawnedObjects.Contains(tmp))
                    {
                        spawnedObjects.Remove(tmp);

                        StartCoroutine(DestroyPointedObject(destroydelayTime, tmp));

                    }
                }
            }
            else if (tmp.CompareTag("Block"))

            {
                Pointer.color = Color.green;
                hitPoint.GetComponent<Renderer>().material.color = Color.green;

                hitPoint.transform.position = new Vector3(hit.point.x, tmp.transform.position.y + tmp.transform.localScale.y / 2, hit.point.z);
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Teleporting = true;
                    Invoke(nameof(CancelTeleporting), teleportingCoolDown);
                    transform.position = new Vector3(hit.point.x, tmp.transform.position.y + transform.localScale.y + 1, hit.point.z);

                    spawnedObjects.Add(Instantiate(SpawnObjectPrefab, transform.position + Vector3.up * 5, Quaternion.identity, SpawnedObjectParent));
                    spawnedObjects[spawnedObjects.Count - 1].GetComponent<Renderer>().material.color = Random.ColorHSV();

                }
            }
            else
            {
                Pointer.color = Color.red;

            }
		}
        else
		{
            Pointer.color = Color.red;
     

        }
    }
    //Delays destroying the spawned object for a given time after it was pointed by the player
    IEnumerator DestroyPointedObject(float time, GameObject tmp)
    {
        yield return new WaitForSeconds(time);

        Object.Destroy(tmp);
    }
    //Sets teleporting to false after a given time so the player can teleport again

    private void CancelTeleporting()
	{
        Teleporting = false;
	}
    // Is called when the change color button is pressed
    private void ChangeSpawnedObjectsColor()
	{
       
            foreach (var obj in spawnedObjects)
		{
         
            obj.GetComponent<Renderer>().material.color = Random.ColorHSV();
		}
	}

    // Is called when the Remove objects button is pressed
    private void RemoveSpawnedObjectsColor()
    {
        for(int i=0;i<spawnedObjects.Count;i++)
        { 
            Object.Destroy(spawnedObjects[i]);
        
        }
        spawnedObjects.Clear();
    }


}
