using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonController : MonoBehaviour {

    Collider2D mainCollider;
    public float overlap = 0.5f;
    public LayerMask playerLayer;
    public GameObject UIMenu;
    public GameObject cursor;
    public float interval = 0.5f;
    private float timeSinceStart;
    private float timeSinceStart1;

    bool firstTriggered = false;
    public bool scaleObject;
    private bool collisionPause = false;
    

    // Use this for initialization
    void Start () {
        timeSinceStart1 = 0;
        
        mainCollider = this.gameObject.GetComponent<Collider2D>();
        timeSinceStart = 0.0f;
        scaleObject = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (collisionPause)
        {
            if (timeSinceStart1<interval)
            {
                timeSinceStart1 += Time.deltaTime;

            }
            else
            {
                collisionPause = false;
                timeSinceStart1 = 0;
            }
            return;

        }
        CollisionWithCursor();
	}

    private void CollisionWithCursor()
    {
      
        var Triggered = Physics2D.OverlapCircle(mainCollider.transform.position, overlap, this.playerLayer);
        
        if (Triggered != null)
        {
            if (GameManegmentHelper.isColliding.Contains(Triggered.name))
            {
                return;
            }
            if (scaleObject)
            {
                scaleObject = false;
                ScaleObject(0.1f);
            }
            if (timeSinceStart<interval)
            {
                timeSinceStart += Time.deltaTime;
                return;
            }
            
                firstTriggered = true;

            
            UIMenu.SetActive(true);

            GameManegmentHelper.isUI = true;
        }
        else
        {
            scaleObject = true;
            if (firstTriggered)
            {
                firstTriggered = false;
                var pos = cursor.transform.position;
                pos.x -= 1;
                pos.y -= 1;
                cursor.transform.position = pos;
            }
            timeSinceStart = 0.0f;
            if (GameManegmentHelper.isLevelEnd==false)
            {
                GameManegmentHelper.isUI = false;


            }

            //if (UIMenu.active)
            //{
            //    ScaleObject(-0.1f);
            //    UIMenu.SetActive(false);
            //}
        }


    }

    public void PauseGame()
    {
        cursor.SetActive(false);
        UIMenu.SetActive(true);
    }
    public void UnPauseGame()
    {
        Debug.Log("Unpause Game!");
        GameManegmentHelper.isUI = false;
        collisionPause = true;
        UIMenu.SetActive(false);
    }

    private void ScaleObject(float scaleCoeficient)
    {
        Vector3 scale = transform.localScale;
        scale.x += scaleCoeficient; // your new value
        scale.y += scaleCoeficient; // your new value
        transform.localScale = scale;
    }
}
