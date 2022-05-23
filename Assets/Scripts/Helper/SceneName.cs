namespace fallingball
{
    namespace helper
    {
        public enum SceneName
        {
            IntroScene,
            LoadingScene,
            MainMenuScene,
            GameScene,
            None
        }

        public static class SceneNameExtension
        {
            public static string GetName(this SceneName sceneName)
            {
                switch (sceneName)
                {
                    case SceneName.IntroScene:
                    case SceneName.MainMenuScene:
                    case SceneName.LoadingScene:
                    case SceneName.GameScene:
                    default: return sceneName.ToString();
                }
            }
        }
    }
}
