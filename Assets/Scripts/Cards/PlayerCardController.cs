using System.Collections.Generic;
using UnityEngine;

public class PlayerCardController : MonoBehaviour
{
    [SerializeField] private List<Card> cardsInDeck;
    [SerializeField] private GameObject cardPrefab;

    private DeckController deck;

    private void Start()
    {
        deck = FindObjectOfType<DeckController>();

        List<GameObject> cardsToAdd = new();

        foreach (Card card in cardsInDeck)
        {
            GameObject newCard = Instantiate(cardPrefab);
            newCard.GetComponent<CardController>().InstantiateCard(card);
            cardsToAdd.Add(newCard);
        }

        deck.InitialiseDeck(cardsToAdd);
    }
}
