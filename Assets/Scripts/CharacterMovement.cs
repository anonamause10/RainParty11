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
    private bool inputLock;
    public CameraControl camControl;
    

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        camControl = Camera.main.GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine&&PhotonNetwork.IsConnected){
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.T)){
            inputLock = !inputLock;
            camControl.locked = !camControl.locked;
            if(!inputLock){
                camControl.transitioning = true;
            }
        }


        if(inputLock){
            return;
        }

        Movement();

        //if (Input.GetKeyDown(KeyCode.Escape))
        //    Application.Quit();
        
    }

    void Movement()
    {
        if(hopTimer>0){
            hopTimer -= Time.deltaTime;
        }
        Vector3 prevMoveVec = moveVec;
        moveVec = new Vector3(Input.GetAxis("Horizontal"),2f*(1.2f*hopTimer-2),Input.GetAxis("Vertical"))*4;
        if(Input.GetKeyDown(KeyCode.E)){
            hopTimer = 2f;
        }
        controller.Move(moveVec*Time.deltaTime);
        //animator.SetFloat("speed", Mathf.Lerp(animator.GetFloat("speed"),xzMag(moveVec.normalized),5*Time.deltaTime));
        animator.SetFloat("speed", xzMag(moveVec));
        Quaternion targetRot = Quaternion.Euler(0,(xzMag(moveVec)>0.05?Mathf.Atan2(moveVec.x,moveVec.z)*Mathf.Rad2Deg:180),0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,5*Time.deltaTime);
    }

    void hop(Vector3 direction){
        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,5*Time.deltaTime);
        Vector3 vel = new Vector3(direction.x,2*hopTimer-2,direction.z);

    }

    float evalYPos(float val){
        return Mathf.Clamp((-1*(Mathf.Pow(val-1,2))+1),0,1);
    }

    float xzMag(Vector3 v){
        Vector2 z = new Vector2(v.x,v.z);
        return z.magnitude;
    }
}
