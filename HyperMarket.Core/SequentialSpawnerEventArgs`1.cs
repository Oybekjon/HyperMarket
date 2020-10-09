using System;
namespace HyperMarket
{
    public class SequentialSpawnerEventArgs<T> : EventArgs
    {
        public Int32 Count { get; private set; }
        public T Object { get; private set; }
        public SequentialSpawnerEventArgs(Int32 count, T @object)
        {
            Count = count;
            Object = @object;
        }
    }
}