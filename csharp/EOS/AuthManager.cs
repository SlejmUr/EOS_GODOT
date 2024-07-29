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
    public EpicAccountId LocalAccountId { get; internal set; } = null;
    public EpicAccountId AccountId { get; internal set; } = null;
    public bool IsLoggedIn { get; internal set; }

    /// <summary>
    /// Use it when login into the DEV account.
    /// </summary>
    /// <param name="token">Dev credentials</param>
    /// <param name="action">a callback that is fired when the operation completes</param>
    /// <param name="id">Dev host</param>
    /// <param name="clientData">arbitrary data that is passed back to you in the action</param>
    public void LoginDEV(string token, Action<LoginCallbackInfo> action, string id = "locahost:6666", object clientData = null)
    {
        if (IsLoggedIn)
            return;
        Login(new Credentials()
        {
            Id = id,
            Token = token,
            Type = LoginCredentialType.Developer
        },
        action, clientData);
    }

    /// <summary>
    /// Use this when starting from EGS Launcher (get parameters and get the excahnge code)
    /// </summary>
    /// <param name="code">The exchange code</param>
    /// <param name="action">a callback that is fired when the operation completes</param>
    /// <param name="clientData">arbitrary data that is passed back to you in the action</param>
    public void LoginCode(string code, Action<LoginCallbackInfo> action, object clientData = null)
    {
        if (IsLoggedIn)
            return;
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
                LocalAccountId = data.LocalUserId;
                AccountId = data.SelectedAccountId;
                IsLoggedIn = true;
            }
            action(data);
        });
    }


    public void LogOut(Action<LogoutCallbackInfo> action, object clientData = null)
    {
        if (!IsLoggedIn)
            return;
        LogoutOptions logoutOptions = new()
        { 
            LocalUserId = LocalAccountId,
        };
        Auth.Logout(ref logoutOptions, clientData, (ref LogoutCallbackInfo data) =>
        {
            if (data.ResultCode == Result.Success)
            {
                if (LocalAccountId == data.LocalUserId)
                {
                    LocalAccountId = null;
                }
                AccountId = null;
                IsLoggedIn = false;
            }
            action(data);
        });
    }

}
