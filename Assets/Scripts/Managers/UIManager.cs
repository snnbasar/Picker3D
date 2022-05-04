using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private TapToAddForce tapToAddForceObject; //Sets on inspector

    [Header("MENUS")]
    [SerializeField] private GameObject levelCompleteMenu; //Sets on inspector
    [SerializeField] private GameObject levelFailedMenu; //Sets on inspector
    [SerializeField] private GameObject tapToPlay; //Sets on inspector

    [Header("PROGRESSBAR SETTINGS")]
    [SerializeField] private Color progressbarColor;
    [SerializeField] private GameObject progressbarObject; //Sets On Inspector
    [SerializeField] private TextMeshProUGUI curLevelText; //Sets On Inspector
    [SerializeField] private Image[] progressbarImages; //Sets On Inspector
    [SerializeField] private TextMeshProUGUI nextLevelText; //Sets On Inspector

    [Header("GEM COUNTER")]
    public TextMeshProUGUI gemText; //Sets On Inspector
    public Transform gemEffect; //Sets On Inspector


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        EnableTapToAddForceMenu(false);
        BringLevelCompleteMenuOnScreen(false);
        BringLevelFailedMenuOnScreen(false);

        gemEffect.gameObject.SetActive(false);


        tapToPlay.SetActive(!GameManager.instance.GetSettingsData().IsSwichingNextLevel);

        SetProgressBar();

        Invoke("UpdateGemText", 0.1f);
    }

    public void EnableTapToAddForceMenu(bool status)
    {
        tapToAddForceObject.gameObject.SetActive(status);
    }

    public void BringLevelCompleteMenuOnScreen(bool status)
    {
        levelCompleteMenu.SetActive(status);
    }
    public void BringLevelFailedMenuOnScreen(bool status)
    {
        levelFailedMenu.SetActive(status);
    }

    public void BringProgressbarMenuOnScreen(bool status)
    {
        progressbarObject.SetActive(status);
    }
    public void SetLevelTexts(int level)
    {
        curLevelText.text = level.ToString();
        int nextLevel = level + 1;
        nextLevelText.text = nextLevel.ToString();
    }


    private void SetProgressBar()
    {
        BringProgressbarMenuOnScreen(true);
        SetLevelTexts(GameManager.instance.GetSettingsData().playersLevel);
    }

    public void UpdateProgressbarsCheckpoints(int index)
    {
        for (int i = 0; i < index; i++)
        {
            progressbarImages[i].color = progressbarColor;
        }
    }

    private void UpdateGemText()
    {
        gemText.text = ScoreManager.instance.totalScore.ToString();
    }
    public void AddGem()
    {
        gemEffect.gameObject.SetActive(true);

        gemEffect.GetComponent<TextMeshProUGUI>().text = "+" + ScoreManager.instance.myScore;

        gemEffect.DOScale(1f, 1f).From(0.5f);
        gemEffect.DOLocalMove(gemText.transform.localPosition, 1f).From(Camera.main.ViewportToScreenPoint(Vector3.one * 0.5f)).SetEase(Ease.OutBack).OnComplete(() =>
        {
            UpdateGemText();
            gemEffect.gameObject.SetActive(false);
        });
    }
}
