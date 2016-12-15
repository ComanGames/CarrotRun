using System;
using System.Collections;
using Script.Menu.DataManagment;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Script.Menu.DataManagment
{

	internal class TimeAndTrack
	{
		public float Time;
		public int TrackNumber;

		public TimeAndTrack(float time, int trackNumber)
		{
			Time = time;
			TrackNumber = trackNumber;
		}
	}
	public class SoundController : MonoBehaviour
	{

		public AudioMixerSnapshot VolumeDown;   
		public AudioMixerSnapshot VolumeUp;
		public AudioMixer MainMixer;
		public AudioClip[] SoundTrecks;
		public static SoundController Instance;
		private AudioSource _audioSource;
		private static readonly string SoundFx= "sfxVol";
		private static readonly string MusicVol= "musicVol";
		private static readonly float VolumeMax = 80;
		private static readonly float TransactionTime = 1.5f;
		private static readonly float PauseTime = 0.5f;

		public bool IsGameSound;

		private bool _isSoundPaused;

		private bool _isGameEffectsPaused;


		public void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
			if (Instance == null)
			{
				Instance = this;
				SceneManager.CurrentSoundController = this;
				SetSoundToSaved();
			}
			else 
				Destroy(gameObject);
		}

		private void SetSoundToSaved()
		{
			ChangeSoundVolume(GameData.Instance.GameSoundSettings.Sound);
			ChangeFxVolume(GameData.Instance.GameSoundSettings.Effects );
		}

		public void ChangeSoundVolume(float volume)
		{
			MainMixer.SetFloat(MusicVol, Volume(volume));
			GameData.Instance.GameSoundSettings= new SoundSettings(volume,GameData.Instance.GameSoundSettings.Effects);
		}

		public void ChangeFxVolume(float volume)
		{
			MainMixer.SetFloat(SoundFx, Volume(volume));
			GameData.Instance.GameSoundSettings = new SoundSettings(GameData.Instance.GameSoundSettings.Sound, volume);

		}

		public float GetSoundProcentVolume()
		{
			return GameData.Instance.GameSoundSettings.Sound;
		}

		public float GetFxProcentVolume()
		{
			return GameData.Instance.GameSoundSettings.Effects;
		}

	private static float ProcentToDb(float volume)
		{
			float linear = Mathf.Pow(10.0f, volume / 40.0f);
			return linear;
		}

		private static float Volume(float volume)
		{
			if (Math.Abs(volume) > 0.001f)
				volume = 40.0f*Mathf.Log10(volume);
			else
				return -VolumeMax;
			return volume;
		}

		public void ChangeSoundTrack(int number)
		{
			IsGameSound = (number == 1);
			if (Instance == null||number>=Instance.SoundTrecks.Length)
				return;
			StartCoroutine("ChangeSound",new TimeAndTrack(TransactionTime, number));
		}

		public IEnumerator ChangeSound(object timeAndTrack)
		{
			TimeAndTrack realTimeAndTrack = (TimeAndTrack) timeAndTrack;
			yield return new WaitForSeconds(realTimeAndTrack.Time);
			Instance._audioSource.clip = Instance.SoundTrecks[realTimeAndTrack.TrackNumber];
			Instance._audioSource.Play();
			if(!_isSoundPaused)
			{
				TurnOnSound(realTimeAndTrack.Time);
			}
		}


		private float _lastSoundFxVal = 1;
		private bool _isStoped;
		public void StopGameEffects()
		{
			if (MainMixer != null&&!_isStoped)
			{
				MainMixer.GetFloat(SoundFx,out _lastSoundFxVal);
				MainMixer.SetFloat(SoundFx, -80);
				_isStoped = true;
			}
		}

		public void ResumeGameEffects()
		{
			if (MainMixer != null &&_isStoped)
			{
				MainMixer.SetFloat(SoundFx, _lastSoundFxVal);
				_isStoped = false;
			}
		}

		public void PauseSound()
		{
			_isSoundPaused = true;
			TurnOffSound(PauseTime);
		}

		public void RePauseSound()
		{
			_isSoundPaused = false;
			TurnOnSound(PauseTime);
		}

		private void TurnOffSound(float time)
		{
			MainMixer.TransitionToSnapshots(new[] {VolumeDown}, new[] {1.0f}, time);
		}

		private void TurnOnSound(float time)
		{
			MainMixer.TransitionToSnapshots(new[] {VolumeUp}, new[] {1.0f}, time);
		}
	}
}