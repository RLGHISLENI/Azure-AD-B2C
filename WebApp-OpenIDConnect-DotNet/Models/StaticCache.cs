using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;

namespace WebApp_OpenIDConnect_DotNet.Models
{
    public class MSALStaticCache
    {
        private static Dictionary<string, byte[]> staticCache = new Dictionary<string, byte[]>();

        private static ReaderWriterLockSlim SessionLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private readonly string userId = string.Empty;
        private readonly string cacheId = string.Empty;
        private readonly HttpContext httpContext = null;
        private ITokenCache cache;

        public MSALStaticCache(string userId, HttpContext httpcontext)
        {
            this.userId = userId;
            cacheId = this.userId + "_TokenCache";
            httpContext = httpcontext;
        }

        public ITokenCache EnablePersistence(ITokenCache cache)
        {
            this.cache = cache;
            cache.SetBeforeAccess(BeforeAccessNotification);
            cache.SetAfterAccess(AfterAccessNotification);
            return cache;
        }

        public void Load(TokenCacheNotificationArgs args)
        {
            SessionLock.EnterReadLock();
            byte[] blob = staticCache.ContainsKey(cacheId) ? staticCache[cacheId] : null;
            if (blob != null)
            {
                args.TokenCache.DeserializeMsalV3(blob);
            }
            SessionLock.ExitReadLock();
        }

        public void Persist(TokenCacheNotificationArgs args)
        {
            SessionLock.EnterWriteLock();

            staticCache[cacheId] = args.TokenCache.SerializeMsalV3();
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