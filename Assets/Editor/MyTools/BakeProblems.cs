using Assets.Script.GamePlay;
using Assets.Script.SAnimation.Bakers;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.MyTools
{
    public class BakeProblems
    {
        [MenuItem("MyTools/Backe Problems")]

        public static void BakeSlectedProblems()
        {
            Transform[] transforms = Selection.transforms;
            foreach (Transform transform in transforms)
            {
                ProblemContainer problemContainer = transform.GetComponent<ProblemContainer>();
                if(problemContainer!=null)
                    SerializationUtilits.SerializeProblem(problemContainer);
            }

        }
    }
}