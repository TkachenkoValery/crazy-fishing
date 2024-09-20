public abstract class OneActionPerformerWithParameter<T>
{
    public T Parameter { get; protected set; }

    public abstract void PerformAction();

    public OneActionPerformerWithParameter(T parameter)
    {
        Parameter = parameter;
    }
}