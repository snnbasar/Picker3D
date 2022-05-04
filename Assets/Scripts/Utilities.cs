using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    
    public static async void SetActiveAfterTime(bool status, GameObject item, float timeToDoAction)
    {
        await Task.Delay((int)timeToDoAction * 1000);
        item.SetActive(status);
    }


    public static void DestroyItemAfterTime(GameObject item, float timeToDestroy)
    {
        Destroy(item, timeToDestroy);
    }
}
