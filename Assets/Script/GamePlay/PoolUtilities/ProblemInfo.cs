using System;
using UnityEngine;

namespace Assets.Script.GamePlay.PoolUtilities
{
    [Serializable]
    public class ProblemInfo:ScriptableObject
    {
        public BlockInfo[] ProblemBlocks;
    }
}