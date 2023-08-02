using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float smoothSpeed = 0.125f;
    private Vector3 offset;

    private void Start()
    {
        // offset = transform.position - player.transform.position;

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
