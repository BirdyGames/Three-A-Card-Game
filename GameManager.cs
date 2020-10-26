using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Jobs;

public class GameManager : MonoBehaviour
{
    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public Sprite[] cardFaces;
    public GameObject cardPF;

    public Transform[] playerBottomPos, playerTopPos, enemyBottomPos, enemyTopPos;
    public Transform playerHandPos, enemyHandPos, drawPilePos;

    private List<string> deck;
    public List<string> drawPile;

    [Header("Hands")]
    public List<string> playerBottomRow;
    public List<string> playerTopRow;
    public List<string> playerHand;

    public List<string> enemyBottomRow;
    public List<string> enemyTopRow;
    public List<string> enemyHand;

    public float timeToWait = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        PlayCards();
        DealAllCards();
    }

    private void PlayCards()
    {
        deck = GenerateDeck();
        Shuffle(deck);
        AddToDrawPile();

        foreach (string card in deck)
        {
            print(card);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                newDeck.Add(v + s);
            }
        }

        return newDeck;
    }

    void AddToDrawPile()
    {
        float zOffset = 0.03f;

        foreach (string card in deck)
        {
            GameObject newCard = Instantiate(cardPF, new Vector3(drawPilePos.position.x, drawPilePos.position.y, drawPilePos.position.z + zOffset), Quaternion.identity);
            newCard.name = card;
            newCard.transform.parent = drawPilePos.transform;

            zOffset += 0.03f;
            drawPile.Add(card);
        }

        deck.Clear();
    }

    IEnumerator DealCards(List<string> hand, int numberToDeal, Transform[] target)
    {
        GameObject cardToMove;
        
        for (int i = 0; i < numberToDeal; i++)
        {
            string cardToDeal = drawPile[0];
            hand.Add(cardToDeal);
            cardToMove = GameObject.Find(cardToDeal);
            cardToMove.transform.position = target[i].position;
            cardToMove.transform.parent = target[i].transform;

            if ((hand == playerTopRow) || (hand == enemyTopRow))
            {
                cardToMove.GetComponent<Selectable>().showFace = true;
                cardToMove.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }

            drawPile.Remove(cardToDeal);
            yield return new WaitForSeconds(timeToWait);
        }
        
    }

    IEnumerator DealCardsToHand(List<string> hand, int numberToDeal, Transform target)
    {
        GameObject cardToMove;
        float xOffset = 0f;

        for (int i = 0; i < numberToDeal; i++)
        {
            string cardToDeal = drawPile[i];
            hand.Add(cardToDeal);
            cardToMove = GameObject.Find(cardToDeal);
            cardToMove.transform.position = new Vector3(target.position.x + xOffset, target.position.y, target.position.z);
            cardToMove.transform.parent = target.transform;

            if (hand == playerHand)
            {
                cardToMove.GetComponent<Selectable>().showFace = true;
            }

            xOffset += 1.25f;
            drawPile.Remove(cardToDeal);
            yield return new WaitForSeconds(timeToWait);
        }
    }

    void DealAllCards()
    {
        StartCoroutine(DealCards(playerBottomRow, 3, playerBottomPos));
        StartCoroutine(DealCards(enemyBottomRow, 3, enemyBottomPos));
        StartCoroutine(DealCards(playerTopRow, 3, playerTopPos));
        StartCoroutine(DealCards(enemyTopRow, 3, enemyTopPos));
        StartCoroutine(DealCardsToHand(playerHand, 3, playerHandPos));
        StartCoroutine(DealCardsToHand(enemyHand, 3, enemyHandPos));
    }
}
