using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishButton : GenericButton {
    public override void InitComponents()
    {
        //throw new NotImplementedException();
    }

    public override void OnPressed()
    {
        //throw new NotImplementedException();
        GameManegmentHelper.Exit();
    }

    
}
