using System;
using Assets.Script.DataManagment;
using UnityEngine;

namespace Assets.Script.Menu.DataManagment
{
	public class SceneManager
	{
		public static LoadingManager CurrentLoadingManager;
		public static SoundController CurrentSoundController;
		public static bool NowCharacterSelecter;
		public static bool IsGamePreloaded;
		public static bool IsFirstGame;
		public static bool IsGameSound;
		private const int ScaneGameNumber = 3;

		public static void PreloadGame()
		{
			if(!IsGamePreloaded)
			{
				IsFirstGame = GameData.Instance.IsFirstGame;
				if(!IsFirstGame)
				if(GameData.Instance.PlayerRecords==null)
					throw new ArgumentNullException();
				IsGamePreloaded = true;
			}
		}
		
		public static void LoadGame()
		{
			if (CurrentLoadingManager != null)
			{
				CurrentLoadingManager.StartTransactionOut();
				CurrentLoadingManager.TranactionOutEnd += LoadingLevelProcess;
			}
			else
			{
				LoadingLevelProcess();
			}
		}

		private static void LoadingLevelProcess()
		{
			LoadSceneNumber(ScaneGameNumber);
			if (CurrentLoadingManager != null)
				CurrentLoadingManager.TranactionInEnd += ChangeToGameSound;
		}

		private static void ChangeToMenuSound()
		{
			
			if (CurrentSoundController != null)
			{
				CurrentSoundController.ChangeSoundTrack(0);
				IsGameSound = false;
			}
		}

		private static void ChangeToGameSound()
		{
			if (CurrentSoundController != null)
			{
				IsGameSound = true;
				CurrentSoundController.ChangeSoundTrack(1);
			}
		}

		public static void LoadCharactersSelector()
		{
			NowCharacterSelecter = true;
			if (CurrentLoadingManager != null)
			{
				CurrentLoadingManager.StartTransactionOut();
				CurrentLoadingManager.TranactionOutEnd += LoadingCharacterSelectorProccess;
			}
			else
			{
				LoadingCharacterSelectorProccess();
			}
		}

		private static void LoadingCharacterSelectorProccess()
		{
			LoadSceneNumber(1);
		}

		private static void CleanMemoryIfNeed()
		{
			FileManager.FreeMemory();
		}

		public static void LoadRecords()
		{
				
			if (CurrentLoadingManager != null)
			{
				CurrentLoadingManager.StartTransactionOut();
				CurrentLoadingManager.TranactionOutEnd += LoadingRecordsProccess;
			}
			else
			{
				LoadingRecordsProccess();
			}
		}

		private static void LoadingRecordsProccess()
		{
			LoadSceneNumber(2);
			CurrentLoadingManager.TranactionOutEnd += LoadingRecordsProccess;
		}

		private static void LoadSceneNumber(int number)
		{
			CleanMemoryIfNeed();
			if (NowCharacterSelecter)
			{
				NowCharacterSelecter = false;
			}
#pragma warning disable 618
			Application.LoadLevel(number);
#pragma warning restore 618
			if (CurrentLoadingManager != null)
			{
				CurrentLoadingManager.StartTransactionIn();
				if (IsGameSound)
				{
					CurrentLoadingManager.TranactionInEnd += ChangeToMenuSound;
				}
			}
		}

		public static void QuitGame()
		{
			Application.Quit();
		}

		public static void LoadMenu()
		{
			if (CurrentLoadingManager != null)
			{
				CurrentLoadingManager.StartTransactionOut();
				CurrentLoadingManager.TranactionOutEnd += LoadingMenuProccess;
			}
			else
			{
				LoadingMenuProccess();
			}
		}

		private static void LoadingMenuProccess()
		{
			LoadSceneNumber(0);
		}
	}

}