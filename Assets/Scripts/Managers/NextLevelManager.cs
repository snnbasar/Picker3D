using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class NextLevelManager : MonoBehaviour
{
    public static NextLevelManager instance;


    [Header("SPAWN POINTS")]  //Sets On Inspector
    [SerializeField] private Transform NormalSpawnPoint;
    [SerializeField] private Transform NextLevelSpawnPoint;


    [Header("NEXT LEVELS MOVEABLE BRIDGES")]
    [SerializeField] private Transform[] moveableBridges; //Sets On Inspector


    public static int maxScene = 10; //Max scene until reaching to endless scene

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Invoke("SetPlayerStartPosition", 0.01f); // It needs to wait for playerControl to Register itself to PlayerManager . Can't use Awake because PlayerManagers instance sets on awake.
    }

    public void DoRampMovement() //calls on ramp trigger event
    {
        PlayerManager.instance.GetPlayerController().SwitchMovementToForward(true);
        UIManager.instance.EnableTapToAddForceMenu(true);
    }


    private void SetPlayerStartPosition() //Invokes On Start
    {

        if (CheckIfNextLevelLoading())
        {
            Vector3 spawnPoint = new Vector3(GameManager.instance.GetSettingsData().xPositionOfPlayerOnNextLevel, NextLevelSpawnPoint.position.y, NextLevelSpawnPoint.position.z);
            NextLevelSpawnPoint.position = spawnPoint;
            PlayerManager.instance.MovePlayerToTransform(NextLevelSpawnPoint);
            GameManager.instance.GetSettingsData().IsSwichingNextLevel = false;

        }
        else
        {
            PlayerManager.instance.MovePlayerToTransform(NormalSpawnPoint);
        }
    }

    public bool CheckIfNextLevelLoading()
    {
        if (GameManager.instance.GetSettingsData().IsSwichingNextLevel)
            return true;
        else
            return false;
    }

    public void EndOfTheRamp() //Calls On EventTrigger
    {
        PlayerManager.instance.GetPlayerController().CanPlayerDoForwardMove(false);
    }

    public async void DoAfterScoreAnimation()
    {
        PlayerManager.instance.GetPlayerController().GetPlayersRigidbody().velocity = Vector3.zero;
        await Task.Delay(1500);
        PlayerManager.instance.GetPlayerController().GetPlayersRigidbody().isKinematic = true;

        var seq = DOTween.Sequence();
        seq.Join(PlayerManager.instance.GetPlayerController().transform.DORotate(Vector3.zero, 3f));
        seq.Join(PlayerManager.instance.GetPlayerController().transform.DOMove(NormalSpawnPoint.position, 3f));
        seq.Insert(0.5f, moveableBridges[0].DOLocalRotate(new Vector3(0, 0, 70f), 1f).SetEase(Ease.OutSine)); // Rise Left Bridge
        seq.Insert(0.5f, moveableBridges[1].DOLocalRotate(new Vector3(0, 0, -70f), 1f).SetEase(Ease.OutSine)); // Rise Right Bridge
        seq.OnComplete(() => {
            PlayerManager.instance.GetPlayerController().GetPlayersRigidbody().isKinematic = false;
            //PlayerManager.instance.GetPlayerController().SwitchMovementToForward(false);
            GameManager.instance.LevelComplete();
        });
    }

}
