public interface IAction<TOwner,TTarget>
{
    public abstract void DoAction(TOwner owner, TTarget target);
}

public interface IAction<TOwner>
{
    public abstract void DoAction(TOwner owner);
}
