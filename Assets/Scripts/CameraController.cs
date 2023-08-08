using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private float smoothSpeed = 0.125f;
    private Vector3 offset;

    private int startGoing = 10 ;
    private float cameraSpeed = 0.01f ;



    private void Start()
    {
        // offset = transform.position - player.transform.position;

    }

    private void Update(){
        if (transform.position.y > startGoing) transform.position += new Vector3(0, cameraSpeed, 0);

    }

    private void LateUpdate()
    {
        if (player.transform.position.y > transform.position.y)
        {
            Vector3 newPosition = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition + offset, smoothSpeed);
        }
    }
}
