using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        if (cardsInDeck.Count == 0) return;

        GameObject randomCard = cardsInDeck[Random.Range(0, cardsInDeck.Count)];
        randomCard.SetActive(true);
        hand.AddCardToHandSlot(randomCard);
        cardsInDeck.Remove(randomCard);
    }

    public void ReturnCardToDrawPile(GameObject cardToReturn)
    {
        cardToReturn.SetActive(true);
        cardToReturn.transform.SetParent(gameObject.transform, false);
        cardsInDeck.Add(cardToReturn);
    }

    public void RemoveAllPlayedCards()
    {
        for (int i = buffZone.transform.childCount - 1; i >= 0; --i)
        {
            buffZone.transform.GetChild(i).transform.SetParent(gameObject.transform, false);
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
            int randomIndex = Random.Range(0, transform.childCount);
            Transform randomChild = transform.GetChild(randomIndex);
            Vector3 originalPosition = randomChild.position;

            randomChild.DOMove(originalPosition + shuffleOffset, shuffleDuration).SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(shuffleDuration);

            randomChild.DOMove(originalPosition, shuffleDuration).SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(shuffleDuration);

        }
        shuffling = false;
    }
}
