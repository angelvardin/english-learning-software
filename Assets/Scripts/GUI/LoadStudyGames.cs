using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStudyGames : MonoBehaviour {

    public void BackToMainMenu()
    {
        GameManegmentHelper.PlayerDetected = false;
        GameManegmentHelper.isColliding = new HashSet<string>();
        GameManegmentHelper.Colliding = new Dictionary<string, string>();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        string game = String.Empty;
        game += "Game";
        game += GameManegmentHelper.Game.ToString();
        game += "Topic";
        game += GameManegmentHelper.GameTopic.ToString();
        game += "Test";
        GameManegmentHelper.PlayerDetected = false;
        Debug.Log("game is "+game);

        SceneManager.LoadScene(game);
    }

    public void LoadGameOneTopicOne()
    {
        GameManegmentHelper.PlayerDetected = false;

        SceneManager.LoadScene("CheeseGame");
    }
    public void LoadGameTwoTopicOne()
    {
        GameManegmentHelper.PlayerDetected = false;

        SceneManager.LoadScene("ChoiseGame");
    }
    public void LoadGameThreeTopicOne()
    {
        GameManegmentHelper.PlayerDetected = false;

        SceneManager.LoadScene("DragAndDrop");
    }

    public void LoadGameFourTopicOne()
    {
        GameManegmentHelper.PlayerDetected = false;

        SceneManager.LoadScene("Words");
    }
    public void LoadGameFiveTopicOne()
    {
        GameManegmentHelper.PlayerDetected = false;

        SceneManager.LoadScene("Main");
    }
}
