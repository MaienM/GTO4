using UnityEngine;

public class CameraRotator : MonoBehaviour 
{
    public void OnJoinedRoom()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            transform.Rotate(Vector3.up, 180, Space.World);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z * -1);
        }
    }
}
