using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;


    private CheckpointController lastCheckpoint;


    public List<CheckpointSettings> _checkpoints;
    public List<CheckpointSettings> checkpoints { get { return _checkpoints; } set
        {
            _checkpoints = value;
            UpdateCheckpointList();
        } }




    private int _checkpointCounter;
    public int checkpointCounter { get { return _checkpointCounter; } set
        {
            if (value > 3)
                return;
            _checkpointCounter = value;
        } }





    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            DontDestroyOnLoad(this);
        }

        UpdateCheckpointList();
    }


    private void Start()
    {
        if (GameManager.instance.IsRandomSpawn())
        {
            foreach (var checkpoint in checkpoints)
            {
                checkpoint.maxNeededObstacle = UnityEngine.Random.Range(GameManager.instance.randomSpawnMin, GameManager.instance.randomSpawnMax);

                checkpoint.checkpointController.RandomSpawnObstacles();
            }
        }
    }

    private void UpdateCheckpointList()
    {
        for (int i = 0; i < checkpoints.Count; i++)
        {
            checkpoints[i].checkpointController.checkpointSettings = checkpoints[i];
        }
    }


    public void ReachedCheckpoint(CheckpointController checkpoint)
    {
        checkpointCounter++;
        lastCheckpoint = checkpoint;


        DoCheckPointStuff();
        
    }

    public bool CheckIfChechpointHasEnoughObstacle(CheckpointController checkpoint)
    {
        return checkpoint.CheckpointHasEnoughObstacles();
    }


    private async void DoCheckPointStuff()
    {
        PlayerManager.instance.GetPlayerController().CanPlayerMoveOnVertical(false);
        ObstacleManager.instance.GivePlayersObstaclesCheckpointForce();
        await Task.Delay((int)(GameManager.instance.explodeAfterTime * 1000));


        lastCheckpoint.ExplodeThisCheckpointsObstacles();
        //await Task.Delay(1000);

        if (CheckIfChechpointHasEnoughObstacle(lastCheckpoint))
        {
            //You Shall Pass

            lastCheckpoint.SetAnimationTasks();

            var tasks = new List<Task>();
            tasks.Add(lastCheckpoint.platformTask);
            tasks.Add(lastCheckpoint.bridgeTask);

            await Task.WhenAll(tasks);

            PlayerManager.instance.GetPlayerController().CanPlayerMoveOnVertical(true);

            UIManager.instance.UpdateProgressbarsCheckpoints(checkpointCounter);

        }
        else
        {
            //You Shall NOT Pass

            GameManager.instance.LevelFailed();
        }
    }
    
    public void RegisterMe(CheckpointSettings checkpointSettings)
    {
        checkpoints.Add(checkpointSettings);
    }
    
}
