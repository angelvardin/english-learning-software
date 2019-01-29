using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    public float jetSpeed = 75.0f;

    public float maxEmissionRate = 400;
    public float minEmissionRate = 100f;
  
    private bool grounded;
    private ParticleSystem ps;
    private Animator anim;

    private int coins;

    private Collider2D playerCollider;

    private bool gameover;

    private SpriteRenderer rndr;

    private Sprite defaultSprite;
    public Sprite hoverSprite;
    private bool spriteChange;
    private bool previousChange;

    public float scaleCoeficient = 0.3f;

    public LayerMask lm;
 

    // Use this for initialization
    public void Start()
    {
        rndr = this.gameObject.GetComponent<SpriteRenderer>();
        defaultSprite = rndr.sprite;
        this.ps = this.transform.Find("JetPack").GetComponent<ParticleSystem>();
        this.anim = this.GetComponent<Animator>();

        var isGrounded = this.transform.Find("CheckerGround");

        playerCollider = isGrounded.gameObject.GetComponent<Collider2D>();

    }

    public void Update()
    {
        //Debug.Log("Hand corsor hovering" + GameManegmentHelper.isHandCursorHovering);
        //Debug.Log(GameManegmentHelper.isHandCursorCatchingObject);
        if (GameManegmentHelper.isHandCursorCatchingObject)
        {
            //Debug.LogWarning("Daaaaaaaaaaaaa!!!!!!!!!!!!");
            if (!previousChange)
            {
                spriteChange = true;
                previousChange = true;
                StartCoroutine(ChangeCursorSprite(hoverSprite));
               // rndr.sprite = hoverSprite;
                ScaleObject(scaleCoeficient);
            }
           
        }
        else
        {
            //spriteChange = true;
            if (spriteChange)
            {
                spriteChange = false;
                previousChange = false;
                //rndr.sprite = defaultSprite;
                StartCoroutine(ChangeCursorSprite(defaultSprite));

                ScaleObject(-scaleCoeficient);
            }
        }
    }

    private IEnumerator ChangeCursorSprite(Sprite sprite)
    {
        bool workDone = false;

        while (!workDone)
        {
            yield return null;
            rndr.sprite = sprite;
        }
    }

    private void ScaleObject(float scaleCoeficient)
    {
        Vector3 scale = transform.localScale;
        scale.x += scaleCoeficient; // your new value
        scale.y += scaleCoeficient; // your new value
        transform.localScale = scale;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        this.CheckIfOnGround();
        this.AjustJetPack();

    }

    private void CheckIfOnGround()
    {

        var colliding = Physics2D.OverlapCircle(playerCollider.transform.position, 0.5f, this.lm);
        //Debug.Log(colliding);
        if (colliding != null)
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

}
