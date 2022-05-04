using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    [HideInInspector] public bool IsSwichingNextLevel;


    [Header("OBSTACLE SETTINGS")]
    public bool randomSpawn;
    public int randomSpawnMin;
    public int randomSpawnMax;
    public float randomSpawnObstacleSpace;
    public float explodeAfterTime = 1f;

    [Header("REFERANCES")]
    public SettingsData settingsData;


    private void Awake()
    {
        instance = this;

        GetSettingsData().LoadSavedData();

    }


    private void Start()
    {
        GetLastLevel(); //Load level that player has last played.


        if (GetSettingsData().IsSwichingNextLevel) // If it is then start the game else wait for tap to play
        {
            Invoke("StartGame", 0.01f);
        }

        Application.targetFrameRate = 60;
    }




    private void GetLastLevel()
    {
        int curLevel = GameManager.instance.GetSettingsData().playersLevel;
        int curLevelid = curLevel - 1;

        int loadedSceneId = SceneManager.GetActiveScene().buildIndex;
        if (loadedSceneId != curLevelid && loadedSceneId != NextLevelManager.maxScene)
        {
            if (curLevel > NextLevelManager.maxScene)
                SceneManager.LoadScene(NextLevelManager.maxScene);
            else
                SceneManager.LoadScene(curLevelid);
        }
    }

    public void StartGame()
    {
        MovePlayer(true);

        UIManager.instance.BringProgressbarMenuOnScreen(true);
    }

    public void EndGame()
    {
        MovePlayer(false);
    }

    private void MovePlayer(bool status)
    {
        PlayerManager.instance.GetPlayerController().CanPlayerMove(status);
    }
    public void LevelComplete()
    {
        UIManager.instance.BringLevelCompleteMenuOnScreen(true);
        SoundManager.instance.PlaySound(Soundlar.LevelComplete);

        UIManager.instance.BringProgressbarMenuOnScreen(false);
    }

    public void LevelFailed()
    {
        UIManager.instance.BringLevelFailedMenuOnScreen(true);
        SoundManager.instance.PlaySound(Soundlar.LevelFailed);

        UIManager.instance.BringProgressbarMenuOnScreen(false);
    }

    public void RestartLevel() //Calls On BringLevelFailedMenuOnScreens button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel() //Calls On EventTrigger
    {
        GetSettingsData().IsSwichingNextLevel = true; // So we will know on next scene load
        GetSettingsData().playersLevel++;

        GetSettingsData().xPositionOfPlayerOnNextLevel = PlayerManager.instance.GetPlayerController().transform.position.x; // For smooth transition

        // for first 10 levels load scene that's already created. For more levels reload last scene
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int sceneToLoad = (currentScene < NextLevelManager.maxScene) ? currentScene + 1 : currentScene;
        SceneManager.LoadScene(sceneToLoad);
    }


    public SettingsData GetSettingsData()
    {
        return settingsData;
    }

    public bool IsRandomSpawn()
    {
        return randomSpawn;
    }

}
