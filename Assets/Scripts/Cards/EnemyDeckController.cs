using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyDeckController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPlayedCardSlot;

    public List<GameObject> cardsInDeck;
    private List<GameObject> cardsPlayed = new();
    private bool canPlayCard = true;

    public void InitialiseDeck(List<GameObject> cardsToAdd)
    {
        cardsInDeck = cardsToAdd;

        foreach (GameObject card in cardsInDeck)
        {
            card.transform.SetParent(gameObject.transform, false);
        }
    }

    public void PlayRandomCard()
    {
        if (!canPlayCard || cardsInDeck.Count == 0) return;

        canPlayCard = false;
        GameObject randomCard = cardsInDeck[Random.Range(0, cardsInDeck.Count)];

        randomCard.transform.SetParent(enemyPlayedCardSlot.transform);
        Vector3 startCardPosition = randomCard.transform.localPosition;
        Vector3 endCardPosition = Vector3.zero;

        Vector3 deckPosition = transform.localPosition;

        float tweenDuration = 0.15f;
        CanvasGroup canvasGroup = randomCard.GetComponent<CanvasGroup>();

        Sequence sequence = DOTween.Sequence();

        randomCard.GetComponent<CardController>().RemoveFromHand();
        sequence.Append(DOTween.To(() => startCardPosition, x => randomCard.transform.localPosition = x, endCardPosition, tweenDuration).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                randomCard.transform.localPosition = new Vector3(0, -112.5f, 0);
            }));

        sequence.AppendInterval(1f);

        sequence.Append(canvasGroup.DOFade(0f, 0.5f));

        sequence.Append(DOTween.To(() => randomCard.transform.localPosition, x => randomCard.transform.localPosition = x, deckPosition, tweenDuration)
            .OnComplete(() =>
            {
                randomCard.transform.SetParent(transform);
                randomCard.transform.localPosition = deckPosition;
                canvasGroup.alpha = 1f;
                canPlayCard = true;
            })
        );

        cardsPlayed.Add(randomCard);
        cardsInDeck.Remove(randomCard);
    }

    public void ResetCards()
    {
        cardsInDeck.AddRange(cardsPlayed);
        cardsPlayed.Clear();
    }
}
