using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterMovement : MonoBehaviour
{
    private PhotonView PV;
    private CharacterController controller;
    private Animator animator;
    private float hopTimer;
    private Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine&&PhotonNetwork.IsConnected){
            return;
        }
        
        Movement();

        //if (Input.GetKeyDown(KeyCode.Escape))
        //    Application.Quit();
        
    }

    void Movement()
    {
        print(hopTimer);
        if(hopTimer>-2f){
            hopTimer -= 10*Time.deltaTime;
        }
        Vector3 prevMoveVec = moveVec;
        moveVec = new Vector3(Input.GetAxis("Horizontal"),hopTimer,Input.GetAxis("Vertical"))*4;
        if(Input.GetKeyDown(KeyCode.E)){
            hopTimer = 2f;
        }
        controller.Move(moveVec*Time.deltaTime);
        //animator.SetFloat("speed", Mathf.Lerp(animator.GetFloat("speed"),xzMag(moveVec.normalized),5*Time.deltaTime));
        animator.SetFloat("speed", xzMag(moveVec));
        Quaternion targetRot = Quaternion.Euler(0,(xzMag(moveVec)>0.05?Mathf.Atan2(moveVec.x,moveVec.z)*Mathf.Rad2Deg:180),0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,5*Time.deltaTime);
    }

    float evalYPos(float val){
        return Mathf.Clamp((-1*(Mathf.Pow(val-1,2))+1),0,1);
    }

    float xzMag(Vector3 v){
        Vector2 z = new Vector2(v.x,v.z);
        return z.magnitude;
    }
}
