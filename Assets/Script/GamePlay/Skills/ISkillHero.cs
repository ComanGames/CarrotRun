using Assets.Script.GamePlay.Participators;
using Assets.Script.SAnimation;

namespace Assets.Script.GamePlay.Skills
{
	public interface ISkillHero
	{
		bool IsActive { get; }
		 int SkillValue { get; }
		bool ActivateSkill(float power,Hero hero,SHeroAnimator heroAnimator);
		void SetSkill();
		void Froze();
		void Unfroze();
		void ForceEnd();
	}
}