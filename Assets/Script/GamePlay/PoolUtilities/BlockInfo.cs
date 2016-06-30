using System;
using UnityEngine;

namespace Assets.Script.GamePlay.PoolUtilities
{
    [Serializable]
    public class BlockInfo
    {
        public BlockType TypeOfBlock;
        public Vector2 Position;

        public BlockInfo(BlockType typeOfBlock, Vector2 position)
        {
            TypeOfBlock = typeOfBlock;
            Position = position;
        }

        public static BlockInfo InfoOfBlock(GameObject gameObject)
        {
           return new BlockInfo(GetBlockType(gameObject.tag),gameObject.transform.localPosition);
        }

        public static BlockType GetBlockType(string tag)
        {
            switch (tag)
            {
                case "Coin":
                    return BlockType.Coin;
                case "Problem":
                    return BlockType.Problem;
                case "Flying Coin":
                    return BlockType.FlyingCoin;
                case "Flying Problem":
                    return BlockType.FlyingProblem;
                case "Flying Ground":
                    return BlockType.FlyingGround;
            } 
            throw new Exception("Undefined Block Type");
        }
    }
}