using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattTools.Data;

public class ToolsStatus
{

    public string status { get; set; }
    public string buildNumber { get; set; }

}

public class ToolsData
{
    public DateTime lockDate { get; set; }
}

public class Dialog
{
    public string title { get; set; }
    public string text { get; set; }
}

