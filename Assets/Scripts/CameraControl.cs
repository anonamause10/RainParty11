using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Vector3 targetPos;
    public Quaternion targetRot;
    public Vector3 outPos;
    private Vector3 outPosDamp = Vector3.zero;
    public Quaternion outRot;
    public float boost = 10f;
    private float rotX;
    private float rotY;
    private float turnSpeed = 4f;
    private bool transitioning;
    // Start is called before the first frame update
    void Start()
    {
        outPos = transform.position;
        outRot = transform.rotation;
        rotX = -transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(transitioning){
			transition();
			return;
		}

        if(Input.GetKeyDown(KeyCode.T)){
            transitioning = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        // Unlock and show cursor when right mouse button released
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        boost += Input.mouseScrollDelta.y * 0.5f;

        Vector3 translation = GetInputTranslationDirection() * boost;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            translation *= 10.0f;
        }
        transform.Translate(translation*Time.deltaTime);

        if(Input.GetMouseButton(1)){
            rotY = Input.GetAxis("Mouse X") * turnSpeed;
	        rotX += Input.GetAxis("Mouse Y") * turnSpeed;
            rotX = Mathf.Clamp(rotX, -90, 90);
            transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + rotY, 0);
        }
    }

    public void transition(){
        transitioning = true;
        transform.position = Vector3.SmoothDamp(transform.position, outPos, ref outPosDamp, 0.3f);
		transform.rotation = Quaternion.Slerp(transform.rotation, outRot, 5*Time.deltaTime);
		if((transform.position - outPos).magnitude<0.004f&&Quaternion.Angle(transform.rotation, outRot)<0.5f){
			transitioning = false;
			
			rotX = -transform.eulerAngles.x;
			rotY = 0;
			
		}
    }

    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction += Vector3.up;
        }
        return direction;
    }
}
