using System.Collections.Generic;
using WinMediaBox.Classes.Tools;

namespace WinMediaBox.Classes
{
    public static class SessionExiting
    {
        private static OnSessionEndidngActions _actions = new OnSessionEndidngActions();
        public static bool isAnyMediaActionActive = false;
        public static List<HotKey> hotKeys = new List<HotKey>();
        public static bool manualExit = false;
        public static bool isExiting = false;

        public static void SetEndCurrentMediaAction(OnSessionEndidngActions.EndCurrentMediaAction method)
        {
            _actions.SetEndCurrentMediaAction(method);
        }

        public static void EndAll()
        {
            isExiting = true;
            if (isAnyMediaActionActive)
            {
                _actions.endCurrentMediaAction?.Invoke();
                return;
            }
            isExiting = false;
        }

    }

    public class OnSessionEndidngActions
    {
        public delegate void EndCurrentMediaAction();
        public EndCurrentMediaAction endCurrentMediaAction;

        public void SetEndCurrentMediaAction(EndCurrentMediaAction method)
        {
            endCurrentMediaAction = new EndCurrentMediaAction(method);
        }
    }
}
