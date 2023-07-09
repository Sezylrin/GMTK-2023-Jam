using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DeckController : MonoBehaviour
{
    [SerializeField] private GameObject buffZone;

    public List<GameObject> cardsInDeck;
    private HandController hand;

    private float shuffleDuration = 0.05f;
    private int shuffleCount = 10;
    private Vector3 shuffleOffset = new Vector3(3, 15f, 0);
    private bool shuffling = false;

    private void Start()
    {
        hand = FindObjectOfType<HandController>();
    }

    public void InitialiseDeck(List<GameObject> cardsToAdd)
    {
        cardsInDeck = cardsToAdd;

        foreach (GameObject card in cardsInDeck)
        {
            card.transform.SetParent(gameObject.transform, false);
        }

        ShuffleDeck();
    }

    public void Draw()
    {
        if (cardsInDeck.Count == 0 || shuffling || !hand.HasAvailableSlot()) return;

        GameObject randomCard = cardsInDeck[UnityEngine.Random.Range(0, cardsInDeck.Count)];
        randomCard.SetActive(true);
        randomCard.transform.SetAsLastSibling();
        randomCard.transform.localEulerAngles = new Vector3(0, 180, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(randomCard.transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad))
            .Insert(0.25f, DOTween.Sequence().OnComplete(() => {
                randomCard.GetComponent<CardController>().FlipCardUp();
            }))
            .OnComplete(() => {
                hand.AddCardToHandSlot(randomCard);
                cardsInDeck.Remove(randomCard);
            });
    }

    public void ReturnCardToDrawPile(GameObject cardToReturn)
    {
        cardToReturn.SetActive(true);
        cardToReturn.transform.SetParent(gameObject.transform, false);
        cardToReturn.transform.localPosition = new Vector3(-80, 0, 0);
        cardToReturn.GetComponent<CardController>().FlipCardDown();
        cardsInDeck.Add(cardToReturn);
    }

    public void RemoveAllPlayedCards()
    {
        for (int i = buffZone.transform.childCount - 1; i >= 0; --i)
        {
            GameObject card = buffZone.transform.GetChild(i).gameObject;
            card.transform.SetParent(gameObject.transform, false);
            card.GetComponent<CardController>().FlipCardDown();
            card.transform.localPosition = new Vector3(-80, 0, 0);
        }
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        if (shuffling) return;
        shuffling = true;
        StartCoroutine(ShuffleAnimation());
    }

    public IEnumerator ShuffleAnimation()
    {
        for (int i = 0; i < shuffleCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, transform.childCount);
            Transform randomChild = transform.GetChild(randomIndex);
            Vector3 originalPosition = randomChild.position;

            randomChild.DOMove(originalPosition + shuffleOffset, shuffleDuration).SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(shuffleDuration);

            randomChild.DOMove(originalPosition, shuffleDuration).SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(shuffleDuration);

        }
        shuffling = false;
        hand.DrawFullHand();
    }
}
