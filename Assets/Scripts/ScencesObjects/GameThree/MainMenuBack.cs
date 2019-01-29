using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBack : MonoBehaviour
{

    private Collider2D mainCollider;

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
    private void Start()
    {
        this.isTriggered = false;
        mainCollider = this.gameObject.GetComponent<Collider2D>();
        Debug.Log(mainCollider);
        gameEnd = false;
        GameManegmentHelper.isHandCursorCatchingObject = false;
        CantChangeCursorSprite = true;
        CantChangeObjectScale = true;
        isHandCursorCatchingObject = false;
        InitCursor();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameManegmentHelper.GameThreeFinished)
        {
            return;
        }

        CollisionBetweenCurrentObjectAndCursor();
        if (isTriggered == true)
        {
            BackToMainMenu();
            gameEnd = true;
        }
    }

    private void BackToMainMenu()
    {
        //SceneManager.LoadScene("StartScene");
        GameManegmentHelper.Exit();
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