using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public abstract class GenericButton : MonoBehaviour
{
    protected Collider2D mainCollider;

    public LayerMask PlayerLayer;

    public LayerMask TargetLayer;
    public float overlap = 0.5f;
    public bool scaleButton = true;
    public float scaleCoeficient = 0.1f;
    public GameObject cursor;
    public bool presiceHandState = true;
    public bool isDradAndDrop = false;

    protected bool isTriggered;

    protected bool TargetReached = false;
    protected bool StayOnTarget = false;
    protected bool WaitForDrag = false;
    protected bool WaitForPress = false;
    protected bool DisablePress = false;
    protected bool colliding = false;
    protected bool afterMoving = false;

    protected bool isStartPressing;
    protected bool CantChangeObjectScale;
    protected bool CantChangeCursorSprite;
    protected bool FirstTimeCursorClickedWithClosedHand;
    protected bool firstTimeCursorTriggeredWithOpenHand;
    protected string cursorColliderName;
    protected string targetColliderName;
    protected GameObject targetColliderGameObject;
    protected bool scaleCursor = true;
    protected bool stayPressed = false;

    protected bool followCoord = true;

    
    protected float offcetX;
    protected float offcetY;
    protected Transform cursorCoordinates;

    public float time = 1f;
    protected float currentTime = 0f;

    // Use this for initialization
    public void Start()
    {
        this.isTriggered = false;
        mainCollider = this.gameObject.GetComponent<Collider2D>();
        //Debug.Log(mainCollider);
        cursorCoordinates = cursor.gameObject.transform;
        CantChangeCursorSprite = true;
        CantChangeObjectScale = true;
        InitCursor();
        InitComponents();
        GameManegmentHelper.isHandCursorCatchingObject = false;
    }

    public abstract void InitComponents();

    // Update is called once per frame
    public virtual void Update()
    {
        BeforeUpdate();
        if (isDradAndDrop)
        {
            this.DragAndDropProcess();
        }
        else
        {
            this.PressProcess();
        }
        AfterUpdate();

    }

    public virtual void AfterUpdate()
    {
    }

    public virtual void BeforeUpdate()
    {
        
    }

    public virtual void PressProcess()
    {
        if (this.DisablePress)
        {
            return;
        }
        BeforePressingButton();
        CollisionBetweenCurrentObjectAndCursor();
        if (isTriggered == true)
        {
            OnButtonTriggered();
            //Debug.Log("Triggered");
            if (isStartPressing == true)
            {
                //Debug.Log("Is Pressed");
                if (stayPressed)
                {
                    return;
                }

                WaittingForPressing();
                OnPressed();
                //stayPressed = true;
            }
            else
            {
                if (CantChangeCursorSprite == false)
                {
                    GameManegmentHelper.isHandCursorCatchingObject = false;
                    //ScaleObject(-scaleCoeficient, cursor.gameObject);
                    CantChangeCursorSprite = true;
                }
            }
            //if (pressbutton==null)
            //{
            //    if (pressbutton.executePressing == false)
            //    {
            //        return;

            //    }
            //    pressbutton.executePressing = false;

            //}
        }
        else
        {
            if (CantChangeCursorSprite == false)
            {
                GameManegmentHelper.isHandCursorCatchingObject = false;
                //ScaleObject(-scaleCoeficient, cursor.gameObject);
                CantChangeCursorSprite = true;
            }
        }
    }

    public virtual void BeforePressingButton()
    {
        
    }

    public virtual void DragAndDropProcess()
    {
        if (WaitForDrag)
        {
            if (currentTime < time)
            {
                currentTime += Time.deltaTime;
                return;
            }
            currentTime = 0f;
            WaitForDrag = false;
        }
        BeforeDragButton();
        //StayTriggeredWithTarget();
        CollisionBetweenCurrentObjectAndCursor();
        if (isTriggered == true)
        {
            OnButtonTriggered();
            //Debug.Log("Triggered");
            if (isStartPressing == true)
            {
                CheckIfTargetIsReached();
                //Debug.Log("Is Pressed");
                SetNewCoordinates();
                GameManegmentHelper.isHandCursorCatchingObject = true;
                afterMoving = true;
                OnDroped();
            }
            else
            {
                if (CantChangeCursorSprite == false)
                {
                    GameManegmentHelper.isHandCursorCatchingObject = false;
                    //ScaleObject(-scaleCoeficient, cursor.gameObject);
                    CantChangeCursorSprite = true;
                }
                CheckIfIsStillColliding();
            }
        }
        else
        {
            if (CantChangeCursorSprite == false)
            {
                GameManegmentHelper.isHandCursorCatchingObject = false;
                //ScaleObject(-scaleCoeficient, cursor.gameObject);
                CantChangeCursorSprite = true;
            }

            if (CheckIfIsStillColliding()==false)
            {
                OnExitTriggeringTarget();
            }
            //CheckIfIsStillColliding();
        }
    }

    public virtual void BeforeDragButton()
    {
        
    }

    public virtual void OnDroped()
    {
    }

    public virtual bool CheckIfIsStillColliding()
    {
        if (afterMoving == false)
        {
            return true;
        }
        afterMoving = false;
        var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.TargetLayer);
        if (Triggered != null)
        {
            if (GameManegmentHelper.Colliding.ContainsKey(Triggered.name))
            {
                if (GameManegmentHelper.Colliding[Triggered.name].Equals(this.gameObject.name))
                {
                    targetColliderName = Triggered.name;
                    targetColliderGameObject = Triggered.gameObject;
                    StartCoroutine(SetTargetCoordAsync(targetColliderName));
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        
            return false;
        
    }

    private void CheckIfTargetIsReached()
    {
        var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.TargetLayer);
        if (Triggered != null)
        {
            if (colliding)
            {
                return;
            }
            if (CheckIfObjectIsPicked(Triggered.name))
            {
                return;
            }
            //GameManegmentHelper.isHandCursorCatchingObject = true;
            OnTargetReached(Triggered);
        }
        else
        {
            OnExitTriggeringTarget();
        }
    }

    public virtual void OnExitTriggeringTarget()
    {
        if (String.IsNullOrEmpty(targetColliderName))
        {
            return;
        }
       // Debug.Log("Remove "+targetColliderName);
        colliding = false;
        RemoveStringComponentFromDictionary(GameManegmentHelper.Colliding, targetColliderName);
        targetColliderName = String.Empty;
        targetColliderGameObject = null;
    }

    private void OnTargetReached(Collider2D triggered)
    {
        GameManegmentHelper.Colliding.Add(triggered.name, this.gameObject.name);
        this.isStartPressing = false;
        this.isTriggered = false;
        WaitForDrag = true;
        TargetReached = true;
        this.targetColliderName = triggered.name;
        targetColliderGameObject = triggered.gameObject;
        this.colliding = true;
        //Debug.Log("-----------------------------------");
        //Debug.Log("Add " + targetColliderName+" " + this.gameObject.name);
        //Debug.Log("-----------------------------------");

        StartCoroutine(SetTargetCoordAsync(targetColliderName));

        //ScaleObject(scaleCoeficient, cursor);
        CantChangeCursorSprite = false;
    }

    public void SetTargetCoord(string targetName)
    {
        //var obj = GameObject.Find(targetName);
        var obj = targetColliderGameObject;
        var pos = obj.transform.position;
        this.gameObject.transform.position = pos;
    }

    public IEnumerator SetTargetCoordAsync(string targetName)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            SetTargetCoord(targetName);
            workDone = true;
        }
    }

    protected void SetNewCoordinates()
    {
        this.offcetX = this.cursorCoordinates.position.x - this.gameObject.transform.position.x;
        this.offcetY = this.cursorCoordinates.position.y - this.gameObject.transform.position.y;
        var pos = this.gameObject.transform.position;
        pos.x += (offcetX);
        pos.y += (offcetY);
        this.gameObject.transform.position = pos;
    }

    public IEnumerator setNewCoordinatesAsync()
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            SetNewCoordinates();
            workDone = true;
        }
    }

    protected void OnButtonTriggered()
    {
    }

    public void WaittingForPressing()
    {
        if (scaleCursor)
        {
            GameManegmentHelper.isHandCursorCatchingObject = true;
            //ScaleObject(scaleCoeficient,cursor);
            CantChangeCursorSprite = false;
            scaleCursor = false;
        }
        if (currentTime < time)
        {
            currentTime += Time.deltaTime;
            return;
        }
        currentTime = 0f;
        scaleCursor = true;
    }

    public abstract void OnPressed();

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
            OnExitTrigerringButton();
        }
        return false;
    }

    public virtual void OnExitTrigerringButton()
    {
        if (CantChangeObjectScale == false)
        {
            CantChangeObjectScale = true;
            ScaleObject(-scaleCoeficient, this.gameObject);
        }
        //if (CantChangeCursorSprite == false)
        //{
        //    //ScaleObject(-scaleCoeficient, cursor.gameObject);
        //    CantChangeCursorSprite = true;
        //}
        //pressbutton.stopPressing = true;
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
            Debug.Log("Printing "+item.Key + " " + item.Value);
        }
      

    }
}