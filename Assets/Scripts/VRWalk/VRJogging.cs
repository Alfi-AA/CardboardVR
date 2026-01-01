using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VRJogging : MonoBehaviour {

	// variabel ini untuk simulasi gerakan accelerometer
	// gerakkan slider ke kiri dan ke kanan bergantian
	[Range(0f , 5f)]
	public float debugAcc;
	public bool debugAccelerationOn=true;
	//animation of the player game object, used to call the walking state
	//public Animator animPlayer;

	// variabel pengecekan gerakan accelerometer
	public float lowerLiM=-1.3f;
	public float upperLiM=-0.7f;
	public float walkMinTime=0.8f;
	public float speedFactor=0.01f;
	public float jumpTresHold=-0.6f;
	public float deltaDerivative=0.05f;
	public float derivativeThreshold=20;
	float acc_j_1,acc_j;
	public float Dacc;
	public bool jumping;
	public float JumpSpeed=250f;
	public float timeJump=1f;
	float max=0;


	//variabel pengecekan waktu gerak accelerometer
	float LOWtime;
	float UPtime;
	float elapsed;

	// kepala pemain
	public Transform head;
	// the accelerations in the 3 axis
	float accX,accY,accZ;
	// the player's rigid body
	Rigidbody RB;


	void Start ()
    {

		//apabila ada pergerakan accelerometer antara LOWtime sampai UPtime
		UPtime=1000;
		LOWtime=500;

		//gambil rigidbody player
		RB=gameObject.GetComponent<Rigidbody>();
		elapsed=0;
		
		//jalankan metode setelah 2 detik game dimulai
		Invoke("restartMax",2);

	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{

		//mengambil nilai accelerometer
		accX=Input.acceleration.x;
		accY=Input.acceleration.y;
		accZ=Input.acceleration.z;

		// accelerometer pakai debug, bukan aslinya 
		if (debugAccelerationOn) {
			accY = -debugAcc;
		}
		
		//ambil waktu berjalan
		elapsed+=Time.fixedDeltaTime;

		//obtain derivative for NOT for each timeUpdate, but instead:
		if(elapsed > deltaDerivative) 
		{
			acc_j=accY;

			//derivative expression
			Dacc=Mathf.Abs( (acc_j-acc_j_1)/elapsed);


			// ambil nilai tertinggi dari yang sekarang
			max=Mathf.Max(Dacc,max);

			// reset last iteration
			acc_j_1=accY;

			elapsed=0;
		}

		//check downstep
		if(accY>upperLiM)
		{	
			UPtime=Time.fixedTime;
		}
		else if(Time.fixedTime-UPtime>walkMinTime)
		{
			UPtime=1000;
		}
	
		//check upstep
		if(accY<lowerLiM)
		{	
			LOWtime=Time.fixedTime;
		}
		else if(Time.fixedTime-UPtime>walkMinTime)
		{
			UPtime=500;
		}


		//check jump
		if (Mathf.Abs (UPtime - LOWtime) < walkMinTime && Dacc < derivativeThreshold) {
			
			move (1 / Mathf.Abs (UPtime - LOWtime));
			//animPlayer.SetFloat("Forward",10);
		} else {
			//animPlayer.SetFloat("Forward",-1);
		}

		if(Dacc > derivativeThreshold && jumping==false)
		{
			jump();

		}


	}


	//set the animation to walk
	public void move(float v)
	{
		//use with static animations
		RB.MovePosition(transform.position+head.transform.forward*v*speedFactor+transform.up*0.01f);

		//animPlayer.SetFloat("Forward",1);
		
	}


	// JUMPING FUNCTION: change animation here to jump
	public void jump()
	{

		RB.AddForce(new Vector3(0,JumpSpeed*RB.mass,0));
		Invoke("stopJump",timeJump);

		jumping=true;
		//animPlayer.SetFloat("Jump",1);

	}


	// it is called after a certain time to stop jumping
	public void stopJump()
	{

		//RB.AddForce(JumpSpeed*RB.mass/10*head.forward);
		jumping=false;
		//animPlayer.SetFloat("Jump",-1);
	}


	public void restartMax()
	{
		max=0;
	}

}
