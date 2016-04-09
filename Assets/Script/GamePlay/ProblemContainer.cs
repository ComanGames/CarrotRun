using System;
using UnityEngine;

namespace Assets.Script.GamePlay
{
    public class ProblemContainer : MonoBehaviour
    {
        #region Variables

        public Transform Last;

        #endregion

        public float Length()
        {
            if(Last==null)
                throw new NullReferenceException("Last is equals null");
            return Last.transform.localPosition.x;
        }
    }
}