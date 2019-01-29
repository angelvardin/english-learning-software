using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCheckerGameThreeButton : GenericButton {
    private bool gameEnd;
    public Sprite trueAnswer;
    public Sprite falseAnswer;
    public GameObject exitButton;
    public Sprite handSprite;

    public override void InitComponents()
    {
        GameManegmentHelper.GameThreeFinished = false;
        gameEnd = false;
        exitButton.SetActive(false);
    }

    public override void BeforePressingButton()
    {
        base.BeforePressingButton();
        if (GameManegmentHelper.GameThreeFinished)
        {
            return;
        }
        if (gameEnd)
        {
            return;
        }
    }

    public override void OnPressed()
    {
        CheckResults();
        gameEnd = true;
    }

    private void CheckResults()
    {
        var sentencesParts = GameObject.FindGameObjectsWithTag("answerword");
        SortedList<int, SortedList<int, string>> sentences = new SortedList<int, SortedList<int, string>>();

        foreach (var item in sentencesParts)
        {

            var component1 = item.GetComponent<InitGameThreeAnswers>();
            if (component1 != null)
            {


                string word = component1.GivenAnswerName;
                if (word.Equals(component1.CorrectAnswer.name))
                {
                    //word = " вярно!";
                    StartCoroutine(ChandeSpriteAsync(word, trueAnswer));
                    //ChandeSprite(word,trueAnswer);
                }
                else
                {
                    StartCoroutine(ChandeSpriteAsync(word, falseAnswer));

                    //word = " невярно!";
                }
                //StartCoroutine(YieldingUpdateLableText(item, word));

            }

        }


        //var spr = cursor.GetComponent<SpriteRenderer>();
        //spr.sprite = handSprite;
        exitButton.SetActive(true);
        GameManegmentHelper.GameThreeFinished = true;
        


    }

    private IEnumerator ChandeSpriteAsync(string word, Sprite sprite)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            ChandeSprite(word, sprite);
            workDone = true;
        }
    }

    private void ChandeSprite(string word, Sprite sprite)
    {
        var obj = GameObject.Find(word);
        if (obj!=null)
        {
            var component = obj.GetComponent<AnswerCheckerGameObject>();
            var resultSprite = component.AnswerChecker;
            var sprRendr = resultSprite.GetComponent<SpriteRenderer>();
            sprRendr.sprite = sprite;
            Debug.Log("Changed");

        }
        
    }

    private IEnumerator YieldingUpdateLableText(GameObject go, string labletext)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;

            var sl = go.transform.Find("LableText");
            var text = sl.transform.Find("Text");
            var textComponent = text.gameObject.GetComponent<Text>();
            string word = textComponent.text + labletext;
            Debug.Log("word");
            textComponent.text = word;
            workDone = true;
        }
    }

    // Use this for initialization

}
