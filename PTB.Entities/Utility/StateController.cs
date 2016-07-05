using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace PTB.Entities.Utility
{
    public static class StateController
    {
        private const string UserSessionName = "CurrentLoggedInuser";

        public static void SetUserSession(object value)
        {
            SetSession(UserSessionName, value);
        }
        public static UserDetailModel GetUserSession()
        {
            var user = GetSession(UserSessionName);
            return user != null ? (UserDetailModel)user : null;
        }

        private static void SetSession(string key, object value)
        {
             HttpContext.Current.Session[key] = value;
        }

        private static object GetSession(string key)
        {
            if (!Equals(HttpContext.Current.Session[key], null))
            {
                return HttpContext.Current.Session[key];
            }
            return null;
        }


        //public static void SetUserFilter(string userName, object value)
        //{
        //    string keystring = "UserFilter_" + userName;
        //    SetCache(keystring, value, DateTime.Now.AddDays(1));
        //}
        //public static void SetUserAdvanceFilter(string userName, object value)
        //{
        //    string keystring = "UserAdvanceFilter_" + userName;
        //    SetCache(keystring, value, DateTime.Now.AddDays(1));
        //}
        //public static object GetUserFilter(string userName)
        //{
        //    string keystring = "UserFilter_" + userName;
        //    return GetCache(keystring);
        //}
        //public static object GetUserAdvanceFilter(string userName)
        //{
        //    string keystring = "UserAdvanceFilter_" + userName;
        //    return GetCache(keystring);
        //}
        //public static void RemoveUserFilter(string userName)
        //{
        //    string keystring = "UserFilter_" + userName;
        //    RemoveCache(keystring);
        //}

        private static void SetCache(string key, object value, DateTime expDate)
        {
            if (HttpRuntime.Cache[key] != null) HttpRuntime.Cache.Remove(key);
            if (value != null)
            {
                HttpRuntime.Cache.Add(key, value, null, expDate, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
        }

        private static void RemoveCache(string key)
        {
            if (HttpRuntime.Cache[key] != null) HttpRuntime.Cache.Remove(key);

            //  HttpRuntime.Cache.Add(key, value, null, expDate, Cache.NoSlidingExpiration, CacheItemPriority.High, null);

        }

        private static object GetCache(string key)
        {
            if (HttpRuntime.Cache[key] == null) return null;

            else return HttpRuntime.Cache[key];
        }
    }
}
