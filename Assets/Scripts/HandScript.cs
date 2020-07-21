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
            GameObject newCard;
            if(!PhotonNetwork.IsConnected){
                newCard = Instantiate((GameObject)Resources.Load("PhotonPrefabs/Card"), Vector3.zero, Quaternion.Euler(90,0,0));
            }else{
                newCard = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Card"), Vector3.zero, Quaternion.Euler(90,0,0));
            }
            newCard.transform.SetParent(transform, false);
            newCard.transform.localPosition = transform.InverseTransformPoint(new Vector3(15,0,7));
            newCard.transform.forward = transform.InverseTransformDirection(Vector3.up);
            newCard.GetComponent<CardDisplay>().setHome(Vector3.down*0.6f, Quaternion.identity);
            hand.Add(newCard);
            siftHand();
        }


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
            float offsetVal = (i-(hand.Count/2f))/2f+0.5f;
            Vector3 home = (Vector3.down*0.6f)+(Vector3.left*offsetVal)+(Vector3.forward*i*0.03f);
            hand[i].GetComponent<CardDisplay>().setHome(home, Quaternion.identity);
        }
    }
}
