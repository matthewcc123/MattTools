using PdfSharpCore.Pdf.Content.Objects;
using PdfSharpCore.Pdf.Content;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf;
using System.Diagnostics;
using MattTools.Data;

namespace MattTools.Services;

internal class ULIMergerService
{

    public int MaxAllowedFiles = 100;
    public string MergePath;

    //List
    public List<string> Errors = new();
    public List<MergeFile> MergeFiles = new();
    public List<string> InvoicePaths = new();
    public List<string> TaxPaths = new();
    public List<string> SortedTaxPaths = new();

    public void ClearAll()
    {
        MergePath = null;
        Errors.Clear();
        MergeFiles.Clear();
        InvoicePaths.Clear();
        TaxPaths.Clear();
    }

    public async Task PickInvoices()
    {

        await Task.Run(ClearAll);

        try
        {
            FilePickerFileType filePickerFileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>{
                    { DevicePlatform.MacCatalyst, new [] { "pdf" } },
                    { DevicePlatform.macOS, new [] { "pdf" } },
                    { DevicePlatform.WinUI, new [] { ".pdf" } }
                    });
            PickOptions pickOptions = new PickOptions
            {
                FileTypes = filePickerFileType
            };


            var results = await MainThread.InvokeOnMainThreadAsync(() => FilePicker.PickMultipleAsync(pickOptions)).ConfigureAwait(false);

            if (results == null)
                return;

            if (results.Count() > MaxAllowedFiles)
            {
                Errors.Add($"Attempting to select {results.Count()}, only {MaxAllowedFiles} files are allowed.");
                return;
            }

            //Seperate Invoice File and Tax File
            foreach (var file in results)
            {
                if (MergePath == null)
                    MergePath = file.FullPath.Replace(file.FileName, String.Empty);

                SeperateFiles(file);
            }

            //Matching Invoice File and Tax File
            SortedTaxPaths = new List<string>(TaxPaths);
            foreach (var mergeFile in MergeFiles)
            {
                mergeFile.Match = FindTaxInvoice(mergeFile);
            }

        }
        catch (Exception ex)
        {
            Errors.Add(ex.Message);
        }


    }

    public void SeperateFiles(FileResult file)
    {
        Console.WriteLine(file.FileName);
        string[] nameSplit;
        MergeFile mergeFile = new();

        try
        {
            //Seperating Invoice and Tax
            nameSplit = file.FileName.Replace(".pdf", "").Split('-');
            mergeFile.InvoiceNumber = nameSplit[nameSplit.Length - 2];

            if (mergeFile.InvoiceNumber.Length != 10)
            {
                //Tax Invoice
                TaxPaths.Add(file.FullPath);
                return;
            }

            //ULI Invoice
            InvoicePaths.Add(file.FullPath);
            mergeFile.TaxNumber = nameSplit[nameSplit.Length - 1];
            mergeFile.InvoicePath = file.FullPath;
            mergeFile.InvoiceFileName = file.FileName;
            mergeFile.Match = true;
            MergeFiles.Add(mergeFile);

        }
        catch
        {
            Errors.Add($"{file.FileName} is invalid file");
        }
    }
    public bool FindTaxInvoice(MergeFile mergeFile)
    {

        for (int i = 0; i < SortedTaxPaths.Count; i++)
        {
            try
            {
                //Match File Name
                if (!SortedTaxPaths[i].Replace(MergePath, String.Empty).Contains(mergeFile.TaxNumber))
                {
                    mergeFile.mergeStatus = MergeStatus.NotMatch;
                    continue;
                }

                //Find TaxNumber inside Pdf
                PdfDocument taxPdf = PdfReader.Open(SortedTaxPaths[i], PdfDocumentOpenMode.Import);
                bool invoiceFound = false;
                bool taxFound = false;

                //Find by InvoiceNumber
                for (int p = 0; p < taxPdf.PageCount; p++)
                {
                    if (!invoiceFound)
                        invoiceFound = FindText(ContentReader.ReadContent(taxPdf.Pages[p]), mergeFile.InvoiceNumber);

                    if (!taxFound)
                        taxFound = FindText(ContentReader.ReadContent(taxPdf.Pages[p]), TaxNumberFormat(mergeFile.TaxNumber));
                }

                //If Match
                if (invoiceFound && taxFound)
                {
                    mergeFile.TaxFileName = SortedTaxPaths[i].Replace(MergePath, String.Empty);
                    mergeFile.TaxPath = SortedTaxPaths[i];
                    SortedTaxPaths.Remove(SortedTaxPaths[i]);
                    mergeFile.mergeStatus = MergeStatus.Ready;
                    return true;
                }
                else if (!taxFound)
                {
                    //Errors.Add($"{mergeFile.InvoiceNumber} not found it's tax invoice.");
                    mergeFile.mergeStatus = MergeStatus.NotMatch;
                }
            }
            catch (Exception ex)
            {
                Errors.Add($"{mergeFile.InvoiceNumber} Error: {ex}");
            }

        }

        return false;
    }

    public void MergeInvoices()
    {

        if (MergeFiles.Count == 0)
            return;

        foreach (var mergeFile in MergeFiles)
        {
            if (!mergeFile.Match)
                continue;

            try
            {
                //Get the Paths
                string[] files = { mergeFile.InvoicePath, mergeFile.TaxPath };

                //Output Doc
                PdfDocument outputPdf = new();

                //Merging
                foreach (var file in files)
                {
                    PdfDocument inputPdf = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                    for (int a = 0; a < inputPdf.PageCount; a++)
                    {
                        //Add Page to outputPDF
                        PdfPage page = inputPdf.Pages[a];
                        outputPdf.AddPage(page);
                    }
                }

                //Save Document
                if (!System.IO.Directory.Exists(MergePath + @"\Merged"))
                    System.IO.Directory.CreateDirectory(MergePath + @"\Merged");

                string fileName = MergePath + @"\Merged\" + mergeFile.InvoiceNumber + ".pdf";
                outputPdf.Save(fileName);

                mergeFile.mergeStatus = MergeStatus.Merged;

            }
            catch (Exception ex)
            {
                Errors.Add($"{mergeFile.InvoiceNumber} Error: {ex}");
            }

        }
    }

    public string TaxNumberFormat(string taxNumber)
    {
        return $"{taxNumber.Substring(0, 3)}.{taxNumber.Substring(3, 3)}-{taxNumber.Substring(6, 2)}.{taxNumber.Remove(0, 8)}";
    }

    private static bool FindText(CSequence contents, string searchText)
    {
        // Iterate thru each content items. Each item may or may not contain the entire
        // word if there are different stylings (ex: bold parts of the word) applied to a word.
        // So you may have to replace a character at a time.
        bool found = false;

        for (int i = 0; i < contents.Count; i++)
        {
            if (contents[i] is COperator)
            {
                var cOp = contents[i] as COperator;
                for (int j = 0; j < cOp.Operands.Count; j++)
                {
                    if (cOp.OpCode.Name == OpCodeName.Tj.ToString() ||
                        cOp.OpCode.Name == OpCodeName.TJ.ToString())
                    {
                        if (cOp.Operands[j] is CString)
                        {
                            var cString = cOp.Operands[j] as CString;
                            if (cString.Value.Contains(searchText))
                            {
                                found = true;
                            }

                        }
                    }
                }


            }
        }

        return found;


    }

}
