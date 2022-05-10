using System;
using System.Text;

namespace MoralisUnity.Examples.Sdk.Shared
{
    /// <summary>
    /// Helper methods for this class
    /// </summary>
    public static class StringBuilderExtensions
    {
        //  General Methods  --------------------------------------
        
        public static void AppendHeaderLine (this StringBuilder sb, string message)
        {
            // Add one line before it
            sb.AppendLine();
            //sb.AppendLine("<color=\"black\"><b>" + message + "</b></color>");
            sb.AppendLine("<b>" + message + "</b>");
        }
        
        public static void AppendErrorLine (this StringBuilder sb, string message)
        {
            sb.AppendLine("<color=\"red\"><b>" + message + "</b></color>");
        }
        

        
        public static void AppendLines (this StringBuilder sb, int linesToAppend)
        {
            for (int i = 0; i < linesToAppend; i++)
            {
                sb.AppendLine();
            }
        }
        
        public static void AppendBullet (this StringBuilder sb, string message)
        {
            sb.AppendBullet(message, 1);
        }
        
        public static void AppendBullet (this StringBuilder sb, string message, int indentLevels)
        {
            string indentString = "";
            for (int i = 0; i <= indentLevels; i++)
            {
                // Indent just TAB per level to allow for
                // 1. Save the precious horizontal room
                // 2. while still being OBVIOUSLY indented
                indentString += "\t";
            }
            
            sb.AppendLine($"{indentString}• {message}");
        }
        
        public static void AppendBulletLoopLimit (this StringBuilder sb, LoopLimit loopLimit)
        {
            sb.AppendBullet($"Results {loopLimit.CountCurrent} to {loopLimit.CountMax}...");
        }
        
        public static void AppendBulletError (this StringBuilder sb, string message)
        {
            sb.AppendBullet($"<color=\"red\"><b>{message}</b></color>");
        }
        
        
        public static void AppendBulletException (this StringBuilder sb, Exception exception)
        {
            sb.AppendBulletError(exception.Message);
        }
    }
}