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

    public async Task<RossumData.LoginRespone> Login(RossumData.LoginForm form)
    {
        string payload = JsonConvert.SerializeObject(new RossumData.LoginData { username = form.username, password = form.password } );
        HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

        //Check Login Using key or Not
        string url = "auth/login";

        if (form.key != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", form.key);
            url = "organizations/406";
        }
        else
        {
            client.DefaultRequestHeaders.Authorization = null;
        }

        //POST
        HttpResponseMessage response = await client.PostAsync(url, content);

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
        else
        {
            string result = response.Content.ReadAsStringAsync().Result;

            RossumData.LoginBadRequest loginBadRequest = JsonConvert.DeserializeObject<RossumData.LoginBadRequest>(result);

            Debug.WriteLine(loginBadRequest.non_field_errors[0]);

            RossumData.LoginRespone loginRespone = new RossumData.LoginRespone { error = loginBadRequest.non_field_errors[0] };

            return loginRespone;
        }

    }

    public async Task<RossumData.LogoutRespone> Logout(string key)
    {
        string url = "auth/logout";
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", key);

        //POST
        HttpResponseMessage response = await client.PostAsync(url, null);

        string result = response.Content.ReadAsStringAsync().Result;
        RossumData.LogoutRespone logoutRespone = JsonConvert.DeserializeObject<RossumData.LogoutRespone>(result);

        if (response.IsSuccessStatusCode)
            logoutRespone.loggedOut = true;

        return logoutRespone;
    }
}

