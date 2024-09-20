using UnityEngine.SceneManagement;
using Zenject;

public abstract class OneActionPerformerWithBuildIndex : OneActionPerformer, IInitializable
{
    protected int BuildIndex { get; private set; }

    public void Initialize()
    {
        BuildIndex = SceneManager.GetActiveScene().buildIndex;
    }
}