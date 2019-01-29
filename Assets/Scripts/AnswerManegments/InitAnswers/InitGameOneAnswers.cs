using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGameOneAnswers : MonoBehaviour {

    public List<bool> initAnswers = new List<bool>();
	// Use this for initialization
	void Start () {
        GameManegmentHelper.gameOneAllAnswers = 0;
        for (int i = 0; i < initAnswers.Count; i++)
        {
            GameManegmentHelper.GameOneAnswers.Add((i+1).ToString(), initAnswers[i]);
            //Debug.Log(GameManegmentHelper.FirstGameAnswers[(i + 1).ToString()]);
            if (initAnswers[i])
            {
                GameManegmentHelper.gameOneAllAnswers++;
            }
        }
        //Debug.Log(GameManegmentHelper.gameOneAllAnswers);
	}
	
	
}
