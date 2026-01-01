// This script moves your player automatically in the direction he is looking at. You can 
// activate the autowalk function by pull the cardboard trigger, by define a threshold angle 
// or combine both by selecting both of these options.
// The threshold is an value in degree between 0° and 90°. So for example the threshold is 
// 30°, the player will move when he is looking 31° down to the bottom and he will not move 
// when the player is looking 29° down to the bottom. This script can easally be configured
// in the Unity Inspector.Attach this Script to your CardboardMain-GameObject. If you 
// haven't the Cardboard Unity SDK, download it from https://developers.google.com/cardboard/unity/download

using UnityEngine;
using System.Collections;

public class Autowalk : MonoBehaviour 
{
	//batas maksimal sudut lihat kebawah
	private const int RIGHT_ANGLE = 90; 
	
	private bool isWalking = false;
	
	//mengambil posisi kepala atau mata dari reticlepointer
	public GvrReticlePointer head;
	
	//kecepatan pemain
	[Tooltip("With this speed the player will move.")]
	public float speed=5f;
	
	//jalan ketika ditekan cardboard trigger atau ketika lihat kebawah
	[Tooltip("Activate this checkbox if the player shall move when the Cardboard trigger is pulled.")]
	public bool walkWhenTriggered;
	
	[Tooltip("Activate this checkbox if the player shall move when he looks below the threshold.")]
	public bool walkWhenLookDown;
	
	//batas ambang sudut lihat kebawah. antara 0-90'
	[Tooltip("This has to be an angle from 0° to 90°")]
	public double thresholdAngle;
	
	//ini dinyalakan agar ketika karakter tidak memiliki collider, tapi tetap bisa berada pada permukaan
	[Tooltip("Activate this Checkbox if you want to freeze the y-coordiante for the player. " +
	         "For example in the case of you have no collider attached to your CardboardMain-GameObject" +
	         "and you want to stay in a fixed level.")]
	public bool freezeYPosition; 
	
	//nilai Y yang berhubungan dengan variabel diatas
	[Tooltip("This is the fixed y-coordinate.")]
	public float yOffset;
		
	void Update () 
	{
		// jalan ketika menggunakan cardboard trigger
		if (walkWhenTriggered && !walkWhenLookDown && !isWalking && Input.GetButtonDown("Fire1")) 
		{
			isWalking = true;
		} 
		else if (walkWhenTriggered && !walkWhenLookDown && isWalking && Input.GetButtonDown("Fire1")) 
		{
			isWalking = false;
		}
		
		// jalan ketika melihat dibawah sudut batas ambang 
		if (walkWhenLookDown && !walkWhenTriggered && !isWalking &&  
		    head.transform.eulerAngles.x >= thresholdAngle && 
		    head.transform.eulerAngles.x <= RIGHT_ANGLE) 
		{
			isWalking = true;
		} 
		else if (walkWhenLookDown && !walkWhenTriggered && isWalking && 
		         (head.transform.eulerAngles.x <= thresholdAngle ||
		         head.transform.eulerAngles.x >= RIGHT_ANGLE)) 
		{
			isWalking = false;
		}

		// jalan ketika player melihat kebawah DAN trigger cardboard ditekan
		if (walkWhenLookDown && walkWhenTriggered && !isWalking &&  
		    head.transform.eulerAngles.x >= thresholdAngle && 
		    Input.GetButtonDown("Fire1") &&
		    head.transform.eulerAngles.x <= RIGHT_ANGLE) 
		{
			isWalking = true;
		} 
		else if (walkWhenLookDown && walkWhenTriggered && isWalking && 
		         head.transform.eulerAngles.x >= thresholdAngle &&
		         (Input.GetButtonDown("Fire1") ||
		         head.transform.eulerAngles.x >= RIGHT_ANGLE)) 
		{
			isWalking = false;
		}
		
		//apabila jalan, maka ditransform forward sesuai angle kepala
		if (isWalking) 
		{
			Vector3 direction = new Vector3(head.transform.forward.x, 0, head.transform.forward.z).normalized * speed * Time.deltaTime; //maju kedepan
			Quaternion rotation = Quaternion.Euler(new Vector3(0, -transform.rotation.eulerAngles.y, 0)); //ambil posisi kepala
			transform.Translate(rotation * direction); //eksekusi dengan translasi
		}
		
		//mempertahankan posisi Y, agar tidak jatuh' kebawah. kasus untuk apabila player tidak memiliki collider
		if(freezeYPosition)
		{
			transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
		}
	}
}
