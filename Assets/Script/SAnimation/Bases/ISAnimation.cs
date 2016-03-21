using System.Collections;
using UnityEngine; 
namespace Assets.Script.SAnimation.Bases
{
	public interface ISAnimation
	{
		void Starter();
		void LoadContainers();
		void PreLoadAnimation();
		void FreeMemory();
		void StartAnimation();
		void ResetAnimation();
		void StopAnimation();
		void GoToNextFrame(int count);
		IEnumerator UpdatingSprite();

		Transform ThisTransform { get; }
		GameObject ThisGameObject { get; }
		Vector2 Size { get; } 
}
}