using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVelocityController : MonoBehaviour {

    public Vector3 velocity;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position += Time.deltaTime * velocity;
	}
}
