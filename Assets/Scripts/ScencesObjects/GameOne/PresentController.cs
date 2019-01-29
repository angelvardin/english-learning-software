using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class PresentController : MonoBehaviour {


	private SpriteRenderer spriteRndr;
	public string text = "some text";


	private bool lazer;

	public float interval = 0.5f;

	private float timeSinceStart;

	public float collidingInterval = 0.5f;

	public Sprite cheese;
	public Sprite plate;
    public bool isScaled = false;

    public LayerMask lm;

	public bool isTrue;

	private bool isChosen;

	private bool isHandCursorCatchingObject;

	private int CurrentLevel;

	private bool isColliding;

	private bool isTriggered;
	private bool CantChangeObjectScale;
	private bool CantChangeCursorSprite;
	public float scaleCoeficient;
	private bool FirstTimeCursorReachedWithClosedHand;
	private bool firstTimeCursorTriggeredWithOpenHand;
	private string cursorColliderName;

	private Coins coin;
	// Use this for initialization
	public void Start () {
		this.coin = Coins.Instance;
		this.isColliding = false;
		this.timeSinceStart = 0;
		this.spriteRndr = this.GetComponent<SpriteRenderer>();
		isHandCursorCatchingObject = false;

		this.isChosen = false;
		CurrentLevel = 0;
		GameManegmentHelper.isHandCursorCatchingObject = false;
		CantChangeCursorSprite = true;
		CantChangeObjectScale = true;
		InitCursor();
	}
	
	// Update is called once per frame
	public void Update ()
	{
		if (CurrentLevel<GameManegmentHelper.GameOneLevel)
		{
			CurrentLevel = GameManegmentHelper.GameOneLevel;
			isChosen = false;
		}

		if (this.isChosen)
		{
			//this.timeSinceStart += Time.fixedDeltaTime;
			//if (timeSinceStart >= interval)
			//{
			//	//Destroy(this.gameObject);
			//}
			return;
		}
  //      if (timeSinceStart >= interval)
  //      {
  //          isChosen = true;
  //          this.timeSinceStart = 0;

  //          isTrue = GameManegmentHelper.GameOneAnswers[gameObject.name];
  //          //Debug.Log(gameObject.name+" "+isTrue);
  //          if (this.isTrue)
		//	{
		//		Coins.Instance.CoinsCount++;
		//		this.spriteRndr.sprite = this.cheese;
  //              GameManegmentHelper.gameOneGivenAnswers++;
  //          }
		//	else
		//	{
		//		this.spriteRndr.sprite = this.plate;

		//	}
			
  //          ChechIfIsGameOver(GameManegmentHelper.gameOneAllAnswers, GameManegmentHelper.gameOneGivenAnswers);
		//}

		
		this.CheckIfColliding();
		if (isTriggered)
		{
			this.timeSinceStart += Time.deltaTime;
			//Debug.Log("time" + timeSinceStart);
			this.isTriggered = true;
			if (timeSinceStart >= interval)
			{
                //Debug.Log("Change sPRITE!");
                isScaled = true;
				ChangePresentScript();
			}
		}else
		{
			timeSinceStart = 0;
		}
		//if (this.isColliding==false)
		//{
		//	this.timeSinceStart = 0;
		//}
		//else
		//{
		//	this.timeSinceStart += Time.fixedDeltaTime;
		//}

	}

	private void ChechIfIsGameOver(int allAnswers, int givenAnswers)
	{
		if (allAnswers<=0||givenAnswers<=0)
		{
			return;
		}

		if (allAnswers == givenAnswers)
		{
			GameManegmentHelper.CurrentLevelOverGameOne = true;
		}
	}

	private void CheckIfColliding()
	{
		var Triggered = Physics2D.OverlapCircle(this.gameObject.transform.position, collidingInterval , this.lm);
		//Debug.Log("Colliding" + colliding);
		if (Triggered != null)
		{
			//Debug.Log("Yes!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + GameManegmentHelper.isHandCursorHovering);
			CursorFunction(Triggered);
			
		}
		else
		{
			
			if (CantChangeObjectScale == false)
			{
				CantChangeObjectScale = true;
				ScaleObject(-scaleCoeficient);
			}
			if (CantChangeCursorSprite == false)
			{
				//Debug.Log("Set false");
				GameManegmentHelper.isHandCursorCatchingObject = false;
				CantChangeCursorSprite = true;
			}
			//RemoveComponentFromHashSet(GameManegmentHelper.isColliding, cursorColliderName);
			InitCursor();
		}
		
	}

	private void ChangePresentScript()
	{
		isChosen = true;
		this.timeSinceStart = 0;

		isTrue = GameManegmentHelper.GameOneAnswers[gameObject.name];

		//Debug.Log(gameObject.name+" "+isTrue);
		if (this.isTrue)
		{
			Coins.Instance.CoinsCount++;
            var scale = this.transform.localScale;
            scale.x = 0.3f;
            scale.y = 0.3f;
            this.transform.localScale = scale;
            this.spriteRndr.sprite = this.cheese;
			GameManegmentHelper.gameOneGivenAnswers++;
		}
		else
		{
            var scale = this.transform.localScale;
            scale.x = 0.4f;
            scale.y = 0.4f;
            this.transform.localScale = scale;

            this.spriteRndr.sprite = this.plate;

		}

		ChechIfIsGameOver(GameManegmentHelper.gameOneAllAnswers, GameManegmentHelper.gameOneGivenAnswers);
	}

	private void CursorFunction(Collider2D triggered)
	{
        if (GameManegmentHelper.handState == HandState.Open && firstTimeCursorTriggeredWithOpenHand)
		{
			Debug.Log("first collision!");
			CantChangeObjectScale = false;
			ScaleObject(scaleCoeficient);

			firstTimeCursorTriggeredWithOpenHand = false;
		}
		else if (!firstTimeCursorTriggeredWithOpenHand && GameManegmentHelper.handState == HandState.Closed)
		{
			if (this.FirstTimeCursorReachedWithClosedHand)
			{
				//if (CheckIfObjectIsPicked(triggered.name))
				//{
				//    return;
				//}
				
				GameManegmentHelper.isHandCursorCatchingObject = true;
				Debug.Log("Set true");
				CantChangeCursorSprite = false;
				cursorColliderName = triggered.name;
				//GameManegmentHelper.isColliding.Add(triggered.name);
				this.FirstTimeCursorReachedWithClosedHand = false;
				isTriggered = true;
			   
				//this.CantPeekAnswerWords = true;
				//setObjectCoord();
			}
			else
			{
				
				//setObjectCoord();
			}
		}
		else if (!firstTimeCursorTriggeredWithOpenHand && GameManegmentHelper.handState == HandState.Open)
		{
			//if (CantChangeCursorSprite == false)
			//{
			//    Debug.Log("Set false");
			//    GameManegmentHelper.isHandCursorCatchingObject = false;
			//}
		}
	}

	private void ScaleObject(float scaleCoeficient)
	{
		Vector3 scale = transform.localScale;
		scale.x += scaleCoeficient; // your new value
		scale.y += scaleCoeficient; // your new value
		transform.localScale = scale;
	}

	private bool CheckIfObjectIsPicked(string name)
	{
		if (GameManegmentHelper.isColliding.Contains(name))
		{
			return true;
		}
		return false;
	}

	private void RemoveComponentFromHashSet(HashSet<string> hashset, string value)
	{
		if (String.IsNullOrEmpty(value))
		{
			return;
		}
		if (hashset.Contains(value))
		{
			hashset.Remove(value);
		}
	}

	private void InitCursor()
	{
		this.firstTimeCursorTriggeredWithOpenHand = true;
		this.FirstTimeCursorReachedWithClosedHand = true;
		this.isTriggered = false;
		this.cursorColliderName = String.Empty;
	}


}
