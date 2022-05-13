using UnityEngine;
using UnityEditor;

namespace MoralisUnity.Sdk.UI.ReadMe
{
	/// <summary>
	/// Format a ReadMe for prettyfied viewing in the Unity Inspector
	///
	/// Inspired by Unity's "Learn" Sample Projects
	///
	/// </summary>
	[CustomEditor(typeof(ReadMe))]
	[InitializeOnLoad]
	public class ReadMeEditor : UnityEditor.Editor
	{
		static float vSpaceAfterSection = 2f;
		private static float vSpaceBeforeSection = 8;
		static float hSpace1 = 8;
		static float hSpace2 = 16;
		
		protected static ReadMe SelectReadme(string findAssetsFilter, string[] findAssetsFolders)
		{
			var ids = AssetDatabase.FindAssets(findAssetsFilter, findAssetsFolders);

			if (ids.Length == 1)
			{
				var pathToReadme = AssetDatabase.GUIDToAssetPath(ids[0]);
				return SelectReadme(pathToReadme);
			}
			else if (ids.Length > 1)
			{
				Debug.LogError("SelectReadme() Too many results found for Readme.");
			}
			else
			{
				//Debug.LogError("SelectReadme() No results found for Readme.");
			}

			return null;
		}

		public static ReadMe SelectReadmeGuid(string guid )
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			return SelectReadme(path);
		} 
		
		
		private static ReadMe SelectReadme(string pathToReadme)
		{
			if (string.IsNullOrEmpty (pathToReadme))
			{
				return null;
			}
			var readmeObject = AssetDatabase.LoadMainAssetAtPath(pathToReadme);

			if (readmeObject == null)
			{
				return null;
			}

			var editorAsm = typeof(UnityEditor.Editor).Assembly;
			var inspWndType = editorAsm.GetType("UnityEditor.InspectorWindow");
			var window = EditorWindow.GetWindow(inspWndType);
			window.Focus();

			Selection.objects = new UnityEngine.Object[] { readmeObject };
			return (ReadMe)readmeObject;
		}

		protected override void OnHeaderGUI()
		{
			var readme = (ReadMe)target;
			Init();

			var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

			GUILayout.BeginHorizontal("In BigTitle");
			{
				GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
				GUILayout.Label(readme.title, TitleStyle);
			}
			GUILayout.EndHorizontal();
		}

      public override void OnInspectorGUI()
		{
			var readme = (ReadMe)target;
			
			Init();

			foreach (var section in readme.sections)
			{
				if (!string.IsNullOrEmpty(section.heading))
				{
					GUILayout.Space(vSpaceBeforeSection);
					GUILayout.Label(section.heading, HeadingStyle);
					GUILayout.Space(3);
				}
				
				if (!string.IsNullOrEmpty(section.subheading))
				{
					GUILayout.Space(5);
					GUILayout.BeginHorizontal();
					GUILayout.Space(hSpace1);
					GUILayout.Label(section.subheading, SubheadingStyle);
					GUILayout.EndHorizontal();
					GUILayout.Space(3);
				}
				
				if (!string.IsNullOrEmpty(section.text))
				{
					GUILayout.BeginHorizontal();
					GUILayout.Space(hSpace2);
					GUILayout.Label(section.text, BodyStyle);
					GUILayout.EndHorizontal();
					GUILayout.Space(3);
				}
	
				if (!string.IsNullOrEmpty(section.linkText))
				{
					GUILayout.BeginHorizontal();
					GUILayout.Space(hSpace2);
					GUILayout.Label("▶");
					if (LinkLabel(new GUIContent(section.linkText)))
					{
						Application.OpenURL(section.url);
					}
					GUILayout.Space(1000);
					GUILayout.EndHorizontal();

				}
				
				if (!string.IsNullOrEmpty(section.pingObjectName))
				{
					GUILayout.BeginHorizontal();
					GUILayout.Space(hSpace2);
					GUILayout.Label("▶");
					if (LinkLabel(new GUIContent(section.pingObjectName)))
					{
						string path = AssetDatabase.GUIDToAssetPath(section.pingObjectGuid);
						var objectToSelect = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
						EditorGUIUtility.PingObject(objectToSelect);
						
						// Do not select it.
						// Since For most users that would un-select the readme.asset and disorient.
						//Selection.activeObject = objectToSelect;
					}
					GUILayout.Space(1000);
					GUILayout.EndHorizontal();
				}
				if (!string.IsNullOrEmpty(section.openMenuName))
				{
					GUILayout.BeginHorizontal();
					GUILayout.Space(hSpace2);
					GUILayout.Label("▶");
					if (LinkLabel(new GUIContent(section.openMenuName)))
					{
						EditorApplication.ExecuteMenuItem(section.openMenuNameId);
					}
					GUILayout.Space(1000);
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(vSpaceAfterSection);
			}
		}


		bool m_Initialized;

		GUIStyle LinkStyle { get { return m_LinkStyle; } }
		[SerializeField] GUIStyle m_LinkStyle;

		GUIStyle TitleStyle { get { return m_TitleStyle; } }
		[SerializeField] GUIStyle m_TitleStyle;

		GUIStyle HeadingStyle { get { return m_HeadingStyle; } }
		[SerializeField] GUIStyle m_HeadingStyle;

		GUIStyle SubheadingStyle { get { return m_SubheadingStyle; } }
		[SerializeField] GUIStyle m_SubheadingStyle;

		GUIStyle BodyStyle { get { return m_BodyStyle; } }
		[SerializeField] GUIStyle m_BodyStyle;

		void Init()
		{
			if (m_Initialized)
				return;
			m_BodyStyle = new GUIStyle(EditorStyles.label);
			m_BodyStyle.wordWrap = true;
			m_BodyStyle.fontSize = 14;
			
			m_TitleStyle = new GUIStyle(m_BodyStyle);
			m_TitleStyle.fontSize = 26;
			m_TitleStyle.margin.top = 25;

			m_HeadingStyle = new GUIStyle(m_BodyStyle);
			m_HeadingStyle.fontSize = 18;
			
			m_SubheadingStyle = new GUIStyle(m_BodyStyle);
			m_SubheadingStyle.fontStyle = FontStyle.Bold;
			m_SubheadingStyle.fontSize = 12;
			
			m_LinkStyle = new GUIStyle(m_BodyStyle);
			m_LinkStyle.wordWrap = false;
			// Match selection color which works nicely for both light and dark skins
			m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0xC3 / 255f, 0xF8 / 255f, 0xFF);
			m_LinkStyle.stretchWidth = false;

			m_Initialized = true;
		}

		bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
		{
			var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

			Handles.BeginGUI();
			Handles.color = LinkStyle.normal.textColor;
			Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
			Handles.color = Color.white;
			Handles.EndGUI();

			EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

			return GUI.Button(position, label, LinkStyle);
		}
	}
}
