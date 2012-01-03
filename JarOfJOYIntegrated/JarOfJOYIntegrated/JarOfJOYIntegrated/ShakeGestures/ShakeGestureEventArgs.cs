using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ShakeGestures
{
    public class ShakeGestureEventArgs : EventArgs
    {
        private ShakeType _shakeType;

        public ShakeGestureEventArgs(ShakeType shakeType)
        {
            _shakeType = shakeType;
        }

        public ShakeType ShakeType
        {
            get
            {
                return _shakeType;
            }
        }
    }
}
