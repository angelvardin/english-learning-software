using System;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using UnityEngine;
using Microsoft.Kinect.VisualGestureBuilder;
using UnityEngine.SceneManagement;

[Serializable]
public class GameManegmentHelper
{
    protected GameManegmentHelper()
    {
    }

    public static void SetObjectPosition(GameObject go,Vector3 position)
    {
        if (go!=null||position!=null)
        {
            go.transform.position = position;
        }
    }
    public static Dictionary<string, Vector3> gameTwoPresentsStartPosition = new Dictionary<string, Vector3>();
    public static void Exit(String mainScene = "Topic1")
    {
        GameManegmentHelper.GameTwoAnswers = new Dictionary<string, bool>();
        GameManegmentHelper.GameOneAnswers = new Dictionary<string, bool>();
        GameManegmentHelper.isColliding = new HashSet<string>();
        GameManegmentHelper.isHandCursorCatchingObject = false;
        GameManegmentHelper.Colliding = new Dictionary<string, string>();
        GameManegmentHelper.GameTwoLevel = 0;
        GameManegmentHelper.GameOneLevel = 0;
        GameManegmentHelper.gameTwoPresentsStartPosition = new Dictionary<string, Vector3>();
        var go = GameObject.Find("PlayerGesture");
        if (go != null)
        {
            var comp = go.GetComponent<MouseGameTwoController>();

            comp.DestroySource();
        }
        SceneManager.LoadScene(mainScene);
    }
    public static Stack order = new Stack();
    public static HashSet<string> isColliding = new HashSet<string>();
    public static Dictionary<string,string> Colliding = new Dictionary<string, string>();
    public static bool PlayerDetected = false;
    public static bool loadingNewLevel = false;

    public static KinectSensor sensor=null;
    public static BodyFrameReader _Reader=null;
    public static VisualGestureBuilderFrameSource _gestureSource = null;
    public static VisualGestureBuilderFrameReader _gestureReader = null;



    public static bool isUI = false;
    public static bool isLevelEnd = false;

    public static bool isHandOpen = true;
    public static bool isHandClosed = true;
    public static bool isPlayerDetect = false;
    public static bool isHandCursorCatchingObject = false;
    public static bool isPressing = false;

    public static Dictionary<string, bool> GameOneAnswers = new Dictionary<string, bool>();
    public static Dictionary<string, bool> GameTwoAnswers = new Dictionary<string, bool>();

    public static bool isGameFiveOver = false;
    public static int gameOneAllAnswers = 0;
    public static int gameOneGivenAnswers = 0;

    public static bool CurrentLevelOverGameOne = false;
    public static bool CurrentLevelOverGameTwo = false;
    public static bool GameThreeFinished = false;
    public static bool GameFourFinished = false;

    public static int CurrentGame=0;

    public static bool nextLevel = false;

    public static int GameOneLevel = 0;
    public static int GameTwoLevel = 0;

    public static List<string> SentencesGameThree = new List<string>();

    public static HandState handState = HandState.Unknown;

    public static Topic GameTopic;
    public static GameType Game;


    public enum GameState {UI,Level,Play };
    public enum Topic { One, Two, Three, Four, Five };
    public enum GameType { One, Two, Three, Four, Five };




}
