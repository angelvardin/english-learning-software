using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHowToPlayScenes : MonoBehaviour
{
    public void Start()
    {
        GameManegmentHelper.isUI = true;
    }
    public void LoadTutorialGameOneTopicOne()
    {
        GameManegmentHelper.GameTopic = GameManegmentHelper.Topic.One;
        GameManegmentHelper.Game = GameManegmentHelper.GameType.One;
        GameManegmentHelper.PlayerDetected = false;

        SceneManager.LoadScene("GameOne");
    }

    public void LoadTutorialGameTwoTopicOne()
    {
        GameManegmentHelper.GameTopic = GameManegmentHelper.Topic.One;
        GameManegmentHelper.Game = GameManegmentHelper.GameType.Two;
        GameManegmentHelper.PlayerDetected = false;

        SceneManager.LoadScene("GameTwo");
    }
    public void LoadTutorialGameThreeTopicOne()
    {
        GameManegmentHelper.GameTopic = GameManegmentHelper.Topic.One;
        GameManegmentHelper.Game = GameManegmentHelper.GameType.Three;
        GameManegmentHelper.PlayerDetected = false;

        SceneManager.LoadScene("GameThree");
    }
    public void LoadTutorialGameFourTopicOne()
    {
        GameManegmentHelper.GameTopic = GameManegmentHelper.Topic.One;
        GameManegmentHelper.Game = GameManegmentHelper.GameType.Four;
        GameManegmentHelper.PlayerDetected = false;

        SceneManager.LoadScene("GameFour");
    }
    public void LoadTutorialGameFiveTopicOne()
    {
        GameManegmentHelper.GameTopic = GameManegmentHelper.Topic.One;
        GameManegmentHelper.Game = GameManegmentHelper.GameType.Five;
        GameManegmentHelper.PlayerDetected = false;

        SceneManager.LoadScene("GameFive");
    }


}
