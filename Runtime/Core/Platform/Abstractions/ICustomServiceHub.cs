using MoralisUnity.Platform.Objects;

namespace MoralisUnity.Platform.Abstractions
{
    public interface ICustomServiceHub<TUser> : IServiceHub<TUser> where TUser : MoralisUser
    {
        IServiceHub<TUser> Services { get; }
    }
}
