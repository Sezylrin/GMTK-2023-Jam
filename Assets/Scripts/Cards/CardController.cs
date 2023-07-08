using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float duration = 0.05f;
    private Vector2 originalSize;
    private Vector2 hoverSize;
    private Vector3 originalPosition;
    private RectTransform rectTransform;
    private Tween sizeTween;
    private Tween positionTween;

    private Card card;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalSize = rectTransform.sizeDelta;
        originalPosition = rectTransform.localPosition;
        hoverSize = originalSize * 1.5f;
    }

    public void InstantiateCard(Card card)
    {
        this.card = card;
        gameObject.GetComponent<Image>().sprite = card.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 positionOffset = new Vector3(0, (hoverSize.y - originalSize.y) / 2, 0);

        if (sizeTween == null || !sizeTween.IsActive())
        {
            sizeTween = rectTransform.DOSizeDelta(hoverSize, duration).SetAutoKill(false);
            positionTween = rectTransform.DOLocalMove(originalPosition + positionOffset, duration).SetAutoKill(false);
        }
        else
        {
            sizeTween.PlayForward();
            positionTween.PlayForward();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (sizeTween != null && sizeTween.IsActive())
        {
            sizeTween.PlayBackwards();
            positionTween.PlayBackwards();
        }
    }
}
