using System;
using System.Collections.Generic;
using UnityEngine;

public class DragAngDrop : MonoBehaviour
{
    private bool isTriggered;
    private bool destinationReached;
    private bool firstTriggered;
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
    public LayerMask StartLayer;
    private string cursorColliderName;
    private string targetColliderName;
    private string startColliderName;
    private bool isTargetReached;
    private bool isCursorReached;
    private bool isStartReached;
    private bool Objectpicked;

    // Use this for initialization
    private void Start()
    {
        GameManegmentHelper.isUI = false;
        destinationReached = false;
        this.isTriggered = false;
        startX = this.gameObject.transform.position.x;
        startY = this.gameObject.transform.position.y;
        this.firstTriggered = true;
        mainCollider = this.gameObject.GetComponent<Collider2D>();
        Debug.Log(gameObject.name + " " + gameObject.transform.position.y + " " + gameObject.transform.position.y);
        this.isTargetReached = false;
        this.Objectpicked = false;
    }

    // Update is called once per frame
    /// <summary>
    ///
    /// </summary>
    public void Update()
    {
        if (destinationReached)
        {
            this.timeSinceStart += Time.deltaTime;
            if (timeSinceStart >= interval)
            {
                this.timeSinceStart = 0;
                destinationReached = false;
            }
            return;
        }
        CollisionBetweenCurrentObjectAndCursor();
        CollisionBetweenCurrentObjectAndStartColliders();
        if (isTriggered == true)
        {
            var pos = this.gameObject.transform.position;
            pos.x += (offcetX);
            pos.y += (offcetY);
            this.gameObject.transform.position = pos;
            if (this.Objectpicked)
            {
                this.timeSinceStart1 += Time.deltaTime;
                if (timeSinceStart1 >= interval)
                {
                    this.timeSinceStart1 = 0;
                    Objectpicked = false;
                }
                return;
            }
            bool collision = CollisionBetweenCurrentObjectAndTargetPosition();
            if (collision)
            {
                return;
            }
            
            return;
        }
    }

    private void CollisionBetweenCurrentObjectAndStartColliders()
    {
        var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.StartLayer);
        if (Triggered != null)
        {
            if (this.isStartReached)
            {

                return;
            }

            if (GameManegmentHelper.isColliding.Contains(Triggered.name))
            {
                return;
            }
            GameManegmentHelper.isColliding.Add(Triggered.name);
            startColliderName = Triggered.name;
            this.gameObject.transform.position = Triggered.transform.position;
            this.destinationReached = true;
            this.isTargetReached = true;
            GameManegmentHelper.order.Push(
               new UndoInformation()
               {
                   ObjectName = this.gameObject.name,
                   X = startX,
                   Y = startY
               });
        }
        else
        {
            RemoveComponentFromHashSet(GameManegmentHelper.isColliding, startColliderName);
            startColliderName = String.Empty;
            this.isTargetReached = false;
        }
        return;
    }

    public bool CollisionBetweenCurrentObjectAndCursor()
    {
        var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.PlayerLayer);
        if (Triggered != null)
        {
            if (this.isCursorReached)
            {
                setObjectCoord(Triggered);
                return true;
            }

            if (GameManegmentHelper.isColliding.Contains(Triggered.name))
            {
                return false;
                
            }

            cursorColliderName = Triggered.name;
            GameManegmentHelper.isColliding.Add(Triggered.name);
            this.isCursorReached = true;
            this.isTriggered = true;
            this.isCursorReached = true;
            this.Objectpicked = true;
            this.offcetX = this.cursorCoordinates.position.x - this.gameObject.transform.position.x;
            this.offcetY = this.cursorCoordinates.position.y - this.gameObject.transform.position.y;

            return true;
        }
        else
        {
            RemoveComponentFromHashSet(GameManegmentHelper.isColliding, cursorColliderName);
            cursorColliderName = String.Empty;
            isTriggered = false;
            this.isCursorReached = false;
            this.Objectpicked = false;
        }
        return false;
    }

    private void setObjectCoord(Collider2D Triggered)
    {
        cursorColliderName = Triggered.name;
        this.isTriggered = true;
        this.offcetX = this.cursorCoordinates.position.x - this.gameObject.transform.position.x;
        this.offcetY = this.cursorCoordinates.position.y - this.gameObject.transform.position.y;
    }

    public bool CollisionBetweenCurrentObjectAndTargetPosition()
    {
        var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.TargetLayer);
        if (Triggered != null)
        {
            if (this.isTargetReached)
            {
                
                return true;
            }

            if (GameManegmentHelper.isColliding.Contains(Triggered.name))
            {
                return false;
            }
            GameManegmentHelper.isColliding.Add(Triggered.name);
            targetColliderName = Triggered.name;
            this.gameObject.transform.position = Triggered.transform.position;
            this.isTriggered = false;
            this.destinationReached = true;
            this.isTargetReached = true;
            ResultCollector.UserResults.Add(this.gameObject.name, Triggered.name);
            ResultCollector.UserResults.Add(Triggered.name, this.gameObject.name);
            GameManegmentHelper.order.Push(
               new UndoInformation()
               {
                   ObjectName = this.gameObject.name,
                   X = startX,
                   Y = startY
               });
        }
        else
        {
            RemoveComponentFromHashSet(GameManegmentHelper.isColliding,targetColliderName);
            RemoveStringComponentFromDictionary(ResultCollector.UserResults, targetColliderName);
            RemoveStringComponentFromDictionary(ResultCollector.UserResults, this.gameObject.name);
            targetColliderName = String.Empty;
            isTriggered = false;
            this.isTargetReached = false;
        }
        return isTriggered;
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

    private void RemoveStringComponentFromDictionary(Dictionary<string,string> dictionary, string key )
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