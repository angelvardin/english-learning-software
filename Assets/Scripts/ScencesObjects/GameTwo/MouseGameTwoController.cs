using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGameTwoController : GestureSourceManager
{
    public Transform GameObjectLeft;
    public Transform GameObjectRight;
    public Transform GameObjectMiddle;

    private bool isChoosen = false;
    private string choosenGesture;
    private Vector3 TargetPosition;
    private Renderer rndr;
    private float Coeficient;
    public float speed = 0.5f;
    public float offcetX = 0.5f;
    public float offcetY = 0.0f;


    private Vector3 StartPosition;

    private Dictionary<string, Vector2> functionCoordinates;

    // handup LeftHand_Left RightHand_Right TwoHandsUp


    public override void DetectedWork()
    {
        if (isChoosen)
        {
            //this._Sensor.Close();
            //this._BodySource = null;
            return;
        }

        if (CurrentDetectedGestureName.Equals("LeftHand_Left"))
        {
            Debug.Log("left hand detected");
            InitFunction("LeftHand_Left", this.gameObject.transform.position, this.GameObjectLeft.position,
                this.GameObjectLeft.position);

        }
        else if (CurrentDetectedGestureName.Equals("TwoHandsUp"))
        {
            Debug.Log("two hands up detected");
            InitFunction("TwoHandsUp", this.gameObject.transform.position, this.GameObjectMiddle.position,
                this.GameObjectMiddle.position);
        }
        else if (CurrentDetectedGestureName.Equals("TwoHandsUp"))
        {
            Debug.Log("two hands up detected");
            InitFunction("RightHand_Right", this.gameObject.transform.position, this.GameObjectRight.position,
                this.GameObjectRight.position);
        }

    }

    private void InitFunction(string GestureName, Vector3 pointX, Vector3 pointY, Vector3 target)
    {
        //functionCoordinates = new Dictionary<string, Vector2>();
        //Vector2 AB = CalculateAandBCoeficients(pointX, pointY);
        //functionCoordinates.Add(GestureName, AB);
        isChoosen = true;
        Debug.Log("Choosen");
        choosenGesture = GestureName;
        this.TargetPosition = target;
        this.TargetPosition.x += offcetX;
        this.TargetPosition.y += offcetY;
        //if(this.StartPosition.x>target.x)
        //{
        //    this.Coeficient = 1;
        //}else if(this.StartPosition.x<target.x)
        //{
        //    this.Coeficient = -1;
        //}
    }
    public override void InitComponents()
    {
        base.InitComponents();
        this.StartPosition = this.gameObject.transform.position;
        rndr = this.gameObject.GetComponent<Renderer>();
        rndr.enabled = false;
        NewLevel();
    }

    public void NewLevel()
    {
        this.isChoosen = false;
        this.choosenGesture = String.Empty;
    }

    public override void NotDetectedWork()
    {
       
    }

    public override void Update()
    {
        base.Update();
       
       
    }

    public  void FixedUpdate()
    {
        if (this.isPlayerDetected==false)
        {
            if (rndr.enabled==true)
            {
                rndr.enabled = false;
            }
            return;
        }
        else if (this.isPlayerDetected == true)
        {
            if (rndr.enabled == false)
            {
                rndr.enabled = true;
            }
        }

        if (!isChoosen)
        {
            //this.StartPosition = this.gameObject.transform.position;
            return;
        }
        float step = speed * Time.fixedDeltaTime;

       transform.position = Vector3.MoveTowards(transform.position, this.TargetPosition, step);

        //if (!isChoosen)
        //{
        //    //this.StartPosition = this.gameObject.transform.position;
        //    return;
        //}
        //float step = speed * Time.deltaTime;


        //Vector3 newPosition = this.gameObject.transform.position;
        //if (Vector3.Distance(this.TargetPosition,this.StartPosition) <
        //    Vector3.Distance(this.StartPosition,this.gameObject.transform.position))
        //{
        //    return;
        //}
        //newPosition.x += Time.fixedDeltaTime;
        //newPosition.y = newPosition.x * functionCoordinates[choosenGesture].x + functionCoordinates[choosenGesture].y;
        //this.transform.position = newPosition;


    }

    private Vector2 CalculateAandBCoeficients(Vector3 A, Vector3 B )
    {
        float a = (B.y - A.y) / (B.x - A.x);
        float b = A.y - ((A.x * (B.y - A.y)) / (B.x - A.x));

        return new Vector2(a, b);

    }

}
