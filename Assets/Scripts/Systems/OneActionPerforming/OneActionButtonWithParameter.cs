using System;
using UnityEngine.UI;
using Zenject;

public class AllButtonsData<T>
{
    public Button[] Buttons { get; private set; }
    public OneActionPerformerWithParameter<T>[] Performers { get; private set; }

    public AllButtonsData(Button[] buttons, OneActionPerformerWithParameter<T>[] performers)
    {
        Buttons = buttons;
        Performers = performers;
    }
}

public class OneActionButtonsWithParameter<T> : IInitializable, IDisposable
{
    protected AllButtonsData<T> m_ButtonsData { get; private set; }

    public OneActionButtonsWithParameter(AllButtonsData<T> buttonsData)
    {
        m_ButtonsData = buttonsData;
    }

    void IInitializable.Initialize()
    {
        for(int i = 0; i < m_ButtonsData.Buttons.Length; i++)
        {
            int ICopy = i;
            m_ButtonsData.Buttons[i].onClick.AddListener(() => OnButtonClicked(ICopy));
        }
    }

    void IDisposable.Dispose()
    {

    }

    private void OnButtonClicked(int index)
    {
        m_ButtonsData.Performers[index].PerformAction();
    }
}