using System;
using MattTools.Data;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Net.Http;

namespace MattTools.Services;

public class RossumService : IRossumService
{
    private readonly HttpClient client;

    public RossumService(HttpClient client)
    {
        this.client = client;
    }

    private async Task<HttpResponseMessage> POST(string url, HttpContent content, string key)
    {

        //Set Header Key
        client.DefaultRequestHeaders.Authorization = null;

        if (key != null)
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", key);


        //POST
        HttpResponseMessage response = await client.PostAsync(url, content);

        //Return Response
        return response;
    }

    private async Task<HttpResponseMessage> GET(string url, string key)
    {

        //Set Header Key
        client.DefaultRequestHeaders.Authorization = null;

        if (key != null)
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", key);


        //POST
        HttpResponseMessage response = await client.GetAsync(url);

        //Return Response
        return response;
    }

    public async Task<RossumData.LoginRespone> Login(RossumData.LoginForm form)
    {
        string payload = JsonConvert.SerializeObject(new RossumData.LoginData { username = form.username, password = form.password } );
        HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

        //Check Login Using key or Not
        string url = form.key == null ? "auth/login" : "organizations/406";

        //POST
        HttpResponseMessage response = await POST(url, content, form.key);

        if (response.IsSuccessStatusCode)
        {
            //Login by Username and Password
            string result = response.Content.ReadAsStringAsync().Result;

            RossumData.LoginRespone loginRespone = JsonConvert.DeserializeObject<RossumData.LoginRespone>(result);

            return loginRespone;
        }
        else if (response.StatusCode == HttpStatusCode.Forbidden && form.key != null)
        {
            //Login by Key
            RossumData.LoginRespone loginRespone =  new RossumData.LoginRespone { key = form.key };

            return loginRespone;
        }
        else if (response.StatusCode == HttpStatusCode.Unauthorized && form.key != null)
        {
            RossumData.LoginRespone loginRespone = new RossumData.LoginRespone { error = "Token Expired" };

            return loginRespone;
        }
        else
        {
            string result = response.Content.ReadAsStringAsync().Result;

            RossumData.LoginBadRequest loginBadRequest = JsonConvert.DeserializeObject<RossumData.LoginBadRequest>(result);

            RossumData.LoginRespone loginRespone = new RossumData.LoginRespone { error = loginBadRequest.non_field_errors?[0] };

            return loginRespone;
        }

    }

    public async Task<RossumData.LogoutRespone> Logout(string key)
    {
        string url = "auth/logout";

        //POST
        HttpResponseMessage response = await POST(url, null, key);

        string result = response.Content.ReadAsStringAsync().Result;
        RossumData.LogoutRespone logoutRespone = JsonConvert.DeserializeObject<RossumData.LogoutRespone>(result);

        if (response.IsSuccessStatusCode)
            logoutRespone.loggedOut = true;

        return logoutRespone;
    }

    public async Task<RossumData.PagingObject<RossumData.WorkspaceResult>> GetWorkspaces(string key, string url)
    {
        RossumData.PagingObject<RossumData.WorkspaceResult> workspaces = null;


        HttpResponseMessage response = await GET(url == null ? "workspaces" : url.Replace(client.BaseAddress.ToString(), string.Empty), key);

        if (response.IsSuccessStatusCode)
        {
            string result = response.Content.ReadAsStringAsync().Result;

            workspaces = JsonConvert.DeserializeObject<RossumData.PagingObject<RossumData.WorkspaceResult>>(result);
        }

        return workspaces;
    }
    public async Task<RossumData.QueueResult> GetQueue(string key, string url)
    {
        RossumData.QueueResult queue = null;

        HttpResponseMessage response = await GET(url.Replace(client.BaseAddress.ToString(), string.Empty), key);

        if (response.IsSuccessStatusCode)
        {
            string result = response.Content.ReadAsStringAsync().Result;

            queue = JsonConvert.DeserializeObject<RossumData.QueueResult>(result);
        }

        return queue;
    }
}

