using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Coins
{
    protected Coins()
    {

    }
    public int CoinsCount = 0;

    #region SINGLETON PATTERN
    public static Coins _instance;
    private static object _lock = new object();

    public static Coins Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Coins();

                    if (_instance == null)
                    {
                        _instance = new Coins();
                    }
                }
            }
           

            return _instance;
        }
    }
    #endregion


}

