using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerController : MonoBehaviour {

	public Sprite laserOn;
	public Sprite lazerOff;
    public BoxCollider2D bxcoll;
    public float rotationSpeed = 1.0f;
    

	private SpriteRenderer spriteRndr;

	private bool lazer;

	public float interval = 0.5f;

	private float timeSinceStart;

	// Use this for initialization
	public void Start () {

		this.timeSinceStart = 0;
		lazer = false;
		this.spriteRndr = this.GetComponent<SpriteRenderer>();
        this.bxcoll = this.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		this.timeSinceStart += Time.fixedDeltaTime;
		if (this.timeSinceStart > this.interval)
		{
			this.lazer = !this.lazer;
			this.timeSinceStart = 0;

            if (this.lazer)
            {
                bxcoll.enabled = true;

            }
            else
            {
                bxcoll.enabled = false;
            }


            this.spriteRndr.sprite = this.lazer ? this.laserOn : this.lazerOff;
		}

        this.transform.Rotate(0, 0, this.rotationSpeed);
	}
}
