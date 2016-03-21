using Assets.Script.SAnimation.Bakers;
using Assets.Script.SAnimation.Bases;

namespace Assets.Script.SAnimation
{
	public class SAnimationUi : SpriteAnimationUi
	{
		#region Variables

		//Public Variables 
		public CLLContainer Container;
		//private Variables

		#endregion
		public override void LoadContainers()
		{
			AnimationContainer = Container.GetCircleLinkedList();
			Loaded = true;
		}

	}
}