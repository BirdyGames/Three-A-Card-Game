using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public Sprite cardFace, cardBack;
    private SpriteRenderer theSR;
    private Selectable Selectable;
    private GameManager theGM;

    // Start is called before the first frame update
    void Start()
    {
        GameManager theGM = FindObjectOfType<GameManager>();
        theSR = GetComponent<SpriteRenderer>();
        Selectable = GetComponent<Selectable>();

        List<string> deck = GameManager.GenerateDeck();

        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = theGM.cardFaces[i];
                break;
            }
            i++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Selectable.showFace)
        {
            theSR.sprite = cardFace;
        } else
        {
            theSR.sprite = cardBack;
        }
    }
}
