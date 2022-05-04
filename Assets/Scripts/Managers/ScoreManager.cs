using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("SETTINGS TO CREATE SCORE PART")]
    [SerializeField] private GameObject triggerPrefab;
    [SerializeField] private int scoreMultiplayer;
    [SerializeField] private float triggerScaleX;
    [SerializeField] private float triggerScaleY;
    [SerializeField] private Transform plane;
    [SerializeField] private Material[] materials;
    [SerializeField] private int triggerCount;
    public List<GameObject> triggers = new List<GameObject>(); //has to be public


    [HideInInspector] public bool playerHasTriggered;

    private int _myScore;
    public int myScore { get { return _myScore; } set
        {
            _myScore = value;
            totalScore += _myScore;
            GameManager.instance.GetSettingsData().totalScore = this.totalScore;
            UIManager.instance.AddGem();
        } }

    public int totalScore;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        totalScore = GameManager.instance.GetSettingsData().totalScore;
    }


    [ContextMenu("Create")]
    public void CreateTriggers()
    {
        //Undo.RegisterCompleteObjectUndo(this, "Create Triggers");
        float planelenght = plane.lossyScale.z;
        Vector3 startPos = new Vector3(plane.position.x, plane.position.y, plane.position.z - planelenght / 2);

        float triggerZScale = planelenght / triggerCount;

        for (int i = 0; i < triggerCount; i++)
        {
            GameObject trigger = Instantiate(triggerPrefab);
            trigger.transform.localScale = new Vector3(triggerScaleX, triggerScaleY, triggerZScale);

            trigger.transform.position = startPos + new Vector3(0, plane.lossyScale.y / 2, triggerZScale * i + triggerZScale / 2);
            trigger.transform.parent = plane;

            trigger.GetComponent<SingleScoreTrigger>().SetMyScoreAndMaterial(scoreMultiplayer + scoreMultiplayer * i, materials[Random.Range(0, materials.Length)]);

            triggers.Add(trigger);
        }

    }

    [ContextMenu("Destroy")]
    public void DeleteTriggers()
    {
        foreach (var trigger in triggers)
        {
            DestroyImmediate(trigger);
        }
        triggers.Clear();
    }


    public void DoAfterGettingScore()
    {
        NextLevelManager.instance.DoAfterScoreAnimation();
    }


    
}
