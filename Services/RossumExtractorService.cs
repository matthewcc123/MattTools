using System;
using System.Diagnostics;
using MattTools.Data;
namespace MattTools.Services;

public class RossumExtractorService
{

    public RossumData.LoginCache loginCache;

    public RossumExtractorService()
    {
        loginCache = new RossumData.LoginCache();
    }

    public void SaveLoginCache(string username, string key)
    {
        loginCache.username = username;
        loginCache.key = key;
    }

}

