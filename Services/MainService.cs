using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using MattTools.Data;
using Newtonsoft.Json;

namespace MattTools.Services;

public class MainService
{

    public event Action OnShowLoading;
    public event Action OnHideLoading;
    public string LoadingText;

    public List<Dialog> DialogList = new List<Dialog>();
    public event Action OpenDialog;
    public event Action CloseDialog;

    private bool StartupCheck;

    private ToolsData LoadData()
    {
        try
        {
            string path = Path.Combine(FileSystem.CacheDirectory, "ToolsData");
            string json = File.ReadAllText(path);
            ToolsData toolsData = JsonConvert.DeserializeObject<ToolsData>(json);

            return toolsData;

        }
        catch
        {
            return null;
        }

    }

    private void CreateDefaultData()
    {
        ToolsData data = new ToolsData();
        data.lockDate = DateTime.Now.AddDays(7);

        string json = JsonConvert.SerializeObject(data);
        string path = Path.Combine(FileSystem.CacheDirectory, "ToolsData");
        File.WriteAllText(path, json);
    }

    public async void CheckVersion()
    {
        if (StartupCheck)
            return;

        ShowLoading("Please Wait");

        await Task.Delay(1000);

        //Create Data if First Time
        if (LoadData() == null)
        {
            CreateDefaultData();
        }

        //Load Data
        try
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage respone = await client.GetAsync("https://raw.githubusercontent.com/matthewcc123/JsonTest/main/ToolsStatus.json");

            if (respone.IsSuccessStatusCode)
            {
                string result = respone.Content.ReadAsStringAsync().Result;
                ToolsStatus toolsStatus = JsonConvert.DeserializeObject<ToolsStatus>(result);

                if (toolsStatus.status != "active")
                {
                    ToolsData data = new ToolsData();
                    data.lockDate = DateTime.Now;
                    string json = JsonConvert.SerializeObject(data);
                    string path = Path.Combine(FileSystem.CacheDirectory, "ToolsData");
                    File.WriteAllText(path, json);
                }
                else
                {
                    //Update Expired Date
                    CreateDefaultData();
                }
            }

            client.Dispose();

        }
        catch
        {
            //nothing
        }
        finally
        {

            StartupCheck = true;
            HideLoading();

            await Task.Delay(200);

            //Force Close if LockTime
            ToolsData data = LoadData();

            if (DateTime.Now >= data.lockDate)
            {
                ShowLoading("Good Bye");

                await Task.Delay(3000);

                Application.Current.Quit();
            }

            
        }


    }

    public void CreateDialog(string title, string text)
    {
        //Add Dialog
        DialogList.Add(new Dialog { title = title, text = text });

        //Open Dialog
        OpenDialog?.Invoke();
    }

    public void DestroyDialog()
    {
        //Remove Dialog
        if (DialogList.Count > 0)
            DialogList.Remove(DialogList.First());

        //Close Dialog
        CloseDialog?.Invoke();
    }

    public void ShowLoading(string text)
    {
        LoadingText = text;
        OnShowLoading?.Invoke();
    }

    public void HideLoading()
    {
        OnHideLoading?.Invoke();
    }

}
