using Epic.OnlineServices;
using Epic.OnlineServices.Auth;
using System;

namespace EOS_GODOT.EOS;

public class AuthManager
{
    public AuthManager(AuthInterface authInterface) 
    {
        Auth = authInterface;
    }

    AuthInterface Auth;
    public EpicAccountId LocaLAccountId = null;
    public EpicAccountId AccountId = null;
    public bool IsLoggedIn { get; internal set; }
    public void LoginDEV(string token, Action<LoginCallbackInfo> action, int port = 6666, object clientData = null)
    {
        Login(new Credentials()
        {
            Id = $"localhost:{port}",
            Token = token,
            Type = LoginCredentialType.Developer
        },
        action, clientData);
    }

    /// <summary>
    /// Use this when starting 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="action"></param>
    /// <param name="clientData"></param>
    public void LoginCode(string code, Action<LoginCallbackInfo> action, object clientData = null)
    {
        Login(new Credentials()
        {
            Token = code,
            Type = LoginCredentialType.ExchangeCode
        },
        action, clientData);
    }


    protected void Login(Credentials credentials, Action<LoginCallbackInfo> action, object clientData = null)
    {
        LoginOptions loginOptions = new LoginOptions()
        {
            ScopeFlags = AuthScopeFlags.BasicProfile | AuthScopeFlags.FriendsList | AuthScopeFlags.Presence,
            Credentials = credentials
        };
        Auth.Login(ref loginOptions, clientData, (ref LoginCallbackInfo data) =>
        {
            if (data.ResultCode == Result.Success)
            {
                LocaLAccountId = data.LocalUserId;
                AccountId = data.SelectedAccountId;
                IsLoggedIn = true;
            }
            action(data);
        });
    }

}
