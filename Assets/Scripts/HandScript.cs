using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class HandScript : MonoBehaviour
{

    private Transform cameraT;
    private PhotonView PV;
    private List<GameObject> hand;
    private float offestFactor = 0.25f;
    private float angleFactor = 2;
    // Start is called before the first frame update
    void Start()
    {
        cameraT = Camera.main.transform;
        PV = GetComponent<PhotonView>();
        hand = new List<GameObject>();
        if(!PV.IsMine&&PhotonNetwork.IsConnected){
            this.enabled = false;
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = cameraT.position + cameraT.forward*3;
        transform.rotation = cameraT.rotation;

        checkHand();

        if(Input.GetKeyDown(KeyCode.N)){
            /*
            GameObject newCard;
            if(!PhotonNetwork.IsConnected){
                newCard = Instantiate((GameObject)Resources.Load("PhotonPrefabs/Card"), new Vector3(15,0,7), Quaternion.Euler(-90,0,0));
            }else{
                newCard = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Card"), new Vector3(15,0,7), Quaternion.Euler(-90,0,0));
            }
            addCard(newCard);*/
            requestFromDeck();
        }


    }

    public void requestFromDeck(int index = 0){
        GameObject.Find("Deck").GetComponent<DeckScript>().popToHand(gameObject, index);
    }

    public void addCard(GameObject newCard){
        newCard.transform.SetParent(transform, true);
        newCard.GetComponent<CardDisplay>().setHome(Vector3.down*0.6f, Quaternion.identity);
        newCard.GetComponent<CardDisplay>().setHand();
        hand.Add(newCard);
        siftHand();
    }

    void checkHand(){
        for (int i = 0; i < hand.Count; i++)
        {
            if(hand[i]==null){
                hand.RemoveAt(i);
                i--;
                siftHand();
                continue;
            }
        }
    }

    void siftHand(){
        for (int i = 0; i < hand.Count; i++)
        {
            if(hand[i]==null){
                hand.RemoveAt(i);
                i--;
                continue;
            }
            float offsetVal = (i-(hand.Count/2f))/2f+offestFactor;
            Vector3 home = (Vector3.down*(0.6f+0.1f*Mathf.Abs(offsetVal)))+(Vector3.left*(offsetVal))+(Vector3.forward*i*0.03f);
            hand[i].GetComponent<CardDisplay>().setHome(home, Quaternion.Euler(0,0,offsetVal*10*angleFactor/hand.Count));
        }
    }
}
