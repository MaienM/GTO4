using UnityEngine;

public class CameraController : MonoBehaviour 
{
    public Camera camera;

    public void Start()
    {
        camera = GetComponent<Camera>();
        transform.position = new Vector3(transform.position.x, Utils.levelBuilder.size * -3f, -5 + Utils.levelBuilder.size * -1.5f);
    }

    public void Update()
    {
        Vector3 position = camera.transform.position;

        float step = PhotonNetwork.isMasterClient ? 0.4f : -0.4f;
        if (Input.GetKey(KeyCode.A))
        {
            position.x = position.x - step;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            position.x = position.x + step;
        }

        if (Input.GetKey(KeyCode.W))
        {
            position.z = position.z + step;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            position.z = position.z - step;
        }

        position.y -= Input.GetAxis("Mouse ScrollWheel") * 10;

        camera.transform.position = position;
    }
}
