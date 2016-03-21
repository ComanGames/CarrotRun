using Assets.Script.Menu.DataManagment;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Menu
{
	public class SetAudioLevels : MonoBehaviour
	{
		public Slider MusicSlider;
		public Slider SoundFxSlider;
		public void Awake()
		{
			if(MusicSlider!=null&&SoundFxSlider!=null&&SceneManager.CurrentSoundController!=null)
			{
				MusicSlider.value = SceneManager.CurrentSoundController.GetSoundProcentVolume();
				SoundFxSlider.value = SceneManager.CurrentSoundController.GetFxProcentVolume();
			}
		}

		//Call this function and pass in the float parameter musicLvl to set the volume of the AudioMixerGroup Music in mainMixer
		public void SetMusicLevel(float musicLvl)
		{
			if(SceneManager.CurrentSoundController!=null)
				SceneManager.CurrentSoundController.ChangeSoundVolume(musicLvl);
		}

		//Call this function and pass in the float parameter sfxLevel to set the volume of the AudioMixerGroup SoundFx in mainMixer
		public void SetSfxLevel(float sfxLevel)
		{
			if (SceneManager.CurrentSoundController != null)
				SceneManager.CurrentSoundController.ChangeFxVolume(sfxLevel);
		}
	}
}
