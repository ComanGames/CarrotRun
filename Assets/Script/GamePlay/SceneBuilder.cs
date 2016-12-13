using Assets.Script.GamePlay.Controller;
using Assets.Script.GamePlay.Data_Containers;
using Assets.Script.GamePlay.Participators;
using Assets.Script.Menu.DataManagment;
using Script.GamePlay.Participators;
using UnityEngine;

namespace Assets.Script.GamePlay
{
    public class SceneBuilder : MonoBehaviour
    {
        public SceneSetter SceneSetterComponent;
        public GameController Controller;
        public BlocksManager ManagerBlocks;
        public UserInterfaceManager ManagerUserInterface;

        public Hero Character;
        private Vector2 _playerStartPosition;
        public void Build()
        {
           SetScene(); 
        }
        public Hero BuildHero()
        {
            Character.SettControllerActions(Controller);
            _playerStartPosition = Character.transform.position;
            Character.SetHero();
            Character.Skill.SetSkill();
            return Character;
        }

        public void Stop()
        {
            Character.transform.position = _playerStartPosition;
            Character.Stop();
            ManagerBlocks.Stop();
        }

        private void SetScene()
        {
            Characters character = GameData.Instance.CurrentGameinfo.CurrentCharacter;
            SceneSetter.CharacterSceneSetting currentCharacterScene =  SceneSetterComponent.CharacterScene[0];
            foreach (SceneSetter.CharacterSceneSetting t in SceneSetterComponent.CharacterScene)
                if (t.Name == character)
                    currentCharacterScene = t;
            SceneSetterComponent.SetScane(currentCharacterScene);
            Destroy(SceneSetterComponent);
        }

        public void Reset()
        {
            Character.Reset();
            ManagerUserInterface.Reset();
            ManagerBlocks.Reset();
        }

        public UserInterfaceManager BuildUi()
        {
            ManagerUserInterface.Creator();
            return ManagerUserInterface;
        }

        public BlocksManager BuildBlockManager()
        {
            
            ManagerBlocks.Creator();
            return ManagerBlocks;
        }
    }
}