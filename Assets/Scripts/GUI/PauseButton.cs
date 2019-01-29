using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : GenericButton {

    public float waitInterval = 2f;
    private float currentWaitTime = 0f;
    private bool wait = false;
    private bool enPress = false;
    public GameObject UIMenu;

    public override void InitComponents()
    {
        UIMenu.SetActive(false);
    }

    public override void BeforeUpdate()
    {
        base.BeforeUpdate();
        EnablePressing();
    }

    private void EnablePressing()
    {
        if (enPress)
        {
            if (currentWaitTime<waitInterval)
            {
                currentWaitTime += Time.deltaTime;
            }
            else
            {
                enPress = false;
                DisablePress = false;
                currentWaitTime = 0.0f;
            }
        }
    }

    public void UnPauseGame()
    {
        GameManegmentHelper.isUI = false;
        enPress = true;
        UIMenu.SetActive(false);
    }

    public override void OnPressed()
    {

        UIMenu.SetActive(true);
        DisablePress = true;
        GameManegmentHelper.isUI = true;
    }

   
}
