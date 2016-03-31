using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Assets.Script.DataManagment;
using Assets.Script.GamePlay;
using Assets.Script.GamePlay.Data_Containers;
using UniRx;
using UnityEngine;

namespace Assets.Script.Menu.DataManagment
{
	[Serializable]
	public struct SoundSettings
	{
		public SoundSettings(float sound, float effects)
		{
			Sound = sound;
			Effects = effects;
		}

		public float Sound;
		public float Effects;
	}
	[Serializable]
	public class Gameinfo
	{
		public Gameinfo()
		{
			GameSoundSettings = new SoundSettings(0.75f,0.9f);
			CurrentCharacter = Characters.Carrot;
			_realCoins = 1500;
			CurrentName = "Player";
		}
		public string CurrentName;
		public Characters CurrentCharacter;
		public ulong Coins
		{
			get { return _realCoins; }
			set { _realCoins = value; } 
		}
		private ulong _realCoins;
		public SoundSettings GameSoundSettings;
		public Dictionary<Characters,bool> OpenCharacters;
	} 
    public static class FileManager
    {
	    static FileManager()
	    {
		    _dataPath = Application.persistentDataPath;

	    }
		//Global parameters
	    private const string ScoreFile = "ScoreList.dat";
	    private const string GameRecordFile = "Gameinfo.dat";
	    private static string _dataPath;

	    public static void SaveScore()
	    {
		    Observable.Start(SaveScoreProccess);
		}

	    public static void SaveGameRecord()
	    {
		    Observable.Start(SaveGameRecordProccess);
	    }

	    private static void SaveScoreProccess()
	    {
		    string scoreFilePath = _dataPath + "/" + ScoreFile;
		    //now start saving
		    BinaryFormatter bf = new BinaryFormatter();
		    FileStream file;
		    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
		    if (File.Exists(scoreFilePath))
		    {
			    try
			    {
				    file = File.OpenWrite(scoreFilePath);
			    }
			    catch (Exception)
			    {
				    Thread.Sleep(100);
					SaveScoreProccess();
				    return;
			    }
		    }
		    else
			    file = File.Create(scoreFilePath);
			bf.Serialize(file, GameData.Instance.PlayerRecords);
		    file.Close();
	    }
	    private static void SaveGameRecordProccess()
	    {
			//now start saving
		    string recordFilePath = _dataPath + "/" + GameRecordFile;
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file;
			// ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
			if (File.Exists(recordFilePath))
			{
				try
				{
					file = File.OpenWrite(recordFilePath);
				}
				catch (Exception)
				{
					Thread.Sleep(100);
					SaveScoreProccess();
					return;
				}
			}
			else
				file = File.Create(recordFilePath);
			bf.Serialize(file, GameData.Instance.CurrentGameinfo);
			file.Close();
		}
		public static List<ScoreInfo> LoadScore()
        {
                if (File.Exists(String.Format(_dataPath + "/" + ScoreFile)))
                {
                    var bf = new BinaryFormatter();
                    var file = File.Open(_dataPath + "/" + ScoreFile , FileMode.Open);
					return ((List<ScoreInfo>) bf.Deserialize(file));
                }
	        return new List<ScoreInfo>();
        }

	    public static Gameinfo LoadGameInfo()
	    {
		    
			if (IsLastRecordExists())
				{
				var bf = new BinaryFormatter();
				var file = File.Open(_dataPath + "/" + GameRecordFile , FileMode.Open);
				return ((Gameinfo) bf.Deserialize(file));
			}
			return new Gameinfo();
	    }

	    public static bool IsLastRecordExists()
	    {
		    return File.Exists(String.Format(_dataPath + "/" + GameRecordFile));
	    }

	    public static void FreeMemory()
	    {
			ResourceUnloder.UnoadResources();
			GC.Collect();
	    }
    }
}