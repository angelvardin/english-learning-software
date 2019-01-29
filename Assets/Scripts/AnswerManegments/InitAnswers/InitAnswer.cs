using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAnswer : MonoBehaviour {

    public int answer;
	// Use this for initialization
	public void Start ()
    {
        if (!ResultCollector.answers.ContainsKey(answer))
        {
            ResultCollector.answers.Add(answer, new KeyValuePair<string, string>(this.gameObject.name, string.Empty));
        }
        else if (ResultCollector.answers.ContainsKey(answer))
        {
            var key = ResultCollector.answers[answer].Key;
            var value = this.gameObject.name;
            ResultCollector.answers[answer] = new KeyValuePair<string, string>(key, value);
        }
       
		
    
	}
	
	
}
