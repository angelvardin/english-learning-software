using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropLetter : DragAndDropObject
{
    protected float startX;
    protected float startY;

    public override void InitComponents()
    {
        startX = this.gameObject.transform.position.x;
        startY = this.gameObject.transform.position.y;
    }


    public override void OnDragAndDropProcessProssiding()
    {
        
    }

    public override void OnTargetReached(Collider2D triggered)
    {
        base.OnTargetReached(triggered);
        StartCoroutine(SetAnswer(targetColliderName));
        GameManegmentHelper.order.Push(
           new UndoInformation()
           {
               ObjectName = this.gameObject.name,
               X = startX,
               Y = startY
           });
    }

    public override void OnExitTriggeringTarget()
    {
        base.OnExitTriggeringTarget();
        RemovedAnswerSync(targetColliderName);

    }

    public override void BeforeDragButton()
    {
        base.BeforeDragButton();
        if (GameManegmentHelper.GameFourFinished)
        {
            return;
        }
    }

    private void RemovedAnswerSync(string name)
    {
        var component = this.gameObject.GetComponent<InitGameFourAnswers>();
        component.GivenAnswerName = String.Empty;
        component.WordNumber = -1;
    }

    private IEnumerator SetAnswer(string name)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            var component = this.gameObject.GetComponent<InitGameFourAnswers>();
            component.GivenAnswerName = name;
            //component.WordNumber
            var target = targetColliderGameObject;
            var number = targetColliderGameObject.GetComponent<AnswerCheckerGameObject>();
            component.WordNumber = number.Number;
            workDone = true;
        }
    }

    

    

}
