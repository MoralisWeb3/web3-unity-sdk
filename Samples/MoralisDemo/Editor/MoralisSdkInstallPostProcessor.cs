
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
 
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MoralisWeb3ApiSdk.Editor
{
    /// <summary>
    /// Pre-process to run as part of the Moralis Unity Package Manager 
    /// compatible package. 
    /// </summary>
    public class MoralisSdkInstallPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			var inPackages = importedAssets.Any(path => path.StartsWith("Packages/")) ||
				deletedAssets.Any(path => path.StartsWith("Packages/")) ||
				movedAssets.Any(path => path.StartsWith("Packages/")) ||
				movedFromAssetPaths.Any(path => path.StartsWith("Packages/"));
	 
			if (inPackages)
			{
				InitializeOnLoad();
			}
		}
	   
		[InitializeOnLoadMethod]
		private static void InitializeOnLoad()
		{
            Debug.Log("Starting to Moralis SDK pre-process ...");

            Debug.Log("Calling CopyWebGLTemplate ...");
            CopyWebGLTemplate();

            Debug.Log("Calling UpdateCscFile ...");
            UpdateCscFile();

            Debug.Log("Done pre-processing webl template...");
        }

        /// <summary>
        /// Copies Moralis WebGLTemplate to required Assets/WebGLTemplate folder.
        /// </summary>
        private static void CopyWebGLTemplate()
        {
            var destinationFolder = Path.GetFullPath("Assets/WebGLTemplates/MoralisWebGL");

            var sourceFolder = Path.GetFullPath("Packages/io.moralis.metaversesdk/Resources/WebGLTemplates/MoralisWebGL");

            Debug.Log($"Copying template from {sourceFolder}...");

            FileUtil.ReplaceDirectory(sourceFolder, destinationFolder);

            AssetDatabase.Refresh();

            Debug.Log($"Setting webgl template, old was = {PlayerSettings.WebGL.template}");

            PlayerSettings.WebGL.template = "PROJECT:MyTemplateName";

            Debug.Log($"Set webgl template to {PlayerSettings.WebGL.template}");
        }

        /// <summary>
        /// Creates / Updates csc.rsp file for Moralis Updates in Asset Root .
        /// NOTE: Does not remove entries and changing an existing entry will 
        /// create a duplicate entry. If either becomes a necessity, add a process
        /// that removes entries before entires are appended.
        /// </summary>
        private static void UpdateCscFile()
        {
            // Expected Moralis entires
            string[] moralisEntries = { "-define:MORALIS_UNITY" };

            Debug.Log("Processing csc.rsp file for Moralis entries.");

            try
            {
                var destinationFile = Path.GetFullPath("Assets/csc.rsp");

                // If file exists, determin if it contains all moralis entries.
                if (File.Exists(destinationFile))
                {
                    Debug.Log("csc.rsp exists, updating with Moralis entries ...");

                    List<string> entries = new List<string>(moralisEntries);

                    // First look at the file for current moralis entries
                    using (StreamReader sr = new StreamReader(destinationFile))
                    {
                        while (!sr.EndOfStream)
                        {
                            string e = sr.ReadLine();

                            if (entries.Contains(e))
                            {
                                entries.Remove(e);
                            }
                        }
                    }

                    // If any moralis entries are not in the csc.rsp, append them.
                    if (entries.Count > 0)
                    {
                        using (StreamWriter sw = new StreamWriter(destinationFile, true))
                        {
                            foreach (string e in entries)
                            {
                                sw.WriteLine(e);
                            }

                            sw.Flush();
                        }
                    }
                }
                // file does not exist so create it and populate the file with the moralis entries.
                else
                {
                    Debug.Log("csc.rsp does not exist, creating ...");

                    using (StreamWriter sw = new StreamWriter(destinationFile))
                    {
                        foreach (string e in moralisEntries)
                        {
                            sw.WriteLine(e);
                        }

                        sw.Flush();
                    }
                }

                Debug.Log("csc.rsp files has been processed for Moralis entries.");

                AssetDatabase.Refresh();
            }
            catch (Exception exp)
            {
                Debug.LogError($"Error encountered processing csc.rsp file for Moralis entries: {exp.Message}");
            }
        }
    }
}
