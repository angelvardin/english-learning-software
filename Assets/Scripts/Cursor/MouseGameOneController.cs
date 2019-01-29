using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGameOneController : MonoBehaviour
{

	public float jetSpeed = 75.0f;

	public float maxEmissionRate = 400;
	public float minEmissionRate = 100f;
	private Rigidbody2D rb;
	private bool grounded;
	private ParticleSystem ps;
	private Animator anim;

	private int coins;

    private Collider2D playerCollider;

    private bool gameover;

	public LayerMask lm;
    public Texture2D cheeseTexture;

    private Coins coin;

    // Use this for initialization
    public void Start ()
    {
        this.coin = Coins.Instance;
		this.rb = this.GetComponent<Rigidbody2D>();
        this.ps = this.transform.Find("JetPack").GetComponent<ParticleSystem>();
        this.anim = this.GetComponent<Animator>();

        var isGrounded = this.transform.Find("GroundChecker");

        playerCollider = isGrounded.gameObject.GetComponent<Collider2D>();

    }
	
	// Update is called once per frame
	public void FixedUpdate ()
    {
        this.CheckIfOnGround();
        this.AjustJetPack();
	
	}

    private void CheckIfOnGround()
    {
        
        var colliding = Physics2D.OverlapCircle(playerCollider.transform.position, 0.5f, this.lm);
        //Debug.Log(colliding);
        if (colliding!=null)
        {
            this.grounded = true;
        }
        else
        {
            this.grounded = false;
        }
        this.anim.SetBool("Grounded", this.grounded);
    }

    private void AjustJetPack()
    {
        var emission = ps.emission;
        if (grounded)
        {
            emission.enabled = false;
        }
        else
        {
            emission.enabled = true;
            emission.rateOverTimeMultiplier = maxEmissionRate;
        }
    }

    public void OnGUI()
    {
        this.DisplayCheese();
        

    }

    public void DisplayCheese()
    {
        var rect = new Rect(10, 10, 32, 32);
        GUI.DrawTexture(rect, cheeseTexture);

        var coinsCountRect = new Rect(50, 10, 32, 32);
        var coinsStyle = new GUIStyle();
        coinsStyle.fontSize = 30;
        coinsStyle.fontStyle = FontStyle.Normal;
        coinsStyle.normal.textColor = Color.yellow;
        GUI.Label(coinsCountRect, Coins.Instance.CoinsCount.ToString(), coinsStyle);
    }
}
