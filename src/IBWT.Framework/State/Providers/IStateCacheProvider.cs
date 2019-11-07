namespace IBWT.Framework.State.Providers
{
    public interface IStateProvider
    {
        StateContext GetState(int id);
        bool Clear(int id);
        void ClearAll();
    }
}