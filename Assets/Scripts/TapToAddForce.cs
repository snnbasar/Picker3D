using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapToAddForce : MonoBehaviour, IPointerDownHandler
{
    [Header("PHYSICS SETTINGS")]
    [SerializeField] private float force;
    [Header("BAR SETTINGS")]
    [SerializeField] private int maximum, current;
    [SerializeField] private Image fill;
    [Header("ANIMATION SETTINGS")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float scale, duration;
    [SerializeField] private Ease ease;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        DoTextTween();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerManager.instance.AddPlayerForwardForce(force);
        DoTextTween();
    }

    private void Update()
    {
        current = (int)PlayerManager.instance.GetPlayerController().GetPlayersRigidbody().velocity.magnitude;
        GetCurrentFill();
    }

    private void DoTextTween()
    {
        text.DOKill();
        text.transform.DOScale(new Vector3(scale, scale, scale), duration).From(Vector3.one).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }

    void GetCurrentFill()
    {
        if (current > maximum)
            return;
        float fillAmount = (float)current / (float)maximum;
        fill.fillAmount = fillAmount;
    }
}
