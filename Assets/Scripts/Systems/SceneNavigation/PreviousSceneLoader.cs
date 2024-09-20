using UnityEngine.SceneManagement;

public class PreviousSceneLoader : OneActionPerformerWithBuildIndex
{
    public override void PerformAction()
    {
        SceneManager.LoadScene(BuildIndex - 1);
    }
}