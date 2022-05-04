using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SingleScoreTrigger : MonoBehaviour
{
    public int myScore; //has to be public

    public void SetMyScoreAndMaterial(int score, Material mat)
    {
        //Undo.RegisterCompleteObjectUndo(this, "myScore Update");
        myScore = score;
        GetComponentInChildren<TextMeshPro>().text = myScore.ToString();
        GetComponent<Renderer>().material = mat;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ScoreManager.instance.playerHasTriggered && other.CompareTag("Player"))
        {
            ScoreManager.instance.myScore = myScore;
            ScoreManager.instance.playerHasTriggered = true;
            ScoreManager.instance.DoAfterGettingScore();
        }
    }
}
