using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGames : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManegmentHelper.isUI = true;
	}

    public void LoadGameOne()
    {
        SceneManager.LoadScene("CheeseGame");
    }
}
