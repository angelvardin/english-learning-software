using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManegmentScript: MonoBehaviour {

    public GameObject cursor;
    public GameObject UIMenu;
    public GameObject NextLevelMenu;

    // Use this for initialization
    void Start () {
        this.NextLevelMenu.SetActive(false);
	}

    public void PauseGame()
    {
        cursor.SetActive(false);
        UIMenu.SetActive(true);
    }
    public void UnPauseGame()
    {
        cursor.SetActive(true);
        UIMenu.SetActive(false);
    }

    // Update is called once per frame

}
