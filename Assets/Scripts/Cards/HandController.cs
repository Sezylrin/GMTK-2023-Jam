using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandController : MonoBehaviour
{
    [SerializeField] private List<GameObject> handSlots;
    [SerializeField] private GameObject playedCardSlot;
    [SerializeField] private GameObject buffZone;

    private DeckController deck;
    private List<bool> handSlotsAvailable;
    private bool canPlayCard = true;

    private void Awake()
    {
        handSlotsAvailable = new List<bool>(handSlots.Count);

        for (int i = 0; i < handSlots.Count; i++)
        {
            handSlotsAvailable.Add(true);
        }
    }

    private void Start()
    {
        deck = FindObjectOfType<DeckController>();
    }

    public void AddCardToHandSlot(GameObject card)
    {
        for (int i = 0; i < handSlotsAvailable.Count; i++)
        {
            if (handSlotsAvailable[i])
            {
                card.GetComponent<CardController>().hoverable = true;
                card.transform.SetParent(handSlots[i].transform, false);
                card.transform.localPosition = Vector3.zero;
                handSlotsAvailable[i] = false;
                return;
            }
        }
    }

    public void DiscardHand()
    {
        for (int i = 0; i < handSlotsAvailable.Count; i++)
        {
            if (!handSlotsAvailable[i])
            {
                GameObject cardToDisable = handSlots[i].transform.GetChild(0).gameObject;
                deck.ReturnCardToDrawPile(cardToDisable);
                cardToDisable.SetActive(false);
                handSlotsAvailable[i] = true;
            }
        }
    }

    public void PlayRandomCard()
    {
        if (!canPlayCard) return;

        List<int> availableCards = new List<int>();
        for (int i = 0; i < handSlotsAvailable.Count; i++)
        {
            if (!handSlotsAvailable[i])
            {
                availableCards.Add(i);
            }
        }

        if (availableCards.Count > 0)
        {
            canPlayCard = false;
            int randomCardSelected = availableCards[UnityEngine.Random.Range(0, availableCards.Count)];
            GameObject selectedCard = handSlots[randomCardSelected].transform.GetChild(0).gameObject;

            Vector3 startCardPosition = selectedCard.transform.position;
            Vector3 endCardPosition = playedCardSlot.transform.position;

            float tweenDuration = 0.15f;

            Sequence sequence = DOTween.Sequence();

            selectedCard.GetComponent<CardController>().RemoveFromHand();
            sequence.Append(DOTween.To(() => startCardPosition, x => selectedCard.transform.position = x, endCardPosition, tweenDuration).SetEase(Ease.InOutQuad));
            handSlotsAvailable[randomCardSelected] = true;

            sequence.AppendInterval(3.0f);
            sequence.Append(selectedCard.GetComponent<CanvasGroup>().DOFade(0, 1.0f));
            sequence.AppendCallback(() =>
            {
                selectedCard.transform.SetParent(buffZone.transform);
                selectedCard.transform.localPosition = Vector3.zero;
            });
            sequence.Append(selectedCard.GetComponent<CanvasGroup>().DOFade(1, 1.0f))
                .OnComplete(() =>
                {
                    sequence.Kill();
                    canPlayCard = true;
                });
        }
    }
}
