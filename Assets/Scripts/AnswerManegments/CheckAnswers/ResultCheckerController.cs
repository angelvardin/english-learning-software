using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCheckerController: MonoBehaviour {

	private Collider2D mainCollider;
	private bool isTriggered;
	private bool timer;
	private bool showResults;
    private bool first;
	private float timeSinceStart;
	public float interval;
	private int correctAnswers = 0;
	public float overlap = 0.5f;
    public bool gameEnd;

	public LayerMask PlayerLayer;
	public Texture2D resultTexture;

	// Use this for initialization
	void Start () {
		this.isTriggered = false;
		mainCollider = this.gameObject.GetComponent<Collider2D>();
		timer = false;
		timeSinceStart = 0.0f;
		showResults = false;
        first = true;
        gameEnd = false;
	}

	// Update is called once per frame
	public void Update ()
	{
        if (gameEnd)
        {
            return;
        }
		CollisionBetweenAnswerButtonAndCursor();
		if (!isTriggered)
		{
			timeSinceStart = 0;
			return;
		}

		if (timer)
		{
			this.timeSinceStart += Time.deltaTime;
			if (timeSinceStart >= interval)
			{
				this.timeSinceStart = 0;
				timer = false;
			}
			return;
		}

		CheckResults();
		
	}

	private void CheckResults()
	{
		
		showResults = true;
		int MaxResult = ResultCollector.answers.Count;
		foreach (var item in ResultCollector.UserResults)
		{
            if (first)
            {
                Debug.Log(item.Key + " " + item.Value);
            }
			foreach (var answer in ResultCollector.answers)
			{
                if (first)
                {
                    Debug.Log(answer.Key + " " + answer.Value.Key+" "+answer.Value.Value);
                }

                if (item.Key.Equals(answer.Value.Key) && item.Value.Equals(answer.Value.Value))
				{
                   

                    correctAnswers++;
				}
			}
            
		}
        first = false;
        gameEnd = true;
    }

	private void CollisionBetweenAnswerButtonAndCursor()
	{

		if (GameManegmentHelper.isColliding.Count > 0)
		{
           // Debug.Log(Undo.isColliding.Count);
			//return;
		}
		var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.PlayerLayer);
		//Debug.Log(Triggered);
		if (Triggered != null)
		{
			isTriggered = true;
            timer = true;
		}
	}

	public void OnGUI()
	{
		if (!showResults)
		{
			return;
		}
		this.DisplayCoins();
	}

	private void DisplayCoins()
	{
		var rect = new Rect(10, 10, 32, 32);
		GUI.DrawTexture(rect, resultTexture);

		var coinsCountRect = new Rect(50, 10, 32, 32);
		var coinsStyle = new GUIStyle();
		coinsStyle.fontSize = 30;
		coinsStyle.fontStyle = FontStyle.Normal;
		coinsStyle.normal.textColor = Color.yellow;
		GUI.Label(coinsCountRect, this.correctAnswers.ToString(), coinsStyle);
	}
}
