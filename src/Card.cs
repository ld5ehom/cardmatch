using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer cardRenderer; // Unity rightside Sprite Renderer

    [SerializeField]
    private Sprite siderealSprite; //change card image 

    [SerializeField]
    private Sprite backSprite;  //card return

    private bool isFlipped = false; //card change check (True = siderealSprite)
    private bool isFlipping = false; 
    private bool isMatched = false; 

    public int cardID;

    public void SetCardID(int id) // sidereal card ID 
    {
        cardID = id;
    }

    public void SetMatched() // match set 
    {
        isMatched = true;
    }

    public void SetSiderealSprite(Sprite sprite)  // siderealSprite Method (image)
    {
        siderealSprite = sprite; 
    }

    public void FlipCard()  //Change card image
    {
        isFlipping = true; //Double click check 

        Vector3 originalScale =  transform.localScale; // scale information
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z); //change scale X

        transform.DOScale(targetScale, 0.2f).OnComplete(() =>
        {
            isFlipped = !isFlipped; //check status

            if (isFlipped){
                cardRenderer.sprite = siderealSprite; //change to sidereal
            } else {
                cardRenderer.sprite = backSprite; // change to backside 
            }

            transform.DOScale(originalScale, 0.2f).OnComplete(() =>
            {
                isFlipping = false;
            });
        });

    }

    void OnMouseDown() //onClick Event
    {
        if(!isFlipping && !isMatched && !isFlipped) // Not flipped + not matched + wait click
        { 
            GameManager.instance.CardClicked(this);
        }
    }
}
