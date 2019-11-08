namespace IBWT.Framework.State.Providers
{
    public interface IStateProvider
    {
        StateContext GetState(long id);
        bool Clear(long id);
        void ClearAll();
    }
}