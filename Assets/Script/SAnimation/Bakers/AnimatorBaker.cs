#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Assets.Script.SAnimation.Bakers
{
    [Serializable]
    public class AnimatorState
    {
        public string Name;
        public Sprite[] Sprites;
    }


    public class AnimatorBaker:MonoBehaviour
    {
        #region Variables
        //Public Variables 
	    public bool DestoryIteself;
        public bool Bake;
        public string DeffaultAnimation;
        public AnimatorState[] AnimationStates;


        #endregion

        public void OnDrawGizmos()
        {
            if (Bake)
            {
                if (AnimationStates != null && AnimationStates.Length > 0)
                {
                    SAnimator sa = gameObject.AddComponent<SAnimator>();
                    sa.AnimationAddresses = new AnimationAddress[AnimationStates.Length];
                    for (int i = 0; i < AnimationStates.Length; i++)
                    {
                       CLLContainer container= SerializationUtilits.SerialazingAnimation(AnimationStates[i].Sprites);
	                    sa.AnimationAddresses[i] = new AnimationAddress(AnimationStates[i].Name, container);

                    }

                    sa.DeffaultAnimation = DeffaultAnimation == default(string) ? AnimationStates[0].Name : DeffaultAnimation;

	                if (DestoryIteself)
		                DestroyImmediate(this);
                }
                else
                {
                    Bake = false;

                }
            }
        }
    }
}
#endif