using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTopics : MonoBehaviour {

	public void LoadTopicOne()
    {
        GameManegmentHelper.PlayerDetected = false;
        GameManegmentHelper.loadingNewLevel = true;
        SceneManager.LoadScene("Topic1");

    }
}
