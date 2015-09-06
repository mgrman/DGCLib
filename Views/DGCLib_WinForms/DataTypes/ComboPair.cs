namespace DGCLib_WinForms.DataTypes
{
    internal class ComboPair<T>
    {
        public string Name { get; private set; }

        public T Value { get; private set; }

        public ComboPair(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}