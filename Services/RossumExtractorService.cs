using System;
using System.Diagnostics;
using MattTools.Data;
using Newtonsoft.Json;

namespace MattTools.Services;

public class RossumExtractorService
{

    public RossumData.LoginCache LoginCache;
    public List<RossumData.WorkspaceData> WorkspaceDataList;
    public int WorkspaceValue;
    public List<RossumData.QueueData> QueueDataList;
    public int QueueValue;
    public string Key { get { return LoginCache.key; } }
    public List<string> WorkspaceList
    {
        get
        {
            List<string> list = new List<string>();

            for (int i = 0; i < WorkspaceDataList.Count; i++)
            {
                list.Add(WorkspaceDataList[i].name);
            }

            return list;
        }
    }
    public List<string> QueueList
    {
        get
        {
            List<string> list = new List<string>();

            for (int i = 0; i < QueueDataList.Count; i++)
            {
                list.Add(QueueDataList[i].name);
            }

            return list;
        }
    }

    public RossumExtractorService()
    {
        LoginCache = new RossumData.LoginCache();
        WorkspaceDataList = new List<RossumData.WorkspaceData>();
        QueueDataList = new List<RossumData.QueueData>();
    }

    public void SaveLoginCache(string username, string key)
    {
        LoginCache.username = username?.ToLower();
        LoginCache.key = key;

        string path = Path.Combine(FileSystem.CacheDirectory, "LoginData.key");
        string json = JsonConvert.SerializeObject(LoginCache);

        File.WriteAllText(path, json);
    }

    public bool LoadLoginCache()
    {
        string path = Path.Combine(FileSystem.CacheDirectory, "LoginData.key");
        string json;

        try
        {
            json = File.ReadAllText(path);
            RossumData.LoginCache LoadedLoginCache = JsonConvert.DeserializeObject<RossumData.LoginCache>(json);
            
            if (LoadedLoginCache.username != null)
            {
                LoginCache.username = LoadedLoginCache.username;
                LoginCache.key = LoadedLoginCache.key;
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public void ClearLoginCache()
    {
        LoginCache.key = null;
        LoginCache.username = null;
    }

}

