using UnityEngine;

public class HomeScript : Photon.MonoBehaviour 
{
    public void OnDie()
    {
        Utils.gameController.IsEnded = true;
        Utils.gameController.IsWinner = !photonView.isMine;
    }
}
