using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGameTwoAnswers : MonoBehaviour {

    public List<bool> initAnswers = new List<bool>();
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < initAnswers.Count; i++)
        {
            GameManegmentHelper.GameTwoAnswers.Add((i + 1).ToString(), initAnswers[i]);
            //Debug.Log(GameManegmentHelper.FirstGameAnswers[(i + 1).ToString()]);
            
        }
        //Debug.Log(GameManegmentHelper.gameOneAllAnswers);
    }
}
