using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;

namespace Ophura
{
    internal static partial class Projection
    {
        [MenuItem("Tools/Ophura/Build Controller")]
        private static void BuildController()
        {
            AnimatorController Controller = CreateController();

            CreateInfluenceParameter(Controller);

            CreateSynchronizationParameters(Controller, GetAvatarDescriptor());

            CreateBufferParameters(Controller);

            CreateWorkspace(Controller);

            CreateSynchronizationLogic(Controller);

            EditorUtility.SetDirty(Controller);

            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();

            SetActiveAndPing(Controller.GetInstanceID());
        }
    }
}
