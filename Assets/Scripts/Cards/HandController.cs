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
                Vector3 handSlotPosition = handSlots[i].transform.position;
                Sequence sequence = DOTween.Sequence();
                sequence.Append(card.transform.DOMove(handSlotPosition, 0.5f).SetEase(Ease.InOutQuad))
                    .OnComplete(() =>
                    {
                        card.transform.SetParent(handSlots[i].transform, false);
                        card.transform.localPosition = Vector3.zero;
                        card.GetComponent<CardController>().hoverable = true;
                    });
                handSlotsAvailable[i] = false;
                return;
            }
        }
    }

    public void PlayRandomCard()
    {
        if (!canPlayCard) return;

        List<int> availableCards = new();
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
            //if (GameManager.instance.currentMana < selectedCard.GetComponent<CardController>().card.manaCost)
            //{
                //canPlayCard = true;
                //return;
            //}
            //GameManager.instance.UseMana(selectedCard.GetComponent<CardController>().card.manaCost);

            //BuffManager.instance.CastCard(selectedCard.GetComponent<CardController>().card, true);
            Vector3 startCardPosition = selectedCard.transform.position;
            Vector3 endCardPosition = playedCardSlot.transform.position;

            float tweenDuration = 0.15f;

            Sequence sequence = DOTween.Sequence();

            selectedCard.GetComponent<CardController>().RemoveFromHand();
            sequence.Append(DOTween.To(() => startCardPosition, x => selectedCard.transform.position = x, endCardPosition, tweenDuration).SetEase(Ease.InOutQuad));
            handSlotsAvailable[randomCardSelected] = true;

            switch (selectedCard.GetComponent<CardController>().card.type)
            {
                case CardType.Powerup:
                    MoveCardToBuffZone(selectedCard, sequence);
                    return;
                case CardType.Weapon:
                case CardType.Spell:
                    FadeCardOut(selectedCard, sequence);
                    return;
            }
        }
    }

    private void MoveCardToBuffZone(GameObject selectedCard, Sequence sequence)
    {
        sequence.AppendInterval(1.0f);
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

    private void FadeCardOut(GameObject selectedCard, Sequence sequence)
    {
        sequence.AppendInterval(1.0f);
        sequence.Append(selectedCard.GetComponent<CanvasGroup>().DOFade(0, 1.0f))
            .OnComplete(() =>
            {
                deck.ReturnCardToDrawPile(selectedCard);
                selectedCard.GetComponent<CanvasGroup>().alpha = 1;
                canPlayCard = true;
            });
    }

    public bool HasAvailableSlot()
    {
        foreach (bool slotAvailable in handSlotsAvailable)
        {
            if (slotAvailable)
            {
                return true;
            }
        }
        return false;
    }

    public void DrawFullHand()
    {
        DrawCardRecursive(0);
    }

    private void DrawCardRecursive(int currentHandSize)
    {
        if (currentHandSize >= handSlots.Count)
            return;

        if (handSlotsAvailable[currentHandSize])
        {
            deck.Draw(() => DrawCardRecursive(currentHandSize + 1));
        }
        else
        {
            DrawCardRecursive(currentHandSize + 1);
        }
    }
}
