using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [HideInInspector] public PlayerController playerController;


    


    private void Awake()
    {
        instance = this;

    }




    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public PlayerTrigger GetPlayerTrigger()
    {
        return playerController.GetPlayerTrigger();
    }


    public void SetPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }





    public void MovePlayerToTransform(Transform point)
    {
        GetPlayerController().transform.position = point.position;
    }


    public void AddPlayerForwardForce(float force) 
    {
        GetPlayerController().GetPlayersRigidbody().AddForce(GetPlayerController().transform.forward * force, ForceMode.Impulse);
    }
}
