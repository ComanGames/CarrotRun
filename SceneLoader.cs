using System;
using System.Collections.Generic;
using Assets.Script.GamePlay;
using UnityEngine;

[Serializable]

public class SceneLoader : MonoBehaviour
{
	public class CharacterInfo
	{
		public Characters Character;
		public Sprite CharacterIcon;
		public byte CharacterLevel;
	}
	public static SceneLoader Instance;
	public CharacterInfo[] CharacterAndInfo;
	public Dictionary<Characters, CharacterInfo> IconsDictionary;
	public Sprite CarrotIcon;

	public void Awake()
	{
		if (Instance == null)
		{
			SetInfoDictionary();
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

	}

	private void SetInfoDictionary()
	{
		IconsDictionary = new Dictionary<Characters, CharacterInfo>();
		foreach (CharacterInfo characterIcon in CharacterAndInfo)
			IconsDictionary.Add(characterIcon.Character, characterIcon);
	}

	public static Sprite GetCharacterIcon(Characters character)
	{
		CharacterInfo characterInfo;
		Instance.IconsDictionary.TryGetValue(character, out characterInfo);
		if (characterInfo != null)
			return characterInfo.CharacterIcon;
		return Instance.CarrotIcon;
	}
}