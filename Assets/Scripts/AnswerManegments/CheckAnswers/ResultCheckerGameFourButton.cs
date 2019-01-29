using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultCheckerGameFourButton : GenericButton
{
    private bool gameEnd;
    public Sprite trueAnswer;
    public Sprite falseAnswer;
    public GameObject exitButton;
    public Sprite handSprite;
    public static int wordNumbers = 3;
    private bool[] isWordCorrect;
    private Dictionary<string, bool> trueWords = new Dictionary<string, bool>();
    // Use this for initialization
    public override void InitComponents()
    {
        GameManegmentHelper.GameFourFinished = false;
        gameEnd = false;
        exitButton.SetActive(false);
        isWordCorrect = new bool[wordNumbers+1];
        for (int i = 1; i <= wordNumbers; i++)
        {
            isWordCorrect[i] = true;
        }
    }

    public override void BeforePressingButton()
    {
        base.BeforePressingButton();
        if (GameManegmentHelper.GameFourFinished)
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
        if (GameManegmentHelper.GameFourFinished)
        {
            return;
        }
        CheckResults();
        gameEnd = true;
    }

    private void CheckResults()
    {
        
        var letters = GameObject.FindGameObjectsWithTag("answerword");
        var sprites = GameObject.FindGameObjectsWithTag("answersprite");
        foreach (var item in sprites)
        {
            if (trueWords.ContainsKey(item.name))
            {
                continue;
            }
            trueWords.Add(item.name, true);
        }
        foreach (var letter in letters)
        {
            var component = letter.GetComponent<InitGameFourAnswers>();
            if (String.IsNullOrEmpty(component.GivenAnswerName))
            {
                StartCoroutine(ChangeSpriteAsync(component.GivenAnswerName, falseAnswer));
                continue;
            }
            bool isTrue = component.CorrectAnswer.Where(x => x.name.Equals(component.GivenAnswerName)).FirstOrDefault(x => x)==null? false : true;
            if (!isTrue)
            {
               
                StartCoroutine(ChangeSpriteAsync(component.GivenAnswerName, falseAnswer));
               
            }
            
        }

        foreach (var item in sprites)
        {
            if (trueWords[item.name])
            {
                StartCoroutine(ChangeGameObjSpriteAsync(item, trueAnswer));
            }

            var scale = item.transform.localScale;
            scale.x = 2.1f;
            item.transform.localScale = new Vector3(2.1f, 1.1f, 1f);

        }
        gameEnd = true;
        GameManegmentHelper.GameFourFinished = true;
        exitButton.SetActive(true);
    }

    private IEnumerator ChangeGameObjSpriteAsync(GameObject obj, Sprite sprite)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            ChangeGameObjSprite(obj, sprite);

            workDone = true;
        }
    }

    private void ChangeGameObjSprite(GameObject obj, Sprite sprite)
    {
        if (obj != null)
        {
            //var component = obj.GetComponent<AnswerCheckerGameObject>();
            //var resultSprite = component.AnswerChecker;
            var sprRendr = obj.GetComponent<SpriteRenderer>();
            sprRendr.sprite = sprite;
            Debug.Log("Changed");
        }
    }

    private IEnumerator ChangeSpriteAsync(string word, Sprite sprite)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            
            var obj = GameObject.Find(word);
            if (obj != null)
            {
                var component = obj.GetComponent<AnswerCheckerGameObject>();
                var resultSprite = component.AnswerChecker;
                trueWords[resultSprite.name] = false;
                yield return null;
                var sprRendr = resultSprite.GetComponent<SpriteRenderer>();
                sprRendr.sprite = sprite;
                Debug.Log("Changed");

            }
            workDone = true;
        }
    }

    private void ChangeSprite(string word, Sprite sprite)
    {
        var obj = GameObject.Find(word);
        if (obj != null)
        {
            var component = obj.GetComponent<AnswerCheckerGameObject>();
            var resultSprite = component.AnswerChecker;
            trueWords[resultSprite.name] = false;
            var sprRendr = resultSprite.GetComponent<SpriteRenderer>();
            sprRendr.sprite = sprite;
            Debug.Log("Changed");

        }

    } 
}
