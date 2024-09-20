using System;
using UnityEngine.UI;
using Zenject;

public class OneActionButton : IInitializable, IDisposable
{
    protected Button m_Button { get; private set; }
    protected OneActionPerformer m_OneActionPerformer { get; private set; }

    public OneActionButton(Button button, OneActionPerformer oneActionPerformer)
    {
        m_Button = button;
        m_OneActionPerformer = oneActionPerformer;
    }

    public void Initialize()
    {
        m_Button.onClick.AddListener(OnButtonClicked);
    }

    public void Dispose()
    {
        m_Button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        m_OneActionPerformer.PerformAction();
    }
}