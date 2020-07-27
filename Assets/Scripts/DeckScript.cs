using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class DeckScript : MonoBehaviour
{
    private PhotonView PV;
    private List<GameObject> deck;
    private Card[] possibleCards;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        deck = new List<GameObject>();
        possibleCards = Resources.LoadAll<Card>("PhotonPrefabs/Cards");
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        checkDeck();

        if(Input.GetKeyDown(KeyCode.M)){
            newCard();
        }


    }

    public void newCard(){
        GameObject newCard;
        if(!PhotonNetwork.IsConnected){
            newCard = Instantiate((GameObject)Resources.Load("PhotonPrefabs/Card"), new Vector3(20,0,10), Quaternion.Euler(-90,180,0));
        }else{
            newCard = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Card"), new Vector3(20,0,10), Quaternion.Euler(-90,180,0));
        }
        newCard.GetComponent<CardDisplay>().SetCard(possibleCards[Random.Range(0,possibleCards.Length)]);
        newCard.transform.SetParent(transform, true);
        deck.Add(newCard);
        siftDeck();
    }

    public void popToHand(GameObject hand, int index = 0){
        if(index>=deck.Count||index<0){
            return;
        }
        GameObject card = deck[index];
        hand.GetComponent<HandScript>().addCard(card);
        deck.RemoveAt(index);
        if(PhotonNetwork.IsConnected){
            card.GetComponent<CardDisplay>().PV.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
        checkDeck();
        siftDeck();
    }

    void checkDeck(){
        for (int i = 0; i < deck.Count; i++)
        {
            if(deck[i]==null){
                deck.RemoveAt(i);
                i--;
                siftDeck();
                continue;
            }
        }
    }

    void siftDeck(){
        for (int i = 0; i < deck.Count; i++)
        {
            if(deck[i]==null){
                deck.RemoveAt(i);
                i--;
                continue;
            }
            float offsetVal = i*0.1f;
            Vector3 home = Vector3.down*offsetVal;
            deck[i].GetComponent<CardDisplay>().setHome(home, Quaternion.Euler(-90,180,0));
        }
    }
}
