using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text manaCost;
    [SerializeField] private TMP_Text health;
    [SerializeField] private TMP_Text attack;
    [SerializeField] private GameObject attackIcon;
    [SerializeField] private GameObject healthIcon;

    private Vector2 originalSize;
    private Vector2 hoverSize;
    private Vector3 originalPosition;
    private RectTransform rectTransform;
    private Vector3 scale = new Vector3(1.75f, 1.75f, 1.75f);
    private float originalCardNameFontSize, originalDescriptionFontSize, originalManaCostFontSize, originalHealthFontSize, originalAttackFontSize;

    private Card card;
    public bool hoverable = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalSize = rectTransform.sizeDelta;
        originalPosition = rectTransform.localPosition;
        hoverSize = originalSize * 1.75f;

        originalCardNameFontSize = cardName.fontSize;
        originalDescriptionFontSize = description.fontSize;
        originalManaCostFontSize = manaCost.fontSize;
        originalHealthFontSize = health.fontSize;
        originalAttackFontSize = attack.fontSize;
    }

    public void InstantiateCard(Card card)
    {
        this.card = card;
        gameObject.GetComponent<Image>().sprite = card.sprite;
        cardName.text = card.name;
        description.text = card.description;
        manaCost.text = card.manaCost.ToString();

        switch (card.type)
        {
            case CardType.Weapon:
                healthIcon.SetActive(false);
                attack.text = card.attack.ToString();
                break;
            default:
                healthIcon.SetActive(false);
                attackIcon.SetActive(false);
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hoverable) return;

        EnlargeCard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!hoverable) return;

        ResetCard();
    }

    public void EnlargeCard()
    {
        rectTransform.sizeDelta = hoverSize;
        rectTransform.localPosition = originalPosition + new Vector3(0, (hoverSize.y - originalSize.y) / 2, 0);

        cardName.fontSize = originalCardNameFontSize * 1.75f;
        description.fontSize = originalDescriptionFontSize * 1.75f;
        manaCost.fontSize = originalManaCostFontSize * 1.75f;
        health.fontSize = originalHealthFontSize * 1.75f;
        attack.fontSize = originalAttackFontSize * 1.75f;

        attackIcon.transform.localScale = scale;
        healthIcon.transform.localScale = scale;
    }

    public void ResetCard()
    {
        rectTransform.sizeDelta = originalSize;
        rectTransform.localPosition = originalPosition;

        cardName.fontSize = originalCardNameFontSize;
        description.fontSize = originalDescriptionFontSize;
        manaCost.fontSize = originalManaCostFontSize;
        health.fontSize = originalHealthFontSize;
        attack.fontSize = originalAttackFontSize;

        attackIcon.transform.localScale = Vector3.one;
        healthIcon.transform.localScale = Vector3.one;
    }

    public void RemoveFromHand()
    {
        ResetCard();
        hoverable = false;
    }
}