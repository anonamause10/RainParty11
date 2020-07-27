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

    public PhotonView PV;

    public Vector3 targetPos;
    private Quaternion targetRotation;

    private Vector3 homePos;
    private Quaternion homeRot;

    private RaycastHit hit;
    private Camera cam;
    public bool transitioning;

    private float targetDissolve;
    private Transform cardCanvas;
    public Color color;
    public Vector2 colorPos;
    private bool inHand;
    // Start is called before the first frame update
    void Start()
    {
        transitioning = false;
        title.text = card.title;
        description.text = card.description;
        cost.text = card.cost+"";
        sideBar.sprite = card.sideBar;
        image.sprite = card.image;
        GetComponent<Renderer>().material.SetColor("EdgeColor", ((Texture2D)sideBar.mainTexture).GetPixel(88,398)*10);
        PV = GetComponent<PhotonView>();
        PV.ObservedComponents[0] = GetComponent<PhotonTransformView>();
        cardCanvas = transform.Find("Canvas");
        targetPos = transform.localPosition;
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
        if(PhotonNetwork.IsConnected&&!PV.IsMine){
            return;
        }
        color = ((Texture2D)sideBar.mainTexture).GetPixel((int)colorPos.x,(int)colorPos.y);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 7.5f*Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, 7.5f*Time.deltaTime);
        if(transitioning){
            if((transform.localPosition - targetPos).magnitude<0.004f&&Quaternion.Angle(transform.localRotation, targetRotation)<0.5f){
                transitioning = false;
                Destroy(gameObject);
            }
            float dissolveVal = Mathf.Lerp(GetComponent<Renderer>().material.GetFloat("_cutoff"),targetDissolve,5*Time.deltaTime);
            GetComponent<Renderer>().material.SetFloat("_cutoff", dissolveVal);
            cardCanvas.localScale = new Vector3(0.01f, 0.00735f, 10) * Mathf.Pow(0.833f*(1-dissolveVal),5);
            return;
        }
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide)&&inHand){
            if(hit.collider.gameObject == gameObject){
                targetPos = homePos + Vector3.up*0.3f + Vector3.back*0.6f + Vector3.left*(transform.localPosition.x*0.6f);
                targetRotation = Quaternion.Euler(0,1.1f*Vector3.SignedAngle(cam.transform.forward,transform.position-cam.transform.position,transform.up),0);
                if(Input.GetMouseButtonDown(0)){
                    transitioning = true;
                    targetPos = Vector3.zero;
                    targetRotation = Quaternion.Euler(90,0,0);
                    targetDissolve = 1;
                }
            }else{
                targetPos = homePos;
                targetRotation = homeRot;
            }
        }else{
            targetPos = homePos;
            targetRotation = homeRot;
        }
        
    }

    public void setHand(){
        inHand = true;
        if(!PV.IsMine&&PhotonNetwork.IsConnected){
            this.enabled = false;
        }
    }

    public void setHome(Vector3 pos, Quaternion rot){
        homePos = pos;
        homeRot = rot;
    }

    public Vector3 getTargetPos(){
        return targetPos;
    }

    
}
