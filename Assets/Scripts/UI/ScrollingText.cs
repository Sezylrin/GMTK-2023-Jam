using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed = 10f;  
    public float loopPosition = -40f; 
    private TMP_Text textMesh;
    private RectTransform textRectTransform;
    private float textWidth;
    private float currentPosition;

    private void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        textRectTransform = textMesh.rectTransform;

        textWidth = textMesh.GetPreferredValues().x; 
        currentPosition = textRectTransform.anchoredPosition.x;
    }

    private void Update()
    {
        currentPosition -= scrollSpeed * Time.deltaTime;

        if (currentPosition < loopPosition)
        {
            currentPosition = textWidth;

            Vector2 anchoredPosition = textRectTransform.anchoredPosition;
            anchoredPosition.x = currentPosition;
            textRectTransform.anchoredPosition = anchoredPosition;
        }
        Vector2 newPosition = textRectTransform.anchoredPosition;
        newPosition.x = currentPosition;
        textRectTransform.anchoredPosition = newPosition;
    }
}
