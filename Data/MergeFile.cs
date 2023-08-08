namespace MattTools.Data;

public enum MergeStatus
{
    NotMatch,
    Ready,
    Merged
}

public class MergeFile
{

    public string InvoiceNumber { get; set; } = String.Empty;
    public string TaxNumber { get; set; } = String.Empty;
    public string InvoiceFileName { get; set; } = "Not Found";
    public string TaxFileName { get; set; } = "Not Found";
    public string InvoicePath { get; set; } = String.Empty;
    public string TaxPath { get; set; } = String.Empty;
    public bool Match { get; set; }
    public MergeStatus mergeStatus { get; set; }

    public string Status { get
        {
            switch (mergeStatus)
            {
                default:
                    return "";
                case MergeStatus.NotMatch:
                    return "Match not Found";
                case MergeStatus.Ready:
                    return "Ready to Merge";
                case MergeStatus.Merged:
                    return "Merged";
            }
        } 
    }



}
