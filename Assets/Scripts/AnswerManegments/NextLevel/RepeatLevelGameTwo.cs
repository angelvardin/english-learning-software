using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RepeatLevelGameTwo : MonoBehaviour
{
    private int numberLevels = 1;
    private int currentLevel = -1;
    public GameObject NextLevelScreen;
    public GameObject waitting;
    public GameObject FinishScreen;
    public GameObject UIMenu;
    public float interval = 0.9f;
    private float currentTime = 0f;
    public Sprite present;
    public string presentTag = "answer";
    private bool dontRepeat = false;

    private RepeatLevelClass[] levels;

    // Use this for initialization
    private void Start()
    {
        waitting.SetActive(false);
        FinishScreen.SetActive(false);
        NextLevelScreen.SetActive(false);
        //UIMenu.SetActive(false);
        GameManegmentHelper.isLevelEnd = false;
        InitAnswers();

        GameManegmentHelper.CurrentLevelOverGameTwo = false;
        //GetAndInitCorrectAnswers();
    }

    private void InitAnswers()
    {
        levels = new RepeatLevelClass[numberLevels];
        levels[0] = new RepeatLevelClass()
        {
            Level = 0,
            Answers = new Dictionary<string, KeyValuePair<string, bool>>()
            {
                {"1", new KeyValuePair<string,bool>("Bike",true) },
                {"2",new KeyValuePair<string,bool>("Chair",false) },
                {"3",new KeyValuePair<string,bool>("Table",false) },
            },
            Labels = new List<string>()
            {
                "Bike",
                "Chair",
                "Table",
            }
        };
        //levels[1] = new RepeatLevelClass()
        //{
        //    Level = 1,
        //    Answers = new Dictionary<string, KeyValuePair<string, bool>>()
        //    {
        //        {"1", new KeyValuePair<string,bool>("First",false) },
        //        {"2",new KeyValuePair<string,bool>("Second",false) },
        //        {"3",new KeyValuePair<string,bool>("Third",true) },
        //    },
        //    Labels = new List<string>()
        //    {
        //        "First",
        //        "Second",
        //        "Third",
        //        "Foud",
        //        "Fift"
        //    }
        //};
        //levels[2] = new RepeatLevelClass()
        //{
        //    Level = 2,
        //    Answers = new Dictionary<string, KeyValuePair<string, bool>>()
        //    {
        //        {"1", new KeyValuePair<string,bool>("First",false) },
        //        {"2",new KeyValuePair<string,bool>("Second",true) },
        //        {"3",new KeyValuePair<string,bool>("Third",false) },
        //    },
        //    Labels = new List<string>()
        //    {
        //        "First",
        //        "Second",
        //        "Third",
        //        "Foud",
        //        "Fift"
        //    }
        //};
    }

    //public void BackToMainMenu()
    //{
    //    SceneManager.LoadScene("StartScene");
    //}

    public void Update()
    {
        if (GameManegmentHelper.CurrentLevelOverGameTwo)
        {
            currentTime += Time.deltaTime;
            if (currentTime > interval && dontRepeat == false)
            {
                currentTime = 0f;

                //Debug.Log("YessSSSSSSSSSSSSS "+changeQuestions.name);
                GameManegmentHelper.isUI = true;
                GameManegmentHelper.isLevelEnd = true;
                dontRepeat = true;
                if (currentLevel + 1 == numberLevels)
                {
                    FinishScreen.SetActive(true);
                    return;
                }
                NextLevelScreen.SetActive(true);
            }
        }
        else
        {
            currentTime = 0f;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void NextLevel()
    {
        //Debug.Log("Starting");
        waitting.SetActive(true);
        currentLevel++;
        GameManegmentHelper.GameTwoAnswers.Clear();
        foreach (var item in levels[currentLevel].Answers)
        {
            GameManegmentHelper.GameTwoAnswers.Add(item.Key, item.Value.Value);
            //Debug.Log(item.Key + "  " + item.Value.Value);
        }
        UpdateLabelsAsync(levels[currentLevel]);

        SetPresentStartSpritesAsync(levels[currentLevel]);

        GetAndInitCorrectAnswers();

        GameManegmentHelper.GameTwoLevel++;
        waitting.SetActive(false);
        NextLevelScreen.SetActive(false);
        GameManegmentHelper.isUI = false;
        dontRepeat = false;
        GameManegmentHelper.isLevelEnd = false;

        var pos = this.gameObject.transform.position;
        pos.x = -0.16f;
        pos.y = -2.82f;
        this.gameObject.transform.position = pos;
        //float scaleCoef = -0.1f;
        //ScaleObject(scaleCoef, scaleCoef, this.gameObject);
        Debug.Log("1 Scaled");
        ScaleObjSync();
        Debug.Log("2 Scaled");

        var mouseController = this.gameObject.GetComponent<MouseGameTwoController>();
        mouseController.NewLevel();
    }

    private IEnumerator ScaleAnswers()
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            var answers = GameObject.FindGameObjectsWithTag("answer");
            Debug.Log("3 Scaled");
            foreach (var item in answers)
            {
                if (item.GetComponent<GameTwoPresentController>().isScaled==true)
                {
                    var scale = item.transform.localScale;
                    scale.x = 2.1f;
                    scale.y = 2.1f;
                    item.transform.localScale = scale;
                    Debug.Log("Scaled");
                    break;
                }
            }
            workDone = true;
        }
    }
    public void ScaleObjSync()
    {
        var answers = GameObject.FindGameObjectsWithTag("answer");
        Debug.Log("3 Scaled");
        foreach (var item in answers)
        {
            var comp = item.GetComponent<GameTwoPresentController>();
            if (comp.isScaled == true)
            {
                var scale = item.transform.localScale;
                scale.x = 2.1f;
                scale.y = 2.1f;
                item.transform.localScale = scale;
                Debug.Log("Scaled");
                comp.isScaled = false;
                break;
            }
        }
    }
    public  void ScaleObject(float scaleCoeficientX, float scaleCoeficientY, GameObject obj)
    {
        
        Vector3 scale = obj.transform.localScale;
        scale.x += scaleCoeficientX;
        scale.y += scaleCoeficientY;
        obj.transform.localScale = scale;
    }
    private void SetPresentStartSprites(RepeatLevelClass level)
    {
        foreach (var item in level.Answers)
        {
            var go = GameObject.Find(item.Key);
            SpriteRenderer spriteRndr = go.GetComponent<SpriteRenderer>();
            spriteRndr.sprite = this.present;
            Vector3 pos = GameManegmentHelper.gameTwoPresentsStartPosition.ContainsKey(go.name) ? GameManegmentHelper.gameTwoPresentsStartPosition[go.name] : new Vector3();
            GameManegmentHelper.SetObjectPosition(go, pos);
        }
    }

    private void SetPresentStartSpritesAsync(RepeatLevelClass level)
    {
        foreach (var item in level.Answers)
        {
            StartCoroutine(YieldingUpdateSprites(item));
        }
    }

    private IEnumerator YieldingUpdateSprites(KeyValuePair<string, KeyValuePair<string, bool>> item)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            var go = GameObject.Find(item.Key);
            SpriteRenderer spriteRndr = go.GetComponent<SpriteRenderer>();
            spriteRndr.sprite = this.present;
            Vector3 pos = GameManegmentHelper.gameTwoPresentsStartPosition.ContainsKey(go.name) ? GameManegmentHelper.gameTwoPresentsStartPosition[go.name] : new Vector3();
            GameManegmentHelper.SetObjectPosition(go, pos);
            workDone = true;
            // Do Work...
        }
    }

    private void GetAndInitCorrectAnswers()
    {
        
        GameManegmentHelper.CurrentLevelOverGameTwo = false;
    }

    private void UpdateLabels(RepeatLevelClass level)
    {
        // Debug.Log("Labels");
        foreach (var item in level.Answers)
        {
            var go = GameObject.Find(item.Key);
            //Debug.Log(go.name);
            var sl = go.transform.Find("LableText");
            //Debug.Log(sl.name);

            var text = sl.transform.Find("Text");
            //Debug.Log(text);

            var textComponent = text.gameObject.GetComponent<Text>();
            //Debug.Log(textComponent.text);
            textComponent.text = item.Value.Key;
        }
    }

    private void UpdateLabelsAsync(RepeatLevelClass level)
    {
        //var options = new ParallelOptions();
        // var task = new UnityEditor.VersionControl.Task(() => { });
        // Debug.Log("Labels");
        foreach (var item in level.Answers)
        {
            StartCoroutine(YieldingUpdateLableText(item));
        }
    }

    private IEnumerator YieldingUpdateLableText(KeyValuePair<string, KeyValuePair<string, bool>> item)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            var go = GameObject.Find(item.Key);
            var sl = go.transform.Find("LableText");
            var text = sl.transform.Find("Text");
            var textComponent = text.gameObject.GetComponent<Text>();
            textComponent.text = item.Value.Key;
            workDone = true;
        }
    }
}