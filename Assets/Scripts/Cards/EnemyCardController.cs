using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardController : MonoBehaviour
{
    [SerializeField] private List<Card> cardsInDeck;
    [SerializeField] private GameObject cardPrefab;

    private EnemyDeckController deck;

    private void Start()
    {
        deck = FindObjectOfType<EnemyDeckController>();

        List<GameObject> cardsToAdd = new();

        foreach (Card card in cardsInDeck)
        {
            GameObject newCard = Instantiate(cardPrefab);
            newCard.GetComponent<CardController>().InstantiateCard(card, true);
            newCard.GetComponent<CardController>().FlipCardUp();
            cardsToAdd.Add(newCard);
        }

        deck.InitialiseDeck(cardsToAdd);
    }
}