using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour {

    public float scaleCoeficient = 0.1f;
    public float time = 1f;
    private float currentTime=0f;
    public bool isPressing = false;
    public bool waitting = false;
    public bool stopPressing = false;
    public GameObject cursor;
    private bool scaleCursor = true;
    public bool executePressing = false;

    // Use this for initialization
    void Start () {
		
	}

    public  void PressProcces(GameObject gameObj, float scalecoef, float time)
    {
        if (scaleCursor)
        {
            ScaleObject(scalecoef);

        }
        PressWait(gameObj, time);
    }

    public  void PressWait(GameObject gameObj, float time)
    {
        if (currentTime<time)
        {
            currentTime += Time.deltaTime;
            return;
        }
        stopPressing = true;
        executePressing = true;
    }

    public void ScaleObject(float scaleCoeficient)
    {
        
        Vector3 scale = cursor.transform.localScale;
        scale.x += scaleCoeficient; // your new value
        scale.y += scaleCoeficient; // your new value
        cursor.transform.localScale = scale;
    }
    public void StopPressing(float scaleCoeficient)
    {

        ScaleObject(-scaleCoeficient);
        stopPressing = false;
        isPressing = false;
        executePressing = false;
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update () {
        if (stopPressing)
        {
            StopPressing(scaleCoeficient);
        }
        if (this.isPressing)
        {
            PressProcces(this.gameObject, scaleCoeficient, time);
        }
        
	}
}
