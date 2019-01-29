using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour {

	public void Exit()
    {
        GameManegmentHelper.GameTwoAnswers = new Dictionary<string, bool>();
        GameManegmentHelper.GameOneAnswers = new Dictionary<string, bool>();
        GameManegmentHelper.isColliding = new HashSet<string>();
        GameManegmentHelper.isHandCursorCatchingObject = false;
        GameManegmentHelper.Colliding = new Dictionary<string, string>();
        GameManegmentHelper.GameTwoLevel = 0;
        GameManegmentHelper.GameOneLevel = 0;
        var go = GameObject.Find("PlayerGesture");
        if (go!=null)
        {
            var comp = go.GetComponent<MouseGameTwoController>();

            comp.DestroySource();
        }
        SceneManager.LoadScene("Topic1");
    }
}
