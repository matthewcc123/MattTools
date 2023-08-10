using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattTools.Services;

public class MainLayoutService
{

    public class Dialog
    {
        public string title;
        public string text;
    }

    public event Action OnShowLoading;
    public event Action OnHideLoading;
    public string LoadingText;

    public List<Dialog> DialogList = new List<Dialog>();
    public event Action OpenDialog;
    public event Action CloseDialog;

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
