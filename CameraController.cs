using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Raul Rojas
//CSE 1302
//Project One
//6-23-2017

public class CameraController : MonoBehaviour
{
	public GameObject Player;

	private Transform playerTF;
	private Vector3 offset;

	void Start ()
	{
		playerTF = Player.GetComponent<Transform>();
        offset = transform.position - playerTF.position;
	}
	void Update ()
	{
        transform.position = (playerTF.position + offset);
        
    }   
}
