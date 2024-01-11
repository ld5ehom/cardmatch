using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //TextMeshPro
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Card> allCards;

    private Card flippedCard; // flipped card data 뒤집힌 카드 정보 저장하기
    private bool isFlipping = false; // waiting card onclick 카드 클릭 시 중복클릭 방지 

    [SerializeField]
    private Slider timeoutSlider; //Unity Engine UI

    [SerializeField]
    private TextMeshProUGUI timeoutText;  // 60 time out text 

    [SerializeField]
    private TextMeshProUGUI gameOverText;

    [SerializeField]
    private GameObject gameOverPanel; // game over panel 
    private bool isGameOver = false;

    [SerializeField]
    private float timeLimit = 60f; // time limit 60s 
    private float currentTime;
    private int totalMatches = 9;
    private int matchesFound = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Board board = FindObjectOfType<Board>();
        allCards = board.GetCards();

        currentTime = timeLimit; //60 sec bar
        SetCurrentTimeText(); // 60 sec text 
        StartCoroutine("FlipAllCardsRoutine"); //open all card

    }

    void SetCurrentTimeText()
    {
        int timeSec = Mathf.CeilToInt(currentTime);
        timeoutText.SetText(timeSec.ToString());
    }

    IEnumerator FlipAllCardsRoutine() // all cards open 
    {
        isFlipping = true;
        yield return new WaitForSeconds(0.5f); //wait
        FlipAllCards(); // open
        yield return new WaitForSeconds(3f); // wait 3 sec 
        FlipAllCards(); // backside
        yield return new WaitForSeconds(0.5f); //wait
        isFlipping = false;

        yield return StartCoroutine("CountDownTimerRoutine"); //timeout start 
    }

    IEnumerator CountDownTimerRoutine() //Timeout slider 
    {
        while(currentTime > 0) // over 0 sec 
        {
            currentTime -= Time.deltaTime;
            timeoutSlider.value = currentTime / timeLimit;
            SetCurrentTimeText(); //Timeout Text 
            yield return null;
        }
        GameOver(false); //Game Over...
    }

    void FlipAllCards() // Flip
    {
        foreach (Card card in allCards)
        {
            card.FlipCard();
        }
    }

    public void CardClicked(Card card) // flip
    {
        if(isFlipping || isGameOver)  // Flipping or Game end 
        {
            return;
        }
        card.FlipCard();

        if (flippedCard == null) //first card 
        {
            flippedCard = card; //input card data 
        } else {  // check match 
            StartCoroutine(CheckMatchRoutine(flippedCard, card));
        }
    }

    IEnumerator CheckMatchRoutine(Card card1, Card card2)
    {
        isFlipping = true; // wait double click 

        if(card1.cardID == card2.cardID)  //same card 
        {
            card1.SetMatched();
            card2.SetMatched();
            matchesFound++;

            if(matchesFound == totalMatches)
            {
                GameOver(true); // Win!
            }
        } else {  // diff card 
            yield return new WaitForSeconds(0.5f);
            card1.FlipCard();
            card2.FlipCard();
            yield return new WaitForSeconds(0.5f);
        }
        isFlipping = false;
        flippedCard = null; // reset flippedcard list 
    }

    void GameOver(bool success)
    {
        if(isGameOver == false)
        {
            isGameOver = true;
            StopCoroutine("CountDownTimerRoutine");

            if (success)
            {
                gameOverText.SetText("Great Job!");
            } else{
                gameOverText.SetText("GAME OVER");
            }
            
            Invoke("ShowGameOverPanel", 1f); 
        }
    }

    void ShowGameOverPanel() // gameover panel setting 
    {
        gameOverPanel.SetActive(true);
    }

    public void Restart()  //restart
    {
        SceneManager.LoadScene("SampleScene");
    }
}
