using System.Collections.Generic;
using UnityEngine;

public class PlayerCardController : MonoBehaviour
{
    [SerializeField] private List<Card> cardsInDeck;
    [SerializeField] private GameObject cardPrefab;

    private DeckController deck;
    private HandController hand;

    private void Start()
    {
        deck = FindObjectOfType<DeckController>();
        hand = FindObjectOfType<HandController>();

        List<GameObject> cardsToAdd = new();

        foreach (Card card in cardsInDeck)
        {
            GameObject newCard = Instantiate(cardPrefab);
            newCard.GetComponent<CardController>().InstantiateCard(card);
            cardsToAdd.Add(newCard);
        }

        deck.InitialiseDeck(cardsToAdd);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            deck.Draw();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            hand.DiscardHand();
        }
    }
}
