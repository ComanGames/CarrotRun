using System;
using Assets.Script.GamePlay;
using Assets.Script.GamePlay.Data_Containers;
using Assets.Script.iTween;
using UnityEngine;

namespace Assets.Script.CharacterSelecter
{
    public class CharacterCard : MonoBehaviour
    {
        #region Varaibles
        //Public Variables 
		public Characters Character;
	    public bool IsOpen;
	    public float BigSizeScale = 2;
	    public float DistanceUp = 3;
	    //Private Variables
	    private SAnimation.SAnimation _animationSprite;
	    private Vector2 _normalSize;
	    private float _normalPositionY;
	    private bool _startingAnimation;
	    #endregion

	    public void Start()
	    {
	        _animationSprite =transform.GetChild(0).GetComponent<SAnimation.SAnimation>();
		    _normalPositionY = _animationSprite.transform.localPosition.y;
		    _normalSize = _animationSprite.transform.localScale;
		    if(_animationSprite==null) throw new InvalidOperationException();
		    if (!IsOpen)
		    {
			    _animationSprite.GetComponent<SpriteRenderer>().color = Color.black;
		    }
		    SetAnimationSprite();
	    }

	    public void OpenCard()
	    {
		   SetSpriteOpen(); 
			StartAnimationAndShowCard();
			StartAnimation();
	    }
	    public void SetSpriteOpen()
	    {
			if(_animationSprite==null)
				_animationSprite =transform.GetChild(0).GetComponent<SAnimation.SAnimation>();
		    _animationSprite.GetComponent<SpriteRenderer>().color = Color.white;
	    }
	    private void SetAnimationSprite()
	    {
		    _animationSprite.UseTimeScale = false;
		    _animationSprite.AutoStart = false;
	    }

	    public void MakeBig()
        {
		    ITween.EaseType easeType = ITween.EaseType.easeInQuad;
		    ITween.ScaleTo(gameObject, ITween.Hash("x", _normalSize.x*BigSizeScale, "y", _normalSize.y*BigSizeScale, "easeType",easeType,"time", CharactersUiController.Instance().CharacterAnimationTime, "onComplete", "StartAnimation"));
		    ITween.MoveTo(_animationSprite.gameObject, ITween.Hash("y",_normalPositionY+DistanceUp, "easeType", easeType, "islocal", true, "time",  CharactersUiController.Instance().CharacterAnimationTime ));
			transform.SetSiblingIndex(20);
		    _animationSprite.GetComponent<SpriteRenderer>().sortingOrder = 100;
			if(IsOpen)
				StartAnimationAndShowCard();
        }

	    private void StartAnimationAndShowCard()
	    {
		    _startingAnimation = true;
	    }

	    public void MakeNormal()
        {
		        ITween.ScaleTo( gameObject, ITween.Hash("x", _normalSize.x, "y", _normalSize.y, "time", CharactersUiController.Instance().CharacterAnimationTime, "onComplete", "FreeMemory"));
		        ITween.MoveTo( _animationSprite.gameObject, ITween.Hash( "y", _normalPositionY, "islocal", true, "time", CharactersUiController.Instance().CharacterAnimationTime));
		        _animationSprite.ResetAnimation();
			_animationSprite.StopAnimation();
		    _animationSprite.GetComponent<SpriteRenderer>().sortingOrder = 0;
		        _startingAnimation = false;
        }

		public void StartAnimation()
		{
			if(_startingAnimation)
				_animationSprite.StartAnimation();
		}
    }
}