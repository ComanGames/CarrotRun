using Assets.Script.GamePlay.Participators;
using Assets.Script.SAnimation;
using Script.GamePlay.Participators;

namespace Assets.Script.GamePlay.Skills
{
	public interface ISkillHero
	{
		bool IsActive { get; }
		 int SkillValue { get; }
		bool ActivateSkill(int power,Hero hero,SHeroAnimator heroAnimator);
		void SetSkill();
		void Froze();
		void Unfroze();
		void ForceEnd();
	}
}