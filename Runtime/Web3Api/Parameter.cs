#nullable enable
using System;
using UnityEngine.Networking;

namespace MoralisUnity.Web3Api.Core
{
    public class Parameter : IEquatable<Parameter>
    {
        public Parameter(string name, object? value, ParameterType type)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
        }

        public Parameter(string name, object value, string contentType, ParameterType type)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
            this.ContentType = contentType;
        }

        public string? Name { get; set; }
        public object? Value { get; set; }
        public ParameterType Type { get; set; }
        public DataFormat DataFormat { get; set; }
        public string? ContentType { get; set; }

        public bool Equals(Parameter? other)
        {
            var name = this.Name;
            var value = this.Value;
            return value != null && name != null && name.Equals(other?.Name) && value.Equals(other?.Value) && this.Type.Equals(other?.Type);
        }

        public override bool Equals(object? obj)
        {
            var name = this.Name;
            var value = this.Value;
            return value != null && name != null && obj is Parameter && name.Equals(((Parameter)obj)?.Name) && value.Equals(((Parameter)obj)?.Value) && this.Type.Equals(((Parameter)obj)?.Type);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"Type: {this.Type}, Name: {this.Name}, Value: {this.Value}, ContentType: {this.ContentType}";
        }
    }
}
