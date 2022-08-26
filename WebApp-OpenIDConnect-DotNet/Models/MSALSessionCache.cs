using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;

namespace WebApp_OpenIDConnect_DotNet.Models
{
    public class MSALSessionCache
    {
        private static ReaderWriterLockSlim SessionLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        string UserId = string.Empty;
        string CacheId = string.Empty;
        private readonly HttpContext httpContext = null;
        private ITokenCache cache;

        public MSALSessionCache(string userId, HttpContext httpcontext)
        {
            UserId = userId;
            CacheId = UserId + "_TokenCache";
            httpContext = httpcontext;
        }

        public ITokenCache EnablePersistence(ITokenCache cache)
        {
            this.cache = cache;
            cache.SetBeforeAccess(BeforeAccessNotification);
            cache.SetAfterAccess(AfterAccessNotification);
            return cache;
        }

        public void SaveUserStateValue(string state)
        {
            SessionLock.EnterWriteLock();
            httpContext.Session.SetString(CacheId + "_state", state);
            SessionLock.ExitWriteLock();
        }
        public string ReadUserStateValue()
        {
            string state = string.Empty;
            SessionLock.EnterReadLock();
            state = (string)httpContext.Session.GetString(CacheId + "_state");
            SessionLock.ExitReadLock();
            return state;
        }
        public void Load(TokenCacheNotificationArgs args)
        {
            SessionLock.EnterReadLock();
            byte[] blob = httpContext.Session.Get(CacheId);
            if (blob != null)
            {
                args.TokenCache.DeserializeMsalV3(blob);
            }
            SessionLock.ExitReadLock();
        }

        public void Persist(TokenCacheNotificationArgs args)
        {
            SessionLock.EnterWriteLock();

            httpContext.Session.Set(CacheId, args.TokenCache.SerializeMsalV3());
            SessionLock.ExitWriteLock();
        }

        void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            Load(args);
        }

        void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            if (args.HasStateChanged)
            {
                Persist(args);
            }
        }
    }
}