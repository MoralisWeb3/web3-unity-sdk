using System;
using MoralisUnity.Platform.Objects;

namespace MoralisUnity.Platform.Utilities
{
    public class MoralisFileExtensions
    {
        public static MoralisFile Create(string name, Uri uri, string mimeType = null) => new MoralisFile(name, uri, mimeType);
    }
}
