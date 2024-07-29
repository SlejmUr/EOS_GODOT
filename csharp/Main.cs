using EOS_GODOT;
using Godot;
using System;

public partial class Main : Control
{


	public void LoginDEV_Button_Pressed()
	{
        LineEdit edit = GetNode<LineEdit>("CodeInput");
		EOS_Manager.Instance.Auth.LoginDEV(edit.Text, (LoginCallbackInfo) => 
		{
            GD.Print("Logged in? " + EOS_Manager.Instance.Auth.IsLoggedIn);
            GD.Print(EOS_Manager.Instance.Auth.AccountId);
        });

	}

    public void LoginCode_Button_Pressed()
    {
        LineEdit edit = GetNode<LineEdit>("CodeInput");
        EOS_Manager.Instance.Auth.LoginCode(edit.Text, (LoginCallbackInfo) =>
        {
            GD.Print("Logged in? " + EOS_Manager.Instance.Auth.IsLoggedIn);
            GD.Print(EOS_Manager.Instance.Auth.AccountId);
        });

    }
}
