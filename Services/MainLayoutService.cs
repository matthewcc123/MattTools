using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattTools.Services;

public class MainLayoutService
{

    public event Action OnShowLoading;
    public event Action OnHideLoading;
    public string LoadingText;

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
