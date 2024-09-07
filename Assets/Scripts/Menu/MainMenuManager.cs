using GDT1.Game.System;
using GDT1.Inputs;
using UnityEngine;

namespace GDT1
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject SplashMenuRoot;
        [SerializeField] private GameObject TutoMenuRoot;
        [SerializeField] private GameObject CreditsMenuRoot;

        [SerializeField] private MoveInput CharacterMoveComponent;
        [SerializeField] private Spawner Spawner;

        public void GoToGame()
        {
            SplashMenuRoot.SetActive(false);
            TutoMenuRoot.SetActive(false);
            CreditsMenuRoot.SetActive(false);

            CharacterMoveComponent.enabled = true;
            Spawner.StartSpawning();

            gameObject.SetActive(false);
        }

        public void GoToSplash()
        {
            SplashMenuRoot.SetActive(true);
            TutoMenuRoot.SetActive(false);
            CreditsMenuRoot.SetActive(false);
        }

        public void GoToTuto()
        {
            SplashMenuRoot.SetActive(false);
            TutoMenuRoot.SetActive(true);
            CreditsMenuRoot.SetActive(false);
        }

        public void GoToCredits()
        {
            SplashMenuRoot.SetActive(false);
            TutoMenuRoot.SetActive(false);
            CreditsMenuRoot.SetActive(true);
        }
    }
}
