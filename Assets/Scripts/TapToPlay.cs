using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TapToPlay : MonoBehaviour, IPointerDownHandler
{
    [Header("ANIMATION SETTINGS")]
    private TextMeshProUGUI text;
    public float scale;
    public float duration;
    public Ease ease;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.transform.DOScale(new Vector3(scale, scale, scale), duration).From(Vector3.one).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.StartGame();
        this.gameObject.SetActive(false);
    }
}
