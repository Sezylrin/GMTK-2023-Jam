using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public List<GameObject> cardsInDeck;
    private HandController hand;

    private void Start()
    {
        hand = FindObjectOfType<HandController>();
    }

    public void InitialiseDeck(List<GameObject> cardsToAdd)
    {
        cardsInDeck = cardsToAdd;
    }
    
    public void Draw()
    {
        if (cardsInDeck.Count == 0) return;

        GameObject randomCard = cardsInDeck[Random.Range(0, cardsInDeck.Count)];
        randomCard.SetActive(true);
        hand.AddCardToHandSlot(randomCard);
        cardsInDeck.Remove(randomCard);
    }

    public void DrawFullHand(int cardsToDraw = 4)
    {
        for (int i = 0; i < cardsToDraw; ++i)
        {
            Draw();
        }
    }

    public void ReturnCardToDrawPile(GameObject cardToReturn)
    {
        cardToReturn.SetActive(true);
        cardsInDeck.Add(cardToReturn);
    }
}
