#if UNITY_EDITOR
using Assets.Script.DataManagment;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
	public class CharactersAssetCreator
	{
		[MenuItem("Assets/Create/CharacterAsset")]
		public static void CreateCharacterAsset()
		{
			CharactersAsset charactersAsset = ScriptableObject.CreateInstance<CharactersAsset>();
			AssetDatabase.CreateAsset(charactersAsset,"Assets/Resources/Characters.asset");
			AssetDatabase.SaveAssets();

			EditorUtility.FocusProjectWindow();

			Selection.activeObject = charactersAsset;
		}
	}
}
#endif