using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropButton : GenericButton {

    private bool isTargetReached;
  
    private bool CantPeekAnswerWords;
    private bool answerDestinationReached;

    protected float startX;
    protected float startY;

    public override void OnDroped()
    {
        StartCoroutine(SetAnswer(targetColliderName));
        //ResultCollector.UserResults.Add(this.gameObject.name, Triggered.name);
        //ResultCollector.UserResults.Add(Triggered.name, this.gameObject.name);
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
        if (GameManegmentHelper.GameThreeFinished)
        {
            return;
        }
    }

    private void RemovedAnswerSync(string name)
    {
        var component = this.gameObject.GetComponent<InitGameThreeAnswers>();
        component.GivenAnswerName = String.Empty;
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

    public override void OnPressed()
    {
        //throw new NotImplementedException();
    }

    public override void InitComponents()
    {
        startX = this.gameObject.transform.position.x;
        startY = this.gameObject.transform.position.y;
    }

    // Use this for initialization


    // Update is called once per frame

}
