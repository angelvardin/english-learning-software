using System;
using System.Collections;
using UnityEngine;

public abstract class DragAndDropObject : HandCursor
{
    protected bool WaitForDrag = false;
    protected bool CheckIfTargetIsReachedAfterDragAndDrop = false;

    public void Awake()
    {
        this.isDradAndDrop = true;
    }

    public virtual void Update()
    {
        BeforeUpdate();

        this.DragAndDropProcess();

        AfterUpdate();
    }

    public virtual void DragAndDropProcess()
    {
        if (WaitForDrag)
        {
            if (currentTime < waittingTime)
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
                CheckIfTargetIsReachedAfterDragAndDrop = true;
                // targetColliderName = Triggered.name;

                OnDragAndDropProcessProssiding();
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

            if (CheckIfIsStillColliding() == false)
            {
                OnExitTriggeringTarget();
            }
            //CheckIfIsStillColliding();
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

    public virtual void BeforeDragButton()
    {
    }

    public virtual void OnDragAndDropProcessProssiding()
    {
    }

    public virtual bool CheckIfIsStillColliding()
    {
        if (CheckIfTargetIsReachedAfterDragAndDrop == false)
        {
            return true;
        }
        CheckIfTargetIsReachedAfterDragAndDrop = false;
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

    public virtual void OnTargetReached(Collider2D triggered)
    {
        GameManegmentHelper.Colliding.Add(triggered.name, this.gameObject.name);
        this.isStartPressing = false;
        this.isTriggered = false;
        WaitForDrag = true;
        TargetReached = true;
        this.targetColliderName = triggered.name;
        targetColliderGameObject = triggered.gameObject;
        this.colliding = true;
        StartCoroutine(SetTargetCoordAsync(targetColliderName));
        //ScaleObject(scaleCoeficient, cursor);
        CantChangeCursorSprite = false;
    }

    public void SetTargetCoord(string targetName)
    {
        //var obj = GameObject.Find(targetName);
        var obj = targetColliderGameObject;
        Debug.Log("set targetColliderGameObject" + targetColliderGameObject.name);

        var pos = obj.transform.position;
        this.gameObject.transform.position = pos;
    }

    public IEnumerator SetTargetCoordAsync(string targetName)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.

            var obj = targetColliderGameObject;
            yield return null;
            var pos = obj.transform.position;
            this.gameObject.transform.position = pos;

            workDone = true;
        }
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

    protected void SetNewCoordinates()
    {
        this.offcetX = this.cursorCoordinates.position.x - this.gameObject.transform.position.x;
        this.offcetY = this.cursorCoordinates.position.y - this.gameObject.transform.position.y;
        var pos = this.gameObject.transform.position;
        pos.x += (offcetX);
        pos.y += (offcetY);
        this.gameObject.transform.position = pos;
    }
}