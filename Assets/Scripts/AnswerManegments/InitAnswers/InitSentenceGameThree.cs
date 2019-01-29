using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitSentenceGameThree : MonoBehaviour {

    // Use this for initialization
    public string Text;
    public int SentenceNumber;
    public int PartOfTheSentence;

    void Start () {
        StartCoroutine(Text);

    }

    IEnumerator YieldingUpdateLableText(string labletext)
    {
        bool workDone = false;

        while (!workDone)
        {
            // Let the engine run for a frame.
            yield return null;
           
            var sl = this.transform.Find("LableText");
            var text = sl.transform.Find("Text");
            var textComponent = text.gameObject.GetComponent<Text>();
            textComponent.text = labletext;
            workDone = true;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
