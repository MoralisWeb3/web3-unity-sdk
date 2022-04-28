using MoralisUnity.Platform.Abstractions;

namespace MoralisUnity.Platform.Operations
{
    class MoralisSetOperation : IMoralisFieldOperation
    {
        public object Value { get; private set; }

        public MoralisSetOperation(object value) => Value = value;

        public IMoralisFieldOperation MergeWithPrevious(IMoralisFieldOperation previous) => this;

        public object Apply(object oldValue, string key) => Value;


    }
}
