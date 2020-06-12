using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class SessionHelper
    {                
        public static string ScriptLoader = "ScriptLoader";
        public static string Response = "Response";
        public static string PageHistory = "PageHistory";
        public static string LoggedIn = "##_LoggedIn_##";
        public static string ViewStateElement_CurrentUser = "##_CurrentUser_##";
        public static string LoginAuth = "##_LogInAuth_##";

        // Razsvetljava
        public static string CurrentLuciSettingsShown = "CurrentLuciSettingsShown";

        public static string MenuShown = "MenuShown";
        public static string SpremljajChecked = "SpremljajChecked";
        public static string WarningShowVisible = "WarningShowVisible";

        
        static object GetObject(string id)
        {
            var session = Navigator.GetSession();

            if (session != null)
            {
                return session[id];
            }
            return null;
        }

        static bool? GetBool(string id)
        {
            var variable = GetObject(id);
            return (bool?)variable;
        }

        static int? GetInt(string id)
        {
            var variable = GetObject(id);
            return (int?)variable;
        }

        static string GetString(string id)
        {
            var variable = GetObject(id);
            return (string)variable;
        }

        // SET
                
        static void SetBool(string id, bool value)
        {
            Navigator.GetSession()[id] = value;
        }

        static void SetInt(string id, int value)
        {
            Navigator.GetSession()[id] = value;
        }

        static void SetString(string id, string value)
        {
            Navigator.GetSession()[id] = value;
        }

        public static string GetCurrentUser()
        {
            var a = GetString(ViewStateElement_CurrentUser);
            if (a == null)
            {
                return "";
            }
            return a;
        }

        public static void SetCurrentUser(string CurrentUser)
        {
            SetString(ViewStateElement_CurrentUser, CurrentUser);
        }

    }
}