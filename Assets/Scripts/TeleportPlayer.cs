using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TeleportPlayer : MonoBehaviour
{
    private bool IsBlocked;
    private bool Teleporting = false;
  
    [SerializeField] private float teleportingCoolDown;
    public GameObject camera;
    public Image Pointer;
    public GameObject hitPoint;
    public GameObject SpawnObjectPrefab;
    public Transform SpawnedObjectParent;
  
    [SerializeField] private float destroydelayTime;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    public GameObject ChangeColorButton;
    public GameObject DestroySpawnedObjectsButton;
    public TextMeshProUGUI pointedObject;
    void Start()
    {
        ChangeColorButton.GetComponent<Button>().onClick.AddListener(ChangeSpawnedObjectsColor);
        DestroySpawnedObjectsButton.GetComponent<Button>().onClick.AddListener(RemoveSpawnedObjectsColor);

    }


    void Update()
    {
        Teleport();

        GetComponent<Rigidbody>().velocity =new Vector3(0, GetComponent<Rigidbody>().velocity.y,0);

     
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
            if (tmp.CompareTag("target") && Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (spawnedObjects.Contains(tmp))
                {
                    spawnedObjects.Remove(tmp);

                    StartCoroutine(DestroyPointedObject(destroydelayTime, tmp));
                    Pointer.color = Color.yellow;
               

                }
            }
            else if(tmp.CompareTag("Block"))

            {
                Pointer.color = Color.green;
                hitPoint.GetComponent<Renderer>().material.color = Color.green;
      
                hitPoint.transform.position = new Vector3(hit.point.x, tmp.transform.position.y+ tmp.transform.localScale.y/2, hit.point.z);
                if (Input.GetKeyDown(KeyCode.Mouse1) )
                {
                    Teleporting = true;
                    Invoke(nameof(CancelTeleporting), teleportingCoolDown);
                    transform.position = new Vector3 (hit.point.x , tmp.transform.position.y +transform.localScale.y, hit.point.z);
                  
                    spawnedObjects.Add(Instantiate(SpawnObjectPrefab, SpawnedObjectParent));
                    spawnedObjects[spawnedObjects.Count - 1].GetComponent<Renderer>().material.color = Random.ColorHSV();
                    spawnedObjects[spawnedObjects.Count - 1].transform.position = transform.position+Vector3.up;

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
    
    IEnumerator DestroyPointedObject(float time, GameObject tmp)
    {
        yield return new WaitForSeconds(time);

        Object.Destroy(tmp);
    }
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
