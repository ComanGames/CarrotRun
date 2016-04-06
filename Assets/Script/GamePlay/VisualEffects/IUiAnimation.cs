using System;

namespace Assets.Script.GamePlay.VisualEffects
{
    public interface IUiAnimation
    {
        event Action AnimationDone;
        void RunAnimation();
        void AnimationEnd();
    }
}