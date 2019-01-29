using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public abstract class HandCursor : MonoBehaviour {

    protected Collider2D mainCollider;

    public LayerMask PlayerLayer;
    protected bool stayPressed = false;
    public LayerMask TargetLayer;
    public float overlap = 0.5f;
    public bool scaleButton = true;
    public float scaleCoeficient = 0.1f;
    protected GameObject cursor;
    public bool presiceHandState = true;
    protected bool isDradAndDrop = false;

    protected bool isTriggered;

    protected bool TargetReached = false;
    protected bool StayOnTarget = false;
    
    protected bool WaitForPress = false;
    protected bool DisablePress = false;
    protected bool colliding = false;
    

    protected bool isStartPressing;
    protected bool CantChangeObjectScale;
    protected bool CantChangeCursorSprite;
    protected bool FirstTimeCursorClickedWithClosedHand;
    protected bool firstTimeCursorTriggeredWithOpenHand;
    protected string cursorColliderName;
    protected string targetColliderName;
    protected GameObject targetColliderGameObject;
    protected bool scaleCursor = true;
    

    protected bool followCoord = true;


    protected float offcetX;
    protected float offcetY;
    protected Transform cursorCoordinates;

    public float waittingTime = 1f;
    protected float currentTime = 0f;

    // Use this for initialization
    public void Start()
    {
        this.isTriggered = false;
        this.cursor = GameObject.FindGameObjectWithTag("HandCursor");
        mainCollider = this.gameObject.GetComponent<Collider2D>();
        Debug.Log(PlayerLayer.value);
        cursorCoordinates = cursor.gameObject.transform;
        CantChangeCursorSprite = true;
        CantChangeObjectScale = true;
        InitCursor();
        InitComponents();
        GameManegmentHelper.isHandCursorCatchingObject = false;
    }

    public abstract void InitComponents();

    // Update is called once per frame
    

    public virtual void AfterUpdate()
    {
    }

    public virtual void BeforeUpdate()
    {

    }

    

    public bool CollisionBetweenCurrentObjectAndCursor()
    {
        var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.PlayerLayer);
        if (Triggered != null)
        {
            //Debug.Log("CursorFunction");
            //Debug.Log("Yes!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + GameManegmentHelper.isHandCursorHovering);
            CursorFunction(Triggered);
        }
        else
        {
            OnExitTrigerringButton();
        }
        return false;
    }






    protected void OnButtonTriggered()
    {
    }

    

   

    

    public virtual void OnExitTrigerringButton()
    {
        if (CantChangeObjectScale == false)
        {
            CantChangeObjectScale = true;
            ScaleObject(-scaleCoeficient, this.gameObject);
        }
    
        isStartPressing = false;
        stayPressed = false;
        if (isDradAndDrop)
        {
            // RemoveComponentFromHashSet(GameManegmentHelper.isColliding, cursorColliderName);
            this.RemoveStringComponentFromDictionary(GameManegmentHelper.Colliding, cursorColliderName);
        }
        InitCursor();
    }

    protected void RemoveComponentFromHashSet(HashSet<string> hashset, string value)
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

    protected void InitCursor()
    {
        this.firstTimeCursorTriggeredWithOpenHand = true;
        this.FirstTimeCursorClickedWithClosedHand = true;
        this.isTriggered = false;
        this.cursorColliderName = String.Empty;
    }

    protected void CursorFunction(Collider2D triggered)
    {
        //Debug.Log("cursor function");
        bool handstateClosed;
        bool unknownHandState = GameManegmentHelper.handState == HandState.Unknown;
        bool notTrackedHandState = GameManegmentHelper.handState == HandState.NotTracked;

        if (presiceHandState)
        {
            handstateClosed = GameManegmentHelper.handState == HandState.Closed;
        }
        else
        {
            handstateClosed = GameManegmentHelper.handState != HandState.Open;
            unknownHandState = false;
            notTrackedHandState = false;
        }

        if (GameManegmentHelper.handState == HandState.Open && firstTimeCursorTriggeredWithOpenHand)
        {
            //Debug.Log("FirstTriggered");
            StartButtonTriggeringWithOpenHand(triggered);
            return;
        }
        else if (handstateClosed && firstTimeCursorTriggeredWithOpenHand)
        {
            StartButtonTriggeringWithClosedHand(triggered);
            return;
        }
        else if (unknownHandState && firstTimeCursorTriggeredWithOpenHand)
        {
            StartButtonTriggeringWithUnknownHandState(triggered);
            return;
        }
        else if (notTrackedHandState && firstTimeCursorTriggeredWithOpenHand)
        {
            StartButtonTriggeringWithNotTrckedHandState(triggered);
            return;
        }
        else if (!firstTimeCursorTriggeredWithOpenHand && handstateClosed)
        {
            if (this.FirstTimeCursorClickedWithClosedHand)
            {
                if (CheckIfObjectIsPicked(triggered.name))
                {
                    return;
                }
                //Debug.Log("FirstPressed");
                OnStartClicking(triggered);
                //this.CantPeekAnswerWords = true;
                //setObjectCoord();
            }
            else
            {
                OnStayClicking(triggered);
                //setObjectCoord();
            }
            return;
        }
        else if (!firstTimeCursorTriggeredWithOpenHand && GameManegmentHelper.handState == HandState.Open)
        {
            OnStayTriggeringButtonWithOpenHand(triggered);

            return;
        }
        else if (!firstTimeCursorTriggeredWithOpenHand && unknownHandState)
        {
            OnStayTriggeringButtonWithUnknownHandState(triggered);

            return;
        }
        else if (!firstTimeCursorTriggeredWithOpenHand && notTrackedHandState)
        {
            OnStayTriggeringButtonWithNotTrackedHandState(triggered);

            return;
        }
    }

    public virtual void OnStayTriggeringButtonWithNotTrackedHandState(Collider2D triggered)
    {
    }

    public virtual void StartButtonTriggeringWithNotTrckedHandState(Collider2D triggered)
    {
    }

    public virtual void OnStayTriggeringButtonWithUnknownHandState(Collider2D triggered)
    {
    }

    public virtual void StartButtonTriggeringWithUnknownHandState(Collider2D triggered)
    {
    }

    public virtual void StartButtonTriggeringWithClosedHand(Collider2D triggered)
    {
    }

    public virtual void OnStayTriggeringButtonWithOpenHand(Collider2D triggered)
    {
        isStartPressing = false;
        stayPressed = false;
        //if (CantChangeCursorSprite == false)
        //{
        //    //Debug.Log("Set false");
        //    //GameManegmentHelper.isHandCursorCatchingObject = false;
        //    CantChangeCursorSprite = true;
        //}
    }

    public virtual void StartButtonTriggeringWithOpenHand(Collider2D triggered)
    {
        CantChangeObjectScale = false;
        ScaleObject(scaleCoeficient, this.gameObject);
        firstTimeCursorTriggeredWithOpenHand = false;
        isTriggered = true;
    }

    public virtual void OnStayClicking(Collider2D triggered)
    {
        isStartPressing = true;
    }

    public virtual void OnStartClicking(Collider2D triggered)
    {
        //Debug.Log("Set true");
        CantChangeCursorSprite = false;
        cursorColliderName = triggered.name;
        if (isDradAndDrop)
        {
            //Debug.Log("-----------------------------------");
            //GameManegmentHelper.isColliding.Add(triggered.name);
            GameManegmentHelper.Colliding.Add(triggered.name, this.gameObject.name);
            // Debug.Log("Add "+triggered.name + " " + this.gameObject.name);
            //Debug.Log("-----------------------------------");
        }
        this.FirstTimeCursorClickedWithClosedHand = false;
        isTriggered = true;
        isStartPressing = true;
       // targetColliderGameObject = triggered.gameObject;
    }

    public virtual void ScaleObject(float scaleCoeficient, GameObject obj)
    {
        if (!scaleButton)
        {
            return;
        }
        Vector3 scale = obj.transform.localScale;
        scale.x += scaleCoeficient;
        scale.y += scaleCoeficient;
        obj.transform.localScale = scale;
    }

    public virtual bool CheckIfObjectIsPicked(string name)
    {
        if (GameManegmentHelper.Colliding.ContainsKey(name))
        {
            return true;
        }
        return false;
    }

    public virtual bool CheckIfObjectIsColliding(string name)
    {
        if (GameManegmentHelper.Colliding.ContainsKey(name))
        {
            return true;
        }
        return false;
    }

    public void RemoveStringComponentFromDictionary(Dictionary<string, string> dictionary, string key)
    {
        if (String.IsNullOrEmpty(key))
        {
            return;
        }
        if (dictionary.ContainsKey(key))
        {
            // Debug.Log("Remove "+key+"   "+dictionary[key]);
            dictionary.Remove(key);
        }
        //ShowDictionaryElements();
    }

    public void ShowDictionaryElements()
    {

        foreach (var item in GameManegmentHelper.Colliding)
        {
            Debug.Log("Printing " + item.Key + " " + item.Value);
        }


    }
}
