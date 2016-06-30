using Assets.Script.GamePlay;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.MyTools
{
    public class FixProblemPosition
    {
        public const float TopDistance = -0.03396f;
        public const float CoinPosition = 4.3f;
        [MenuItem("MyTools/Fix Coins")]
        public static void FixingCoins()
        {
            Transform[] transforms = Selection.transforms;
            foreach (Transform transform in transforms)
            {
                FixConPositionChildes(transform);
            }
        }

        private static void FixConPositionChildes(Transform transform)
        {
            foreach (Transform transformChild in transform)
            {
                if (transformChild.GetComponentInChildren<Coin>() != null)
                {
                   Transform ourCoinTransform = transformChild.GetChild(0);
                   ourCoinTransform.localPosition = new Vector3(0,CoinPosition);
                }
            }
        }


        [MenuItem("MyTools/Fix Problem")]
        public static void FixingPosition()
        {
            Transform[] transforms = Selection.transforms;
            foreach (Transform transform in transforms)
            {
                FixPositionForChildes(transform);
            }
        }

        private static void FixPositionForChildes(Transform transform)
        {
            foreach (Transform transformChild in transform)
            {
                transformChild.localPosition = new Vector3(transformChild.localPosition.x,transformChild.localPosition.x*TopDistance);
            }
        }
    }
}