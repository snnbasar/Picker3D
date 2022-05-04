using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SettingsItem")]
public class SettingsData : ScriptableObject
{

    public bool IsSwichingNextLevel;
    public float xPositionOfPlayerOnNextLevel;


    public Material previousGround;
    public Material previousBridge;
    public Material previousRamp;


    private int _playersLevel;
    public int playersLevel
    {
        get
        {
            if (_playersLevel == 0)
            {
                _playersLevel += 1;
                return _playersLevel;
            }
            else
                return _playersLevel;

        }
        set
        {
            _playersLevel = value;
            PlayerPrefs.SetInt("level", _playersLevel);
        }
    }


    private int _totalScore;
    public int totalScore { get { return _totalScore; } set
        {
            _totalScore = value;
            PlayerPrefs.SetInt("totalScore", _totalScore);
        } }

    public event Action OnSettingsChanged;



    public void ChangeSettings() //todo
    {
        OnSettingsChanged?.Invoke();
    }

    public void LoadSavedData()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            playersLevel = PlayerPrefs.GetInt("level");
        }

        if (PlayerPrefs.HasKey("totalScore"))
        {
            totalScore = PlayerPrefs.GetInt("totalScore");
        }
    }
}
