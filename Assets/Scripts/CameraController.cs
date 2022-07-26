using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector2 InputVector;
    [SerializeField] private float MouseSensivityX;
    [SerializeField] private float MouseSensivityY;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position+Vector3.up*2 ;
        LookAt();
    }

    private void LookAt()
	{
        InputVector += new Vector2(-Input.GetAxisRaw("Mouse Y")* MouseSensivityX, Input.GetAxisRaw("Mouse X")*MouseSensivityY);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Mathf.Clamp(InputVector.x,-45,45), InputVector.y, 0),Time.deltaTime*10);
        player.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, InputVector.y, 0), 1.0f);
    }
}
