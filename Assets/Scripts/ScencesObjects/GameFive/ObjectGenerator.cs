using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.UI;

public class ObjectGenerator : MonoBehaviour
{
    public List<GameObject> availableObjects;
    public List<GameObject> currentObjects;
    public List<GameObject> objectsToRemove;

    private bool isCurrentObjectEmpty = false;

    private float minY;
    private float maxY;

    private float screenWidth;
    private float minDistanceBetweenObjs;

    private InitGameFiveAnswers answers;

    public void Start()
    {
        var height = Camera.main.orthographicSize;
        this.minY = -height / 2;
        this.maxY = height / 2;

        this.screenWidth = height * 2 * Camera.main.aspect;
        this.minDistanceBetweenObjs = this.screenWidth;
        answers = this.gameObject.GetComponent<InitGameFiveAnswers>();
        
    }

    public void Update()
    {
        var position = this.transform.position;

        var maxOffsetX = position.x + this.screenWidth;
        var minOffsetX = position.x - this.screenWidth;

        var farthestDistanceX = 0.0f;

        objectsToRemove = new List<GameObject>();

        foreach (var obj in this.currentObjects)
        {
            var objCenterX = obj.transform.position.x;
            farthestDistanceX = Mathf.Max(farthestDistanceX, objCenterX);

            if (objCenterX < minOffsetX)
            {
                objectsToRemove.Add(obj);
            }
        }

        if (minDistanceBetweenObjs >= this.screenWidth / 3)
        {
            this.minDistanceBetweenObjs -= 0.001f;
        }
       
        foreach (var obj in objectsToRemove)
        {
            
            //DestroyObject(obj);
            StartCoroutine(DestroyObjectAsync(obj));
            
        }

        if (farthestDistanceX < maxOffsetX)
        {
           // Debug.Log("Answers" + answers.Count);
            if (answers.index < answers.Count-1)
            {
                this.AddObject(farthestDistanceX + this.minDistanceBetweenObjs);

            }
            else
            {
                GameManegmentHelper.isGameFiveOver = true;
            }
        }
    }

    public void DestroyObject(GameObject obj)
    {
        if (obj==null)
        {
            return;
        }
        if (obj.transform.childCount > 0)
        {
           
            var component = obj.transform.GetChild(0).GetChild(0).GetComponent<TextController>();
            if (component != null)
            {
                if (component.CanBeDestroied == false && component.isChoosen == true)
                {
                    return;
                }
            }
        }
        
        //.Log(component);
        var isRemoved = this.currentObjects.Remove(obj.gameObject);
        //Debug.Log("removed"+isRemoved+obj.name);
        Destroy(obj);
    }

    private IEnumerator DestroyObjectAsync(GameObject obj)
    {

        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            DestroyObject(obj);
            workDone = true;
        }
    }



    public void AddObject(float maxDistanceX)
    {

        var randomObjIndex = UnityEngine.Random.Range(0, this.availableObjects.Count);
        var obj = Instantiate(this.availableObjects[randomObjIndex]);
        this.currentObjects.Add(obj);

        var position = obj.transform.position;
        position.x = maxDistanceX;
        position.y = UnityEngine.Random.Range(this.minY, this.maxY);
        obj.transform.position = position;
        if (obj.gameObject.CompareTag("coin"))
        {
            StartCoroutine(SetCoinLableAsync(obj));
            //SetCoinLable(obj);
        }
    }

    private IEnumerator SetCoinLableAsync(GameObject go)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;

            var sl = go.transform.Find("LableText");
            if (sl!=null)
            {
                var text = sl.transform.Find("Text");
                if (text!=null)
                {
                    var textComponent = text.gameObject.GetComponent<Text>();
                    if (answers.index < answers.Count)
                    {
                        string txt = answers.listOfAnswers[answers.index];
                        answers.index++;
                        textComponent.text = txt;
                    }
                }
                
            }
            workDone = true;
        }
    }
    private void SetCoinLable(GameObject go)
    {
        var sl = go.transform.Find("LableText");
        if (sl != null)
        {
            var text = sl.transform.Find("Text");
            if (text != null)
            {
                var textComponent = text.gameObject.GetComponent<Text>();
                if (answers.index < answers.Count)
                {
                    string txt = answers.listOfAnswers[answers.index];
                    answers.index++;
                    textComponent.text = txt;
                }
            }

        }
    }
}
