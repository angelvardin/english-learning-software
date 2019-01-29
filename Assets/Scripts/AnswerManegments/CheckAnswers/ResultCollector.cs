using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
static class ResultCollector
{
    public static Dictionary<string, string> UserResults = new Dictionary<string, string>();
    public static Dictionary<int, KeyValuePair<string, string>> answers = new Dictionary<int, KeyValuePair<string, string>>();
}

