using MoralisUnity.Platform.Services.Infrastructure;
using MoralisUnity.Platform.Abstractions;
using MoralisUnity.Platform.Objects;

namespace MoralisUnity.Platform.Services
{
    /// <summary>
    /// An <see cref="IServiceHubMutator"/> implementation which changes the <see cref="IServiceHub{TUser}.CacheController"/>'s <see cref="IDiskFileCacheService.AbsoluteCacheFilePath"/> if available.
    /// </summary>
    public class AbsoluteCacheLocationMutator : IServiceHubMutator
    {
        /// <summary>
        /// A custom absolute cache file path to be set on the active <see cref="IServiceHub{TUser}.CacheController"/> if it implements <see cref="IDiskFileCacheService"/>.
        /// </summary>
        public string CustomAbsoluteCacheFilePath { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Valid => CustomAbsoluteCacheFilePath is { };

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="target"><inheritdoc/></param>
        /// <param name="composedHub"><inheritdoc/></param>
        public void Mutate<TUser>(ref IMutableServiceHub<TUser> target, in IServiceHub<TUser> composedHub) where TUser : MoralisUser
        {
            if ((target as IServiceHub<TUser>).CacheService is IDiskFileCacheService { } diskFileCacheController)
            {
                diskFileCacheController.AbsoluteCacheFilePath = CustomAbsoluteCacheFilePath;
                diskFileCacheController.RefreshPaths();
            }
        }
    }

}
