namespace Exercise.DynamicProxy
{
    public interface IFreezable
    {
        bool IsFrozen { get; }
        void Freeze();
    }


}
