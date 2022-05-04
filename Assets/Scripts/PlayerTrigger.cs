using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{


    public List<ObstacleController> obstaclesThatPlayerMoving = new List<ObstacleController>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<ObstacleController>())
        {
            obstaclesThatPlayerMoving.Add(other.GetComponentInParent<ObstacleController>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<ObstacleController>())
        {
            obstaclesThatPlayerMoving.Remove(other.GetComponentInParent<ObstacleController>());
        }
    }

    public void ClearList()
    {
        obstaclesThatPlayerMoving.Clear();
    }
}
