using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	
	public Animator anim;
	public Rigidbody rbody;

	private float inputH;
	private float inputV;
	private bool run;
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;
    // Use this for initialization
    void Start () 
	{
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody>();
		run = false;
	}


	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			int n = Random.Range (0, 2);
            anim.Play("Attack_01", -1, 0F);
   //         if (n == 0) {
			//	anim.Play ("Dame_01", -1, 0F);
			//} else {
			//	anim.Play ("Dame_02", -1, 0F);
			//}
		}
			if (Input.GetKeyDown ("1")) {
				anim.Play ("Attack_01", -1, 0F);
			}
			if (Input.GetKeyDown ("2")) {
				anim.Play ("Attack_02", -1, 0F);
			}
			if (Input.GetKeyDown ("3")) {
				anim.Play ("Attack_03", -1, 0F);
			}
			if (Input.GetKeyDown ("4")) {
				anim.Play ("Attack_04", -1, 0F);
			}
			
			if (Input.GetKeyDown ("5")) {
				anim.Play ("Attack_05", -1, 0F);
			}
			if (Input.GetKeyDown ("6")) {
				anim.Play ("Attack_06", -1, 0F);
			}
			if (Input.GetKeyDown ("7")) {
				anim.Play ("Attack_07", -1, 0F);
			}
			if (Input.GetKeyDown ("8")) {
				anim.Play ("Death_01", -1, 0F);
			}
			if (Input.GetKeyDown ("9")) {
				anim.Play ("Death_02", -1, 0F);
			}
			if (Input.GetKeyDown ("0")) {
			anim.Play ("Idle_nonWeapon", -1, 0F);
			}
			
			if (Input.GetKeyDown ("g")) {
				anim.Play ("Crouch", -1, 0F);
			}
			if (Input.GetKeyDown ("t")) {
				anim.Play ("Crouch_Move_F", -1, 0F);
			}
			if (Input.GetKeyDown ("r")) {
				anim.Play ("Crouch_Move_L", -1, 0F);
			}
			if (Input.GetKeyDown ("y")) {
				anim.Play ("Crouch_Move_R", -1, 0F);
			}
		if(Input.GetKey(KeyCode.LeftShift)) 
		{
			run = true;
		}
		else
		{
			run = false;
		}

		if (Input.GetKey (KeyCode.Space)) 
		{
			anim.SetBool ("jump", true);
		} 
		else
		{
			anim.SetBool ("jump",false);
		}

        inputH = Input.GetAxisRaw ("Horizontal");
		inputV = Input.GetAxisRaw ("Vertical");

        if(System.Math.Abs(inputH) > 0 || System.Math.Abs(inputV) > 0)
        {
            anim.SetBool("IsWalking", true);
        }else{
            anim.SetBool("IsWalking", false);
        }
	
		anim.SetBool("run",run);


		float moveX = inputH *30f* Time.deltaTime;
		float moveZ = inputV *30f* Time.deltaTime;

        if(run)
		{
			moveX*=2.5f;
			moveZ*=2.5f;

		}

		rbody.velocity = new Vector3(moveX,0f,moveZ);
        Vector3 movement = rbody.velocity * 10f * Time.deltaTime;
        rbody.MovePosition(transform.position + movement);

        Turning();
    }

    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            rbody.MoveRotation(newRotation);
        }
    }
}


