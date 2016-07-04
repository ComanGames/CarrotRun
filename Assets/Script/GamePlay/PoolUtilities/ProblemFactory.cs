
using System;
using Assets.Script.ObjectPool.Scripts;
using UnityEngine;

namespace Assets.Script.GamePlay.PoolUtilities
{
    public class ProblemFactory : MonoBehaviour
    {
        public ObjectPool.Scripts.ObjectPool Pool;
        public GameObject Coin;
        public GameObject Problem;
        public GameObject FlyingProblem;
        public GameObject FlyingGround;
        public GameObject ProblemContiner;
        //private variables
        public GameObject CreateProblem(ProblemInfo problemInfo)
        {
            GameObject result = ProblemContiner.Spawn(transform);
            for (int i = 0; i < problemInfo.ProblemBlocks.Length; i++)
            {
                GameObject newBlock = CreateBlock(problemInfo.ProblemBlocks[i],result.transform);
                //Add the last as last
                if (i == problemInfo.ProblemBlocks.Length - 1)
                    result.GetComponent<ProblemContainer>().Last = newBlock.transform;
            }
            return result;
        }

        public GameObject CreateBlock(BlockInfo problemBlock, Transform parent)
        {
           GameObject newBlock  = BlockInstance(problemBlock).Spawn(parent);
            newBlock.transform.localPosition = problemBlock.Position;
            return newBlock;
        }

        private GameObject BlockInstance(BlockInfo blockInfo)
        {
            switch (blockInfo.TypeOfBlock)
            {
                    case BlockType.Coin:
                    return Coin;
                    case BlockType.Problem:
                    return Problem;
                    case BlockType.FlyingProblem:
                    return FlyingProblem;
                    case BlockType.FlyingGround:
                    return FlyingGround;
            }
            throw new ArgumentException();
        }
    }

}