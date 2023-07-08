using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private List<GameObject> handSlots;

    public DeckController deck;
    
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
                GameObject instance = Instantiate(card, handSlots[i].transform);
                instance.transform.localPosition = Vector3.zero;
                handSlotsAvailable[i] = false;
                return;
            }
        }
        Debug.LogError("No available hand slots to add the card.");
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
}
