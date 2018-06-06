using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpiderController : MonoBehaviour {

	private Animator anim;
	public Tilemap solids;
	public int turn;
	private float time;
	public float interval;
	public bool controlled;

    public AudioClip step;
    public GameObject dust;

    private AudioSource source;

	private Vector3 target;
	private Vector3 start;
	private float targetAngle;
	private float startAngle;
	private bool jump;
	private bool paused;

	//public int hello;
	//private int turn;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.SetInteger("Turn", turn);
		target = transform.position;
		start = transform.position;
		targetAngle = transform.eulerAngles.z;
		startAngle = transform.eulerAngles.z;
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (time < interval) {
			time += Time.deltaTime;
		  transform.position = start + (target - start) * time / interval;
		  transform.eulerAngles = new Vector3(0, 0, startAngle + Mathf.DeltaAngle(startAngle, targetAngle) * time / interval);
		  //transform.position = Vector3.MoveTowards(transform.position, target, (1 / interval) * Time.deltaTime);
		  //transform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, (90 / interval) * Time.deltaTime));
		} else {
			transform.position = target;
			transform.eulerAngles = new Vector3(0, 0, targetAngle);
			/*
			*/
			start = target;
			startAngle = targetAngle;
			if (paused) {			
				anim.SetInteger("Turn", 0);
			} else {
				time = 0;

				float normal_angle = Mathf.Deg2Rad * (transform.eulerAngles.z - 90);
				float direction_angle = Mathf.Deg2Rad * (transform.eulerAngles.z);
		    Vector3Int normal = new Vector3Int((int)Mathf.Cos(normal_angle), (int)Mathf.Sin(normal_angle), 0);
				Vector3Int direction = new Vector3Int((int)Mathf.Cos(direction_angle) * turn, (int)Mathf.Sin(direction_angle) * turn, 0);
				int x = (int)(transform.position.x - solids.transform.position.x);
				int y = (int)(transform.position.y - solids.transform.position.y);

				if (jump) {
                    jump = false;
					//Debug.Log("Jump");
					for (int i = 1; i < 4; i++) {
						if (solids.GetTile(new Vector3Int(x - i * normal.x, y - i * normal.y, 0)) != null) {
                            //Debug.Log("something");
                            source.PlayOneShot(step, 2);
                            for (int j = -1; j < 2; j += 2)
                            {
                                GameObject d = Instantiate(dust, transform.position, dust.transform.rotation);
                                SimpleVelocityController v = d.GetComponent<SimpleVelocityController>();
                                v.velocity = j * (new Vector3(direction.x, direction.y, 0));
                                d.SetActive(true);
                            }

                            target = new Vector3(transform.position.x - 1 * (i - 1) * normal.x, transform.position.y - 1 * (i - 1) * normal.y, 0);
							targetAngle = transform.eulerAngles.z + 180;
							turn *= -1;
							anim.SetInteger("Turn", turn);
							return;
						}	
					}
					time = interval;
				} else if (solids.GetTile(new Vector3Int(x + direction.x, y + direction.y, 0)) != null) {
					//Debug.Log("inner");
					//transform.Rotate(0, 0, turn * 90, Space.World);
					targetAngle = transform.eulerAngles.z + turn * 90;
				} else if (solids.GetTile(new Vector3Int(x + direction.x + normal.x, y + direction.y + normal.y, 0)) != null) {
					//Debug.Log("flat");
					//transform.Translate(direction, Space.World);				
					target = transform.position + direction;	
				} else {
					//Debug.Log("outer");
					//transform.Translate(new Vector3Int(direction.x + normal.x, direction.y + normal.y, 0), Space.World);
					//transform.Rotate(0, 0, turn * -90, Space.World);
					targetAngle = transform.eulerAngles.z - turn * 90;
					target = transform.position + normal + direction;
				}
			}
		}
		if (controlled) {
			if (Input.GetKeyDown("space")) {
				//jump = true;
				paused = true;
			}
			if (Input.GetKeyUp("space")) {
				paused = false;
				anim.SetInteger("Turn", turn);
				jump = true;
			}
		}
	}
}
