using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandStateDragAngDropFirst : MonoBehaviour
{
    private bool isTriggered;
    private bool answerDestinationReached;
    public Transform cursorCoordinates;
    private float startX;
    private float startY;
    public float overlap = 0.5f;
    private float offcetX;
    private float offcetY;
    public float interval = 0.5f;
    private float timeSinceStart;
    private float timeSinceStart1;
    private Collider2D mainCollider;
    public LayerMask PlayerLayer;
    public LayerMask TargetLayer;
    private string cursorColliderName;
    private string targetColliderName;
    private bool isTargetReached;
    private bool FirstTimeCursorReachedWithClosedHand;
    private bool CantPeekAnswerWords;
    private bool firstTimeCursorTriggeredWithOpenHand;
    public float scaleCoeficient = 0.3f;
    private bool CantChangeCursorSprite;
    private bool CantChangeObjectScale;

    // Use this for initialization
    private void Start()
    {
        answerDestinationReached = false;
        startX = this.gameObject.transform.position.x;
        startY = this.gameObject.transform.position.y;
        mainCollider = this.gameObject.GetComponent<Collider2D>();
        this.isTargetReached = false;
        GameManegmentHelper.isHandCursorCatchingObject = false;
        CantChangeCursorSprite = true;
        CantChangeObjectScale = true;
        InitCursor();
    }

    private void InitCursor()
    {
        this.firstTimeCursorTriggeredWithOpenHand = true;
        this.FirstTimeCursorReachedWithClosedHand = true;
        this.isTriggered = false;
        this.CantPeekAnswerWords = false;
        this.cursorColliderName = String.Empty;
    }

    // Update is called once per frame
    /// <summary>
    ///
    /// </summary>
    public void Update()
    {
        if (GameManegmentHelper.GameThreeFinished)
        {
            return;
        }

        if (answerDestinationReached)
        {
           
            this.timeSinceStart += Time.deltaTime;
            if (timeSinceStart >= interval+1)
            {
                this.timeSinceStart = 0;
                answerDestinationReached = false;
            }
            return;
        }
        CollisionBetweenCurrentObjectAndCursor();
        if (isTriggered == true)
        {
            var pos = this.gameObject.transform.position;
            pos.x += (offcetX);
            pos.y += (offcetY);
            this.gameObject.transform.position = pos;
            //if (this.CantPeekAnswerWords)
            //{
            //    this.timeSinceStart1 += Time.deltaTime;
            //    if (timeSinceStart1 >= interval)
            //    {
            //        this.timeSinceStart1 = 0;
            //        CantPeekAnswerWords = false;
            //    }
            //    return;
            //}
            bool collision = CollisionBetweenCurrentObjectAndTargetPosition();
        }
        else
        {
            CheckForCollisionWithTarget();
        }
    }

    private void CheckForCollisionWithTarget()
    {
        var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.TargetLayer);
        if (Triggered != null)
        {
            if (CheckIfObjectIsPicked(Triggered.name))
            {
                return;
            }
            answerDestinationReached = true;
            this.gameObject.transform.position = Triggered.transform.position;
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

    private void CursorFunction(Collider2D triggered)
    {
        if (GameManegmentHelper.isHandOpen == true && firstTimeCursorTriggeredWithOpenHand)
        {
            //Debug.Log("first collision!");
            CantChangeObjectScale = false;
            ScaleObject(scaleCoeficient);

            firstTimeCursorTriggeredWithOpenHand = false;
        }
        else if (!firstTimeCursorTriggeredWithOpenHand && GameManegmentHelper.isHandOpen == false)
        {
            if (this.FirstTimeCursorReachedWithClosedHand)
            {
                if (CheckIfObjectIsPicked(triggered.name))
                {
                    return;
                }
                GameManegmentHelper.isHandCursorCatchingObject = true;
                //Debug.Log("Set true");
                CantChangeCursorSprite = false;
                cursorColliderName = triggered.name;
                GameManegmentHelper.isColliding.Add(triggered.name);
                this.FirstTimeCursorReachedWithClosedHand = false;
               
                setObjectCoord();
            }
            else
            {
                setObjectCoord();
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

    private void setObjectCoord()
    {
        GameManegmentHelper.isHandCursorCatchingObject = true;
        this.isTriggered = true;
        this.offcetX = this.cursorCoordinates.position.x - this.gameObject.transform.position.x;
        this.offcetY = this.cursorCoordinates.position.y - this.gameObject.transform.position.y;
    }

    public bool CollisionBetweenCurrentObjectAndTargetPosition()
    {
        var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.TargetLayer);
        if (Triggered != null)
        {
            //Debug.Log("Triggered");
            if (this.isTargetReached)
            {
                return true;
            }
            //Debug.Log("Triggered 1");
            if (CheckIfObjectIsPicked(Triggered.name))
            {
                return false;
            }
            //Debug.Log("Triggered 2");
            GameManegmentHelper.isColliding.Add(Triggered.name);
            targetColliderName = Triggered.name;
            this.gameObject.transform.position = Triggered.transform.position;
           // Debug.Log("Triggered 3");
            this.isTriggered = false;
            this.answerDestinationReached = true;
            this.isTargetReached = true;
            StartCoroutine(SetAnswer(Triggered.name));
            //ResultCollector.UserResults.Add(this.gameObject.name, Triggered.name);
            //ResultCollector.UserResults.Add(Triggered.name, this.gameObject.name);
            GameManegmentHelper.order.Push(
               new UndoInformation()
               {
                   ObjectName = this.gameObject.name,
                   X = startX,
                   Y = startY
               });

            this.CantPeekAnswerWords = true;
        }
        else
        {
            //StartCoroutine(RemovedAnswer(targetColliderName));
            RemovedAnswerSync(targetColliderName);
            RemoveComponentFromHashSet(GameManegmentHelper.isColliding, targetColliderName);
            //RemoveStringComponentFromDictionary(ResultCollector.UserResults, targetColliderName);
            //RemoveStringComponentFromDictionary(ResultCollector.UserResults, this.gameObject.name);
            targetColliderName = String.Empty;
            isTriggered = false;
            this.isTargetReached = false;
        }
        return isTriggered;
    }

    private void RemovedAnswerSync(string name)
    {
        var component = this.gameObject.GetComponent<InitGameThreeAnswers>();
        component.GivenAnswerName = String.Empty;
    }

    private IEnumerator RemovedAnswer(string name)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            var component = this.gameObject.GetComponent<InitGameThreeAnswers>();
            component.GivenAnswerName = String.Empty;
            workDone = true;
        }
    }

    private IEnumerator SetAnswer(string name)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            var component = this.gameObject.GetComponent<InitGameThreeAnswers>();
            component.GivenAnswerName = name;
            workDone = true;
        }
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

    private void RemoveStringComponentFromDictionary(Dictionary<string, string> dictionary, string key)
    {
        if (String.IsNullOrEmpty(key))
        {
            return;
        }
        if (dictionary.ContainsKey(key))
        {
            dictionary.Remove(key);
        }
    }
}