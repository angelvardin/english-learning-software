using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RssultCheckerGameThree : MonoBehaviour {

    private Collider2D mainCollider;
    public GameObject cursor;
    public GameObject exitButton;
    public Sprite handSprite;
    public LayerMask PlayerLayer;
    public float overlap = 0.5f;
    public bool gameEnd;
    private int correctAnswers = 0;
    private bool isTriggered;
    private bool CantChangeObjectScale;
    private bool CantChangeCursorSprite;
    public float scaleCoeficient;
    private bool FirstTimeCursorReachedWithClosedHand;
    private bool firstTimeCursorTriggeredWithOpenHand;
    private string cursorColliderName;
    private bool isHandCursorCatchingObject;

    // Use this for initialization
    void Start () {
        this.isTriggered = false;
        mainCollider = this.gameObject.GetComponent<Collider2D>();
        GameManegmentHelper.GameThreeFinished = false;
        gameEnd = false;
        exitButton.SetActive(false);
        isHandCursorCatchingObject = false;

        GameManegmentHelper.isHandCursorCatchingObject = false;
        CantChangeCursorSprite = true;
        CantChangeObjectScale = true;

        InitCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManegmentHelper.GameThreeFinished)
        {
            return;
        }
        if (gameEnd)
        {
            return;
        }
        CollisionBetweenCurrentObjectAndCursor();
        if (isTriggered == true)
        {
            Debug.Log("is triggered");
            CheckResults();
            gameEnd = true;
        }
    }

    private void CheckResults()
    {
        var sentencesParts = GameObject.FindGameObjectsWithTag("answerword");
        SortedList<int, SortedList<int, string>> sentences = new SortedList<int, SortedList<int, string>>();
        
        foreach (var item in sentencesParts)
        {

            var component1 = item.GetComponent<InitGameThreeAnswers>();
            if (component1 != null)
            {
                
                
                string word = component1.GivenAnswerName;
                if (word.Equals(component1.CorrectAnswer.name))
                {
                    word = " вярно!";
                }
                else
                {
                    word = " невярно!";
                }
                StartCoroutine(YieldingUpdateLableText(item, word));
                
            }

        }
        var spr = cursor.GetComponent<SpriteRenderer>();
        spr.sprite = handSprite;
        exitButton.SetActive(true);
        GameManegmentHelper.GameThreeFinished = true;
        ScaleObject(-scaleCoeficient);


    }

    private IEnumerator YieldingUpdateLableText(GameObject go, string labletext)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
           
            var sl = go.transform.Find("LableText");
            var text = sl.transform.Find("Text");
            var textComponent = text.gameObject.GetComponent<Text>();
            string word = textComponent.text + labletext;
            Debug.Log("word");
            textComponent.text = word;
            workDone = true;
        }
    }

    public bool CollisionBetweenCurrentObjectAndCursor()
    {
        var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.PlayerLayer);
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
            RemoveComponentFromHashSet(GameManegmentHelper.isColliding, cursorColliderName);
            InitCursor();
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

    private void CursorFunction(Collider2D triggered)
    {
        if (GameManegmentHelper.isHandOpen == true && firstTimeCursorTriggeredWithOpenHand)
        {
            Debug.Log("first collision!");
            CantChangeObjectScale = false;
            ScaleObject(scaleCoeficient);

            firstTimeCursorTriggeredWithOpenHand = false;
        }
        else if (!firstTimeCursorTriggeredWithOpenHand && GameManegmentHelper.isHandOpen == false)
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
        else if (!firstTimeCursorTriggeredWithOpenHand && GameManegmentHelper.isHandOpen == true)
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

}
