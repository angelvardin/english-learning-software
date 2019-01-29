using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameButton: HandCursor {

    public virtual void Update()
    {
        BeforeUpdate();

        this.PressProcess();


        AfterUpdate();

    }
    public virtual void BeforePressingButton()
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
        if (currentTime < waittingTime)
        {
            currentTime += Time.deltaTime;
            return;
        }
        currentTime = 0f;
        scaleCursor = true;
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
    public abstract void OnPressed();

}
