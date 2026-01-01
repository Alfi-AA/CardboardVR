using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour{ 
	public void TeleportPlayer(){
		GameObject go = GameObject.Find("Player");
		go.transform.position = new Vector3(transform.position.x, transform.position.y+1.5f, transform.position.z);
	}
}
