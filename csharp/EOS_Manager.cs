using Godot;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices;
using Epic.OnlineServices.Logging;
using System.IO;

namespace EOS_GODOT;

public partial class EOS_Manager : Node
{
    public static bool IsServer =
#if SERVER
        true;
#else
        false;
#endif

    public static EOS_Manager Instance;

    public PlatformInterface Platform;
    public EOS.AuthManager Auth;
    public override void _Ready()
    {
        Instance = this;
        INI.Write("EOS_Platform","test","yeet");
        InitializeOptions initializeOptions = new()
        { 
            ProductName = "EOS_GODOT",
            ProductVersion = "0.0.1",
        };

        Result initResult = PlatformInterface.Initialize(ref initializeOptions);
        GD.Print(initResult);
        if (initResult == Result.Success)
        {
            LoggingInterface.SetLogLevel(LogCategory.AllCategories, LogLevel.VeryVerbose);
            LoggingInterface.SetCallback((ref LogMessage message) =>
            {
                GD.Print($"{message.Category} {message.Level.ToString()} {message.Message}");

            });
            Options options = new()
            {
                CacheDirectory = Path.Combine(Directory.GetCurrentDirectory(),"Cache"),
                IsServer = IsServer,
                DeploymentId = INI.Read("EOS_Platform", "DeploymentId"),
                EncryptionKey = INI.Read("EOS_Platform", "EncryptionKey"),
                ProductId = INI.Read("EOS_Platform", "ProductId"),
                SandboxId = INI.Read("EOS_Platform", "SandboxId"),
                ClientCredentials = new()
                {
                    ClientId = INI.Read("EOS_Platform", "ClientId"),
                    ClientSecret = INI.Read("EOS_Platform", "ClientSecret"),
                },
                Flags = PlatformFlags.None,
            };
            Platform = PlatformInterface.Create(ref options);
            GD.Print("Successfully made platform!");
            this.CallDeferred("CreateManagers");
        }
        else if (initResult == Result.InvalidParameters)
        {
            GD.Print("EOS couldnt be initiaized. (Wrong parameter)");
        }
    }

    public void CreateManagers()
    {
        if (Platform == null)
            return;
        Auth = new(Platform.GetAuthInterface());
    }

    public override void _Process(double delta)
    {
        if (Platform != null)
            Platform.Tick();
    }

    public override void _ExitTree()
    {
        if (Platform != null)
            Platform.Release();
        PlatformInterface.Shutdown();
        Instance = null;
    }
}
