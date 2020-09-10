using System;
using System.Linq;
using System.Reflection;
using Android.Runtime;
using IO.Karte.Android.Tracking;
using Java.Interop;

namespace IO.Karte.Android.Variables
{
    public partial class Variables
    {
        public static void Fetch(Action<bool> action)
        {
            Fetch(completion: new CompletionDelegateWrapper(action));
        }
    }

    internal class CompletionDelegateWrapper : Java.Lang.Object, IFetchCompletion
    {
        Action<bool> action;
        public CompletionDelegateWrapper(Action<bool> action)
        {
            this.action = action;
        }
        public void OnComplete(bool isSuccess)
        {
            this.action(isSuccess);
        }
    }
}
