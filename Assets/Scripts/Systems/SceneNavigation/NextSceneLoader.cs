using UnityEngine.SceneManagement;

public class NextSceneLoader : OneActionPerformerWithBuildIndex
{
    public override void PerformAction()
    {
        SceneManager.LoadScene(BuildIndex + 1);
    }
}