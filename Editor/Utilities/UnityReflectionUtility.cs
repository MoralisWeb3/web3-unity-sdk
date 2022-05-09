using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using MoralisUnity.Sdk.Constants;
using MoralisUnity.Sdk.UI.ReadMe;
using UnityEngine;

namespace MoralisUnity.Sdk.Utilities
{
	/// <summary>
	/// Reflection is string-based and Unity-version-based and thus, notoriously fragile.
	/// This wraps the risk for maintainability
	/// </summary>
	public static class UnityReflectionUtility
	{
		/// <summary>
		/// Load a unity editor layout by path
		/// </summary>
		/// <param name="path"></param>
		public static void UnityEditor_WindowLayout_LoadWindowLayout(string path)
		{
			var assembly = typeof(EditorApplication).Assembly;
			var windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
			var methods = windowLayoutType.GetMethods(); 

			for (int i = 0; i < methods.Length; i++)
			{
				if (methods[i].Name == "LoadWindowLayout")
				{
					// As with all Unity reflection, this is relatively fragile.
					// Use test code here if/when needed to debug.
					//
					// Debug.Log("methods: " + methods[i].Name  + " and " + methods[i].GetParameters().Length + " \n\n");
					// for (int j = 0; j < methods[i].GetParameters().Length; j++)
					// {
					// 	Debug.Log("\tparams: " + methods[i].GetParameters()[j].Name  + " and " + methods[i].GetParameters()[j].ParameterType + " \n\n");
					// }
					
					// Tested with success in Unity 2020.3.34f1
					if (methods[i].GetParameters().Length == 2)
					{
						try
						{
							methods[i].Invoke(null, new object[] { path, false });
						}
						catch (Exception e)
						{
							Debug.LogError(e.Message);
						}
					}
				}
			}
		}
	}
}

