using System;
using Assets.Script.GamePlay.PoolUtilities;

namespace Assets.Script.GamePlay.Data_Containers
{
    [Serializable]
    public class Level
    {
        #region  Variables

        public ProblemInfo[] Items;

        #endregion

        #region Proporties

        public int Count
        {
            // ReSharper disable once ConvertPropertyToExpressionBody
            get
            {
                if (Items == null)
                    return 0;
                return Items.Length;
            }
        }

        #endregion

        public ProblemInfo this[int index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }
    }
}