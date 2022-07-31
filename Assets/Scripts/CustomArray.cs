public class CustomArray<T>
{
    private readonly T[] _container;
    private int _index;

    public int Count => _index;
        
    public CustomArray(int capacity)
    {
        _container = new T[capacity];
    }

    public void Add(T item)
    {
        _container[_index++] = item;
    }

    public T Get(int index)
    {
        return _container[index];
    }

    public void Clear()
    {
        _index = 0;
    }
}