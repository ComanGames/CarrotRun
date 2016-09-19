using System;
using System.Collections.Generic;
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
        public GameObject BackGroundProblem;
        public GameObject ProblemContiner;
        //private variables
        public ProblemContainer CreateProblem(ProblemInfo problemInfo)
        {
            GameObject resultGameObject = ProblemContiner.Spawn(transform);
            ProblemContainer result  = resultGameObject.GetComponent<ProblemContainer>();
            for (int i = 0; i < problemInfo.ProblemBlocks.Length; i++)
            {
                GameObject newBlock = CreateBlock(problemInfo.ProblemBlocks[i],resultGameObject.transform,i);
                //Add the last as last
                if (i == problemInfo.ProblemBlocks.Length - 1)
                    result.Last = newBlock.transform;
            }
            return result;
        }

        public void DestroyProblem(Transform problem)
        {
            List<Transform> childrens = new List<Transform>();
            foreach (Transform child in problem)
            {
                childrens.Add(child);
            }
            foreach (Transform child in childrens)
            {
                child.parent = child.root;
                child.Recycle();
            }
            problem.Recycle();
        }
        private GameObject CreateBlock(BlockInfo problemBlock, Transform parent,int index)
        {
           GameObject newBlock  = BlockInstance(problemBlock).Spawn(parent);
            //fix for ground blocks
            if (problemBlock.TypeOfBlock == BlockType.FlyingGround)
            {
                newBlock.GetComponentInChildren<SpriteRenderer>().sortingOrder = index;
            }
            if (problemBlock.TypeOfBlock == BlockType.FlyingProblem 
                || problemBlock.TypeOfBlock == BlockType.Problem
                ||problemBlock.TypeOfBlock == BlockType.Coin)
            {
                SAnimation.SAnimation sAnimation = newBlock.GetComponentInChildren<SAnimation.SAnimation>();
                sAnimation?.StartAnimation();

//                newBlock.GetComponentInChildren<SAnimation.SAnimation>().StartAnimation();
            }
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
                    case BlockType.BackGroundProblem:
                    return BackGroundProblem;
            }
            throw new ArgumentException();
        }
    }

}