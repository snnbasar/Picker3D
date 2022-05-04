using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager instance;

    [SerializeField] private GameObject[] obstaclePrefabs;


    private void Awake()
    {
        instance = this;
    }

    

    public void GivePlayersObstaclesCheckpointForce()
    {
        foreach (var obstacle in PlayerManager.instance.GetPlayerController().GetPlayerTrigger().obstaclesThatPlayerMoving)
        {
            if(obstacle != null)
                obstacle.GiveMeForce((PlayerManager.instance.playerController.transform.position + Vector3.back).normalized + Vector3.up * 0.3f, 10f);
        }
    }


    public GameObject PickARandomObstacle()
    {
        return obstaclePrefabs[UnityEngine.Random.Range(0, obstaclePrefabs.Length)];
    }
}
