using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using System;

public class CheckpointController: MonoBehaviour
{

    [Header("ANIMATION")]
    [SerializeField] private Transform moveablePlatform; //Sets On Inspector
    [SerializeField] private Transform[] moveableBridges; //Sets On Inspector
    [SerializeField] private Ease ease;

    [Header("OTHER STUFF")]
    [SerializeField] private TextMeshPro counterText; //Sets On Inspector
    public CheckpointSettings checkpointSettings;

    public int testMaxObstacle;
    private int _currentObstacle;
    private int currentObstacle { get { return _currentObstacle; } set
        {
            _currentObstacle = value;
            UpdateCounterText();
        } }

    public List<ObstacleController> obstacles = new List<ObstacleController>();


    [Header("RANDOM SPAWNS OBSTACLE POINTS")]
    [SerializeField] private Transform[] threeWaySpawn; //Sets On Inspector
    [SerializeField] private Transform[] fourWaySpawn; //Sets On Inspector
    [SerializeField] private Transform[] fiveWaySpawn; //Sets On Inspector

    bool checkpointReached;

    public Task platformTask;
    public Task bridgeTask;

    private void Awake()
    {
        //CreateMyCheckPointSettings();
    }

    private void CreateMyCheckPointSettings()
    {
        checkpointSettings = new CheckpointSettings();
        checkpointSettings.maxNeededObstacle = testMaxObstacle;
        checkpointSettings.checkpointController = this;
    }


    private void Start()
    {
        //CheckpointManager.instance.RegisterMe(checkpointSettings);


        Invoke("UpdateCounterText", 0.1f); // To wait for scripts initializations
    }

    private void UpdateCounterText()
    {
        counterText.text = _currentObstacle.ToString() + "/" + checkpointSettings.maxNeededObstacle.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponentInParent<ObstacleController>())
        {
            currentObstacle++;
            obstacles.Add(other.GetComponentInParent<ObstacleController>());
            SoundManager.instance.PlaySound(Soundlar.CheckpointPickUp);
        }
    }

    public bool CheckpointHasEnoughObstacles()
    {
        if (currentObstacle >= checkpointSettings.maxNeededObstacle)
            return true;
        else
            return false;
    }

    public void ExplodeThisCheckpointsObstacles()
    {
        foreach (var obstacle in obstacles)
        {
            obstacle.ExplodeMe();
        }
    }

    public async void SetAnimationTasks()
    {
        var sequence = DOTween.Sequence();
        platformTask = moveablePlatform.DOLocalMoveY(0f, 1f).SetEase(ease).AsyncWaitForCompletion();
        bridgeTask = sequence.AsyncWaitForCompletion();
        await Task.WhenAll(platformTask);
        sequence.Append(moveableBridges[0].DOLocalRotate(new Vector3(0, 0, 70f), 1f).SetEase(Ease.OutSine)); // Rise Left Bridge
        sequence.Join(moveableBridges[1].DOLocalRotate(new Vector3(0, 0, -70f), 1f).SetEase(Ease.OutSine)); // Rise Right Bridge
    }

    public void PlayerTrigger() //Calls On EventTrigger script
    {
        CheckpointManager.instance.ReachedCheckpoint(this);
        checkpointReached = true;
    }



    #region RandomObstacleSpawnStuff
    public void RandomSpawnObstacles()
    {
        int rndmWays = UnityEngine.Random.Range(0, 3);
        switch (rndmWays)
        {
            case 0:
                ThreeWaySpawn(checkpointSettings.maxNeededObstacle + UnityEngine.Random.Range(5, 15));
                break;
            case 1:
                FourWaySpawn(checkpointSettings.maxNeededObstacle + UnityEngine.Random.Range(5, 15));
                break;
            case 2:
                FiveWaySpawn(checkpointSettings.maxNeededObstacle + UnityEngine.Random.Range(5, 15));
                break;
            default:
                break;
        }
    }

    

    private void ThreeWaySpawn(int obstacleCount)
    {

        GameObject randomObject = ObstacleManager.instance.PickARandomObstacle();

        int obstacleCountOnSpawnPoint = obstacleCount / 3;

        int formationIndex = (checkpointSettings.maxNeededObstacle < 15) ? UnityEngine.Random.Range(1, 5) : UnityEngine.Random.Range(2, 5); //If its more than 15, then the 1 line formation will be too long. This will prevent it to happen.

        for (int a = 0; a < 3; a++)
        {
            Transform[] spawnedObstacles = new Transform[obstacleCountOnSpawnPoint];


            for (int i = 0; i < obstacleCountOnSpawnPoint; i++)
            {
                GameObject obstacle = Instantiate(randomObject);

                spawnedObstacles[i] = obstacle.transform;
            }

            DoFormation(spawnedObstacles, threeWaySpawn[a].position, formationIndex);
        }


    }


    private void FiveWaySpawn(int obstacleCount)
    {

        GameObject randomObject = ObstacleManager.instance.PickARandomObstacle();

        int obstacleCountOnSpawnPoint = obstacleCount / 5;

        int formationIndex = (checkpointSettings.maxNeededObstacle < 15) ? UnityEngine.Random.Range(1, 5) : UnityEngine.Random.Range(2, 5); //If its more than 15, then the 1 line formation will be too long. This will prevent it to happen.

        for (int a = 0; a < 5; a++)
        {
            Transform[] spawnedObstacles = new Transform[obstacleCountOnSpawnPoint];


            for (int i = 0; i < obstacleCountOnSpawnPoint; i++)
            {
                GameObject obstacle = Instantiate(randomObject);

                spawnedObstacles[i] = obstacle.transform;
            }

            DoFormation(spawnedObstacles, fiveWaySpawn[a].position, formationIndex);
        }


    }

    private void FourWaySpawn(int obstacleCount)
    {

        GameObject randomObject = ObstacleManager.instance.PickARandomObstacle();

        int obstacleCountOnSpawnPoint = obstacleCount / 4;

        int formationIndex = (checkpointSettings.maxNeededObstacle < 15) ? UnityEngine.Random.Range(1, 5) : UnityEngine.Random.Range(2, 5); //If its more than 15, then the 1 line formation will be too long. This will prevent it to happen.

        for (int a = 0; a < 4; a++)
        {
            Transform[] spawnedObstacles = new Transform[obstacleCountOnSpawnPoint];


            for (int i = 0; i < obstacleCountOnSpawnPoint; i++)
            {
                GameObject obstacle = Instantiate(randomObject);

                spawnedObstacles[i] = obstacle.transform;
            }

            DoFormation(spawnedObstacles, fourWaySpawn[a].position, formationIndex);
        }


    }

    private void DoFormation(Transform[] obstacleArray, Vector3 position, int column)
    {
        for (int i = 0; i < obstacleArray.Length; i++)
        {
            obstacleArray[i].position = position + CalcFormationPosition(i, column);
        }
    }


    private Vector3 CalcFormationPosition(int index, int columns)
    {
        float posX;
        if (index % columns == 0)
            posX = 0;
        else if (index % columns == 1)
            posX = -GameManager.instance.randomSpawnObstacleSpace * 3 / 4;
        else if (index % columns == 2)
            posX = GameManager.instance.randomSpawnObstacleSpace * 3 / 4;
        else if (index % columns == 3)
            posX = 2 *GameManager.instance.randomSpawnObstacleSpace * 3 / 4;
        else
            posX = (index % columns) * GameManager.instance.randomSpawnObstacleSpace;
        float posZ = (index / columns) * GameManager.instance.randomSpawnObstacleSpace;
        return new Vector3(posX, 0, posZ);

    }
    #endregion

}



[Serializable]
public class CheckpointSettings
{
    public int maxNeededObstacle;
    public CheckpointController checkpointController;
}
