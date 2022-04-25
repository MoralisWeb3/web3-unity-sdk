using MoralisUnity.Web3Api.Models;
using Newtonsoft.Json.Utilities;
using UnityEngine;

public class AotTypeEnforcer : MonoBehaviour
{
    private void Awake()
    {
        AotHelper.EnsureType<CloudFunctionResult<NativeBalance>>();
        AotHelper.EnsureType<NativeBalance>();
    }
}