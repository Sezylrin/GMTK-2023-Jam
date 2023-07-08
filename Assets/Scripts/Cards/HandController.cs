using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandController : MonoBehaviour
{
    [SerializeField] private List<GameObject> handSlots;
    [SerializeField] private GameObject playedCardSlot;

    private DeckController deck;
    private List<bool> handSlotsAvailable;

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
            int randomCardSelected = availableCards[UnityEngine.Random.Range(0, availableCards.Count)];
            GameObject selectedCard = handSlots[randomCardSelected].transform.GetChild(0).gameObject;

            Vector3 startCardPosition = selectedCard.transform.position;
            Vector3 endCardPosition = playedCardSlot.transform.position;

            float tweenDuration = 0.15f;

            DOTween.To(() => startCardPosition, x => selectedCard.transform.position = x, endCardPosition, tweenDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    selectedCard.transform.SetParent(playedCardSlot.transform, false);
                    selectedCard.transform.localPosition = Vector3.zero;
                    selectedCard.GetComponent<CardController>().ResetCard();
                    selectedCard.GetComponent<CardController>().hoverable = false;
                    handSlotsAvailable[randomCardSelected] = true;
                });
        }
    }

    public void RemovePlayedCard()
    {

    }
}
