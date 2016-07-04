#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.GamePlay;
using Assets.Script.GamePlay.PoolUtilities;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.SAnimation.Bakers
{
    public class SerializationUtilits
    {
        private static readonly string BakeFileName = "Bake.asset";

        public static ProblemInfo SerializeProblem(ProblemContainer problem)
        {
            ProblemInfo problemInfo = ScriptableObject.CreateInstance<ProblemInfo>();
            List<BlockInfo> listOfBlockInfos = new List<BlockInfo>();
            for (int i = 0; i < problem.transform.childCount; i++)
            {
                BlockInfo bi = BlockInfo.InfoOfBlock(problem.transform.GetChild(i).gameObject);
                listOfBlockInfos.Add(bi);
            }
            listOfBlockInfos.Sort((el1,el2)=>el1.Position.x.CompareTo(el2.Position.x));
            problemInfo.ProblemBlocks = listOfBlockInfos.ToArray();
            string folderName = @"Assets/Problems/";
            AssetDatabase.CreateAsset(problemInfo, folderName +problem.gameObject.name+".asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = problemInfo;
            return problemInfo;
        }
	    public static CLLContainer SerialazingAnimation(Sprite[] spritesArr)
	    {
			if(spritesArr==null||spritesArr.Length<1)
				throw  new ArgumentException();
		    Sprite[] sprites = spritesArr;
		    sprites = sprites.OrderBy(s => s.name).ToArray();
			 return CircleLinkedListToScriptableObject(sprites,FolderNameOfSprite(sprites[0]));
	    }

	    private static CLLContainer CircleLinkedListToScriptableObject(Sprite[] sprites,string folderName )
	    {
		    CLLContainer aniationContainer = ScriptableObject.CreateInstance<CLLContainer>();
		    aniationContainer.Sprites = sprites;
		    AssetDatabase.CreateAsset(aniationContainer, folderName+BakeFileName);
		    AssetDatabase.SaveAssets();
		    Selection.activeObject = aniationContainer;
		    return aniationContainer;
	    }

	    public static string FolderNameOfSprite(Sprite sprite)
	    {
		    string folderName = AssetDatabase.GetAssetPath(sprite);
		    folderName = folderName.Replace(sprite.name + ".png", "");
		    return folderName;
	    }
    }
}
#endif