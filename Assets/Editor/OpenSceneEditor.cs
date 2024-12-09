using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Editor
{
    public class OpenSceneEditor : ScriptableObject
    {
        [MenuItem("OpenScene/Launcher")]
        public static void OpenScene_Launcher()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.SaveOpenScenes();
                EditorSceneManager.OpenScene("Assets/Scenes/Launcher.unity");
            }
        }

        [MenuItem("OpenScene/MainMenu")]
        public static void OpenScene_MainMenu()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.SaveOpenScenes();
                EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
            }
        }

        [MenuItem("OpenScene/GamePlay")]
        public static void OpenScene_GamePlay()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.SaveOpenScenes();
                EditorSceneManager.OpenScene("Assets/Scenes/GamePlay.unity");
            }
        }

        [MenuItem("GameLauncher/Play %#u")]
        public static void PlayFromLauncher()
        {
            if (EditorApplication.isPlaying == true)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Scenes/Launcher.unity");
            EditorApplication.isPlaying = true;
        }
    }
}