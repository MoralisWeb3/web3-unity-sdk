#if !DOTNET35
using System;
using log4net;
using Nethereum.JsonRpc.Client.RpcMessages;

public static class ILogExtentions
{
    public static bool IsTraceEnabled(this ILog log)
    {
        return log.Logger.IsEnabledFor(log4net.Core.Level.Trace);
    }

    public static void Trace(this ILog log, string message, Exception exception)
    {
        log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
            log4net.Core.Level.Trace, message, exception);
    }

    public static void Trace(this ILog log, string message)
    {
        log.Trace(message, null);
    }
}

namespace Nethereum.JsonRpc.Client
{

    public class RpcLogger
    {
        public RpcLogger(ILog log)
        {
            Log = log;
        }
        public ILog Log { get; private set; }
        public string RequestJsonMessage { get; private set; }
        public RpcResponseMessage ResponseMessage { get; private set; }

        public void LogRequest(string requestJsonMessage)
        {
            RequestJsonMessage = requestJsonMessage;
            if (IsLogTraceEnabled())
            {
                Log.Trace(GetRPCRequestLogMessage() );
            }
        }

        private string GetRPCRequestLogMessage()
        {
            return $"RPC Request: {RequestJsonMessage}";
        }

        private string GetRPCResponseLogMessage()
        {
            return ResponseMessage != null ? $"RPC Response: {ResponseMessage.Result}" : String.Empty;
        }
        private bool IsLogErrorEnabled()
        {
            return Log != null && Log.IsErrorEnabled;
        }

        public void LogResponse(RpcResponseMessage responseMessage)
        {
            ResponseMessage = responseMessage;

            if (IsLogTraceEnabled())
            {
                Log.Trace(GetRPCResponseLogMessage());
            }

            if (HasError(responseMessage) && IsLogErrorEnabled())
            {
                if (!IsLogTraceEnabled())
                {
                    Log.Trace(GetRPCResponseLogMessage());
                }
                Log.Error($"RPC Response Error: {responseMessage.Error.Message}");
            }
        }

        public void LogException(Exception ex)
        {
            if (IsLogErrorEnabled())
            {
                Log.Error("RPC Exception, "  + GetRPCRequestLogMessage() + GetRPCResponseLogMessage(), ex);
            }
        }

        private bool HasError(RpcResponseMessage message)
        {
            return message.Error != null && message.HasError;
        }

        private bool IsLogTraceEnabled()
        {
            return Log != null && Log.IsTraceEnabled();
        }

    }

}
#endif