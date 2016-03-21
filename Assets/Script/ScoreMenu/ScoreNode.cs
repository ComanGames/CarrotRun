using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.ScoreMenu
{
	[Serializable]
    public class ScoreNode : MonoBehaviour
    {
        //Public Variables 
        public Text Name;
        public Text Score;
	    public Image CharacterIcon;
        //Private Variables
        public void SetNameAndScore(string playerName, string score,Sprite characterIcon)
        {
            Name.text = playerName;
            Score.text = score;
	        CharacterIcon.sprite = characterIcon;
        }
    }
}