using System;
using Assets.Script.DataManagment;
using Assets.Script.GamePlay.Skills;
using Assets.Script.Menu.DataManagment;
using Assets.Script.SAnimation;
using Assets.Script.SAnimation.Bakers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.GamePlay
{
	public class SceneSetter : MonoBehaviour
	{
		public Image JuiceImage;
		public Image JuiceImage2;
		public Hero HeroCharacter;
		public SAnimation.SAnimation FactoryAnimation;
		public CharacterSceneSetting[] CharacterScene;

		public void Awake()
		{
			Characters character = GameData.Instance.CurrentGameinfo.CurrentCharacter;
			CharacterSceneSetting currentCharacterScene = CharacterScene[0];
			foreach (CharacterSceneSetting t in CharacterScene)
				if (t.Name == character)
					currentCharacterScene = t;
			SetScane(currentCharacterScene);
			Destroy(this);
		}

		private void SetScane(CharacterSceneSetting currentCharacterScene)
		{
			JuiceImage.color= JuiceImage2.color = currentCharacterScene.JucieColor;
			GameObject newSprite = GetCharacter(currentCharacterScene.Name.ToString());
			SetSkillToCharacter(newSprite,currentCharacterScene.Name);
			HeroCharacter.MyAnimator = newSprite.GetComponent<SHeroAnimator>();
			HeroCharacter.MyAnimator.Starter();
			HeroCharacter.SetHero();
			FactoryAnimation.Container = GetFactoryContainer(currentCharacterScene.FactoryName);
			FactoryAnimation.LoadContainers();
		}

		private void SetSkillToCharacter(GameObject heroCharacter, Characters characters)
		{
			ISkillHero skill;
			switch (characters)
			{
					case Characters.Apple:
					skill = heroCharacter.GetComponent<SkillApple>();
					break;
					case Characters.Banana:
					skill = heroCharacter.GetComponent<SkillBanana>();
					break;
					case Characters.Carrot:
					skill = heroCharacter.GetComponent<SkillCarrot>();
					break;
					default:
					skill = new EmptySkill();
					break;
			}
			HeroCharacter.Skill = skill;
		}

		private CLLContainer GetFactoryContainer(string factoryName)
		{
			return Resources.Load<CLLContainer>(@"Factory/"+ factoryName);
		}

		private GameObject GetCharacter(string characterName)
		{
			GameObject newSprite = Instantiate(Resources.Load<GameObject>(@"Characters/" + characterName));
			newSprite.transform.parent = HeroCharacter.transform;
			newSprite.transform.localPosition = Vector3.zero;
			newSprite.transform.localScale = new Vector3(1.5f,1.5f);
			return newSprite;
		}


		[Serializable]
		public class CharacterSceneSetting
		{
			public Characters Name;
			public Color JucieColor;
			public string FactoryName;
		}
	}
}