using System;
using System.Collections.Generic;
using Org.Json;

namespace IO.Karte.Android.Tracking
{
    public partial class Tracker
    {
        public static void Identify(IDictionary<string, object> values, Action<bool> action)
        {
            Identify(values, completion: new CompletionDelegateWrapper(action));
        }
        public static void Identify(JSONObject jsonObject, Action<bool> action)
        {
            Identify(jsonObject, completion: new CompletionDelegateWrapper(action));
        }
        public static void Track(string name, Action<bool> action)
        {
            Track(name, completion: new CompletionDelegateWrapper(action));
        }
        public static void Track(string name, IDictionary<string, object> values, Action<bool> action)
        {
            Track(name, values, completion: new CompletionDelegateWrapper(action));
        }
        public static void Track(string name, JSONObject jsonObject, Action<bool> action)
        {
            Track(name, jsonObject, completion: new CompletionDelegateWrapper(action));
        }
        public static void View(string viewName, Action<bool> action)
        {
            View(viewName, completion: new CompletionDelegateWrapper(action));
        }
        public static void View(string viewName, string title, IDictionary<string, object> values, Action<bool> action)
        {
            View(viewName, title, values, completion: new CompletionDelegateWrapper(action));
        }
        public static void View(string viewName, string title, JSONObject jsonObject, Action<bool> action)
        {
            View(viewName, title, jsonObject, completion: new CompletionDelegateWrapper(action));
        }
        public static void View(string viewName, IDictionary<string, object> values, Action<bool> action)
        {
            View(viewName, values, completion: new CompletionDelegateWrapper(action));
        }
        public static void View(string viewName, JSONObject jsonObject, Action<bool> action)
        {
            View(viewName, jsonObject, completion: new CompletionDelegateWrapper(action));
        }
    }

    internal class CompletionDelegateWrapper : Java.Lang.Object, ITrackCompletion
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
