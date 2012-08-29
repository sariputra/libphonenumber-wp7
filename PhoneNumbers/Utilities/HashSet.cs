using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Utilities
{
    public class HashSet<T> : Dictionary<T,T>
    {
        public void Add(T key)
        {
            base.Add(key, key);
        }

        public bool Contains(T key)
        {
            return base.ContainsKey(key);
        }

        public void UnionWith(List<T> value)
        {
            foreach (var VARIABLE in value)
            {
                if(!base.ContainsKey(VARIABLE))
                    base.Add(VARIABLE,VARIABLE);
            }
        }
    }
}
