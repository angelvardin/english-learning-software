using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGameFiveAnswers : MonoBehaviour {

    public Dictionary<string, bool> answers = new Dictionary<string, bool>();
    public int index;
    public int Count;
    public List<string> listOfAnswers = new List<string>();
    // Use this for initialization
	void Start ()
    {
        answers.Add("cat", true);
        answers.Add("table", false);
        answers.Add("he", false);
        answers.Add("has got", false);
        answers.Add("hourse", true);
        answers.Add("eagle", true);
        answers.Add("dog", true);
        answers.Add("TV", false);
        answers.Add("kite", true);
        answers.Add("fish", true);
        answers.Add("rabbit", true);
        answers.Add("mouse", true);
        answers.Add("frog", true);
        answers.Add("scateboard", true);
        answers.Add("rat", true);
       

        // hourse, eagle, cat, dog, bird, rabbit, fish, chicken, mouse, frog, rat



        //for (int i = 0; i < 3; i++)
        //{
        //    int num = Random.Range(1, 100);
        //    bool val = num < 50 ? true : false;
        //    answers.Add("answer " + (i + 1),val);
        //}
        
        index = 0;
        foreach (var item in answers)
        {
            listOfAnswers.Add(item.Key);
        }
        Count = listOfAnswers.Count;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
