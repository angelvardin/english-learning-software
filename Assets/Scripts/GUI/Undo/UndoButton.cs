using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoButton : GenericButton {
    public override void InitComponents()
    {
        
    }

    public override void OnPressed()
    {
       

        if (GameManegmentHelper.order.Count == 0)
        {
            return;
        }
        while (true)
        {
            if (GameManegmentHelper.order.Count == 0)
            {
                return;
            }

            var coord = (UndoInformation)GameManegmentHelper.order.Pop();

            var go = GameObject.Find(coord.ObjectName);
            if (go == null)
            {
                return;
            }
            var pos = go.transform.position;
            if (pos.x != coord.X && pos.y != coord.Y)
            {
                pos.x = coord.X;
                pos.y = coord.Y;
                go.transform.position = pos;
                break;
            }
            var answer = go.GetComponent<InitGameThreeAnswers>();
            if (answer.GivenAnswerName!=null)
            {
                if (GameManegmentHelper.isColliding.Contains(answer.GivenAnswerName))
                {
                    GameManegmentHelper.isColliding.Remove(answer.GivenAnswerName);
                }
                if (!answer.GivenAnswerName.Equals(string.Empty))
                {
                    answer.GivenAnswerName = string.Empty;
                }
            }
            

        }
    }

}
