using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//using System.Threading.Tasks;

public class RepeatLevelGameOne : MonoBehaviour
{
    private int numberLevels = 1;
    private int currentLevel = -1;
    public GameObject NextLevelScreen;
    public GameObject waitting;
    public GameObject FinishScreen;

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
        GameManegmentHelper.isLevelEnd = false;
        InitAnswers();
        GameManegmentHelper.gameOneGivenAnswers = 0;
        GameManegmentHelper.CurrentLevelOverGameOne = false;
        //GetAndInitCorrectAnswers();
    }

    /// <summary>
    /// 
    /// </summary>
    private void InitAnswers()
    {
        levels = new RepeatLevelClass[numberLevels];
        levels[0] = new RepeatLevelClass()
        {
            Level = 0,
            Answers = new Dictionary<string, KeyValuePair<string, bool>>()
            {
                {"1", new KeyValuePair<string,bool>("Chair",true) },
                {"2",new KeyValuePair<string,bool>("They",false) },
                {"3",new KeyValuePair<string,bool>("Table",true) },
                {"4",new KeyValuePair<string,bool>("Cat",false) },
                {"5",new KeyValuePair<string,bool>("Dog",false) },
            },
            Labels = new List<string>()
            {
                "Chair",
                "They",
                "Table",
                "Cat",
                "Dog"
            }
        };
        //levels[1] = new RepeatLevelClass()
        //{
        //    Level = 1,
        //    Answers = new Dictionary<string, KeyValuePair<string, bool>>()
        //    {
        //        {"1", new KeyValuePair<string,bool>("First",true) },
        //        {"2",new KeyValuePair<string,bool>("Second",false) },
        //        {"3",new KeyValuePair<string,bool>("Third",true) },
        //        {"4",new KeyValuePair<string,bool>("Fourth",false) },
        //        {"5",new KeyValuePair<string,bool>("Fifth",true) },
        //    },
        //    Labels = new List<string>()
        //    {
        //        "First",
        //        "Second",
        //        "Third",
        //        "Fourth",
        //        "Fifth"
        //    }
        //};
        //levels[2] = new RepeatLevelClass()
        //{
        //    Level = 2,
        //    Answers = new Dictionary<string, KeyValuePair<string, bool>>()
        //    {
        //        {"1", new KeyValuePair<string,bool>("First",true) },
        //        {"2",new KeyValuePair<string,bool>("Second",false) },
        //        {"3",new KeyValuePair<string,bool>("Third",true) },
        //        {"4",new KeyValuePair<string,bool>("Fourth",false) },
        //        {"5",new KeyValuePair<string,bool>("Fifth",true) },
        //    },
        //    Labels = new List<string>()
        //    {
        //        "First",
        //        "Second",
        //        "Third",
        //        "Fourth",
        //        "Fifth"
        //    }
        //};
        //levels[3] = new RepeatLevelClass()
        //{
        //    Level = 3,
        //    Answers = new Dictionary<string, KeyValuePair<string, bool>>()
        //    {
        //        {"1", new KeyValuePair<string,bool>("First",false) },
        //        {"2",new KeyValuePair<string,bool>("Second",false) },
        //        {"3",new KeyValuePair<string,bool>("Third",true) },
        //        {"4",new KeyValuePair<string,bool>("Fourth",false) },
        //        {"5",new KeyValuePair<string,bool>("Fifth",true) },
        //    },
        //    Labels = new List<string>()
        //    {
        //        "First",
        //        "Second",
        //        "Third",
        //        "Fourth",
        //        "Fifth"
        //    }
        //};
        //levels[4] = new RepeatLevelClass()
        //{
        //    Level = 4,
        //    Answers = new Dictionary<string, KeyValuePair<string, bool>>()
        //    {
        //        {"1", new KeyValuePair<string,bool>("First",true) },
        //        {"2",new KeyValuePair<string,bool>("Second",true) },
        //        {"3",new KeyValuePair<string,bool>("Third",true) },
        //        {"4",new KeyValuePair<string,bool>("Fourth",false) },
        //        {"5",new KeyValuePair<string,bool>("Fifth",true) },
        //    },
        //    Labels = new List<string>()
        //    {
        //        "First",
        //        "Second",
        //        "Third",
        //        "Fourth",
        //        "Fifth"
        //    }
        //};
    }

    public void Update()
    {
        if (GameManegmentHelper.CurrentLevelOverGameOne)
        {
            currentTime += Time.deltaTime;
            if (currentTime > interval && dontRepeat == false)
            {
                currentTime = 0f;
                
                //Debug.Log("YessSSSSSSSSSSSSS "+changeQuestions.name);
                GameManegmentHelper.isUI = true;
                GameManegmentHelper.isLevelEnd = true;
                dontRepeat = true;
                if (currentLevel+1==numberLevels)
                {
                    GameManegmentHelper.isUI = true; 
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
        GameManegmentHelper.GameOneAnswers.Clear();
        foreach (var item in levels[currentLevel].Answers)
        {
            GameManegmentHelper.GameOneAnswers.Add(item.Key, item.Value.Value);
            //Debug.Log(item.Key + "  " + item.Value.Value);
        }
        UpdateLabelsAsync(levels[currentLevel]);

        SetPresentStartSpritesAsync(levels[currentLevel]);

        GetAndInitCorrectAnswers();
        ScaleAnswersSync();
        GameManegmentHelper.GameOneLevel++;
        waitting.SetActive(false);
        NextLevelScreen.SetActive(false);
        GameManegmentHelper.isUI = false;
        dontRepeat = false;
        GameManegmentHelper.isLevelEnd = false;
        //StartCoroutine(ScaleAnswers());

    }
    public void ScaleAnswersSync()
    {
        var answers = GameObject.FindGameObjectsWithTag("answer");
        //Debug.Log("1 Scaled");
        foreach (var item in answers)
        {
            //Debug.Log("2 Scaled");
            Debug.Log(item.name);
            var comp = item.GetComponent<PresentController>();
            Debug.Log(comp.isScaled);
            bool isScaled = comp.isScaled;
            if (isScaled == true)
            {
                Debug.Log("1 Scaled");
                var scale = item.transform.localScale;
                scale.x = 2.1f;
                scale.y = 2.1f;
                item.transform.localScale = scale;
                comp.isScaled = false;
                //break;
            }
        }
    }
    private IEnumerator ScaleAnswers()
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            var answers = GameObject.FindGameObjectsWithTag("answer");
            //Debug.Log("1 Scaled");
            foreach (var item in answers)
            {
                //Debug.Log("2 Scaled");
                Debug.Log(item.name);
                var comp = item.GetComponent<PresentController>();
                Debug.Log(comp.isScaled);
                bool isScaled = comp.isScaled;
                if (isScaled == true)
                {
                    Debug.Log("1 Scaled");
                    var scale = item.transform.localScale;
                    scale.x = 2.1f;
                    scale.y = 2.1f;
                    item.transform.localScale = scale;
                    //break;
                }
            }
            workDone = true;
        }
    }
    private void SetPresentStartSprites(RepeatLevelClass level)
    {
        foreach (var item in level.Answers)
        {
            var go = GameObject.Find(item.Key);
            SpriteRenderer spriteRndr = go.GetComponent<SpriteRenderer>();
            spriteRndr.sprite = this.present;
        }
    }
    private void SetPresentStartSpritesAsync(RepeatLevelClass level)
    {
        foreach (var item in level.Answers)
        {
            StartCoroutine(YieldingUpdateSprites(item));

        }
    }

    IEnumerator YieldingUpdateSprites(KeyValuePair<string, KeyValuePair<string, bool>> item)
    {

        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
            var go = GameObject.Find(item.Key);
            SpriteRenderer spriteRndr = go.GetComponent<SpriteRenderer>();
            spriteRndr.sprite = this.present;
            workDone = true;
            // Do Work...
        }
    }

    private void GetAndInitCorrectAnswers()
    {
        GameManegmentHelper.gameOneAllAnswers = 0;
        foreach (var item in GameManegmentHelper.GameOneAnswers)
        {
            if (item.Value)
            {
                GameManegmentHelper.gameOneAllAnswers++;
            }
        }
        //Debug.Log(GameManegmentHelper.gameOneAllAnswers);
        GameManegmentHelper.gameOneGivenAnswers = 0;
        GameManegmentHelper.CurrentLevelOverGameOne = false;
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

    

    IEnumerator YieldingUpdateLableText(KeyValuePair<string, KeyValuePair<string, bool>> item)
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