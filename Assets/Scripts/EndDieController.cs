using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDieController : MonoBehaviour {

    public float delay;
    private float time;
	// Use this for initialization
	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > delay)
        {
            gameObject.SetActive(false);
        }
    }
}
