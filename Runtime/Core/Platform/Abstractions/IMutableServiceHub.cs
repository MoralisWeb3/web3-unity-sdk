using MoralisUnity.Platform.Objects;

namespace MoralisUnity.Platform.Abstractions
{
    public interface IMutableServiceHub<TUser> : IServiceHub<TUser> where TUser : MoralisUser
    {
        new IServerConnectionData ServerConnectionData { set; }
        new IMetadataService MetadataService { set; }
        new IJsonSerializer JsonSerializer { set; }
        //IServiceHubCloner Cloner { set; }

        new IWebClient WebClient { set; }
        new ICacheService CacheService { set; }
        //IParseObjectClassController ClassController { set; }

        //IParseDataDecoder Decoder { set; }

        //IParseInstallationController InstallationController { set; }
        new IMoralisCommandRunner CommandRunner { set; }

        //IParseCloudCodeController CloudCodeController { set; }
        //IParseConfigurationController ConfigurationController { set; }
        //IParseFileController FileController { set; }
        //IParseObjectController ObjectController { set; }
        //IParseQueryController QueryController { set; }
        //IParseSessionController SessionController { set; }
        new IUserService<TUser> UserService { set; }
        new ICurrentUserService<TUser> CurrentUserService { get; }

        //IParseAnalyticsController AnalyticsController { set; }

        //IParseInstallationCoder InstallationCoder { set; }

        //IParsePushChannelsController PushChannelsController { set; }
        //IParsePushController PushController { set; }
        //IParseCurrentInstallationController CurrentInstallationController { set; }
        //IParseInstallationDataFinalizer InstallationDataFinalizer { set; }
    }
}
