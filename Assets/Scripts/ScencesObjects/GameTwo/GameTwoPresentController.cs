using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTwoPresentController : MonoBehaviour {


    private SpriteRenderer spriteRndr;

    public float interval = 0.5f;

    private float timeSinceStart;

    public float collidingInterval = 0.5f;

    public Sprite cheese;
    public Sprite plate;

    public LayerMask lm;

    public bool isTrue;
    public bool isScaled = false;

    private bool isChosen;

    private int CurrentLevel;

    private bool isColliding;
    
    // Use this for initialization
    public void Start()
    {
       
        this.isColliding = false;
        this.timeSinceStart = 0;
        this.spriteRndr = this.GetComponent<SpriteRenderer>();
        this.isChosen = false;
        CurrentLevel = 0;
        GameManegmentHelper.gameTwoPresentsStartPosition.Add(this.name, this.transform.position);
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (CurrentLevel < GameManegmentHelper.GameTwoLevel)
        {
            CurrentLevel = GameManegmentHelper.GameTwoLevel;
            isChosen = false;
        }

        if (this.isChosen)
        {
            
            return;
        }
        if (timeSinceStart >= interval)
        {
            isChosen = true;
            this.timeSinceStart = 0;

            isTrue = GameManegmentHelper.GameTwoAnswers[gameObject.name];
            isScaled = true;
            //Debug.Log(gameObject.name+" "+isTrue);
            if (this.isTrue)
            {
               
                this.spriteRndr.sprite = this.cheese;
                var scale = this.transform.localScale;
                scale.x = 0.7f;
                scale.y = 0.7f;
                var position = this.transform.position;
                //position.y =position.y + 10.0f;
                this.transform.localScale = scale;
                float x = this.transform.position.x;
                //x = x + 1.5f;
                position.x = x + 2f;
                this.transform.position = position;
                GameManegmentHelper.CurrentLevelOverGameTwo = true;
            }
            else
            {
                GameManegmentHelper.CurrentLevelOverGameTwo = true;
                
                this.spriteRndr.sprite = this.plate;
                var scale = this.transform.localScale;
                scale.x = 0.5f;
                scale.y = 0.5f;
                var position = this.transform.position;
                //position.y =position.y + 10.0f;
                this.transform.localScale = scale;
                float x = this.transform.position.x;
                //x = x + 1.5f;
                position.x = x+2f;
                this.transform.position = position;

            }

        }


        this.CheckIfColliding();
        if (this.isColliding == false)
        {
            this.timeSinceStart = 0;
        }
        else
        {
            this.timeSinceStart += Time.fixedDeltaTime;
        }

    }


    private void CheckIfColliding()
    {
        var colliding = Physics2D.OverlapCircle(this.gameObject.transform.position, collidingInterval, this.lm);
        //Debug.Log("Colliding" + colliding);
        if (colliding!=null)
        {
            //Debug.Log("Colliding");
        }
        this.isColliding = colliding == null ? false : true;
    }

}
