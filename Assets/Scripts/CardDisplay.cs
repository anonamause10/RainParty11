using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CardDisplay : MonoBehaviour
{

    public Card card;
    public Card replaceCard;

    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text cost;
    
    public Image sideBar;
    public Image image;

    private PhotonView PV;

    private Vector3 targetPos;
    private Vector3 posDamp = Vector3.zero;
    private Quaternion targetRotation;


    private RaycastHit hit;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
        title.text = card.title;
        description.text = card.description;
        cost.text = card.cost+"";
        sideBar.sprite = card.sideBar;
        image.sprite = card.image;
        PV = GetComponent<PhotonView>();
        
        if(!PV.IsMine&&PhotonNetwork.IsConnected){
            this.enabled = false;
        }
        targetPos = transform.position;
        cam = Camera.main;
    }

    public void SetCard(Card newCard){
        card = newCard;
        title.text = card.title;
        description.text = card.description;
        cost.text = card.cost+"";
        sideBar.sprite = card.sideBar;
        image.sprite = card.image;
    }

    void Update() {
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref posDamp, 0.1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10*Time.deltaTime);
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide)){
            if(hit.collider.gameObject == gameObject){
                targetPos = new Vector3(0,0.3f,-5.5f);
                targetRotation = Quaternion.Euler(0,-20,0);
            }else{
                targetPos = new Vector3(0,0,-5.5f);
                targetRotation = Quaternion.Euler(0,0,0);
            }
        }else{
            targetPos = new Vector3(0,0,-5.5f);
            targetRotation = Quaternion.Euler(0,0,0);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            SetCard(replaceCard);
        }
    }

    
}
