using System;
using MattTools.Data;
namespace MattTools.Services;

public interface IRossumService
{
	Task<RossumData.LoginRespone> Login(RossumData.LoginForm form);
    Task<RossumData.LogoutRespone> Logout(string key);
    Task<RossumData.PagingObject<RossumData.WorkspaceResult>> GetWorkspaces(string key, string url);
    Task<RossumData.QueueResult> GetQueue(string key, string url);
    Task<RossumData.PagingObject<RossumData.DocumentResult>> GetDocumentByFileName(string name, string key);
    Task<RossumData.AnnotationResult> GetAnnotation(string url, string key);
    Task<string> GetJson(RossumData.AnnotationData annotation, string key);
    Task<byte[]> GetPdf(RossumData.AnnotationData annotation, string key);
    string GetBaseAddress();

}

