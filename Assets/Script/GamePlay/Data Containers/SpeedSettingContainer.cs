using System;

namespace Assets.Script.GamePlay.Data_Containers
{
    [Serializable]
    public class SpeedSettingContainer
    {
        #region  Variables

        public float StartSpeed;
        public float MaxTimeScale = 30;
        public float SpeedScale;
        public int SpeedingUpCount;
        public float SpeedTimeOut;
        public float Acceleration;

        #endregion
    }
}