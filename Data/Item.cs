namespace ConsoleApp1.Data
{
    public interface IEntity<T>  where T : struct
    {
        public T Id { get; set; }
    }

    public class Item : IEntity<Guid>
    {
        public string DataId { set; get; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsFavorite { get; set; }
        public Uri Uri { get; set; }
        public string Name { get; set; }
        public int Price { set; get; }
        public bool IsChecked { set; get; }
        public bool IsAbuse { set; get; }
        public bool IsLike { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class Category : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Uri Uri { get; set; }
        public string Name { get; set; }
        public int LastParsePage { get; set; }
        public DateTime LastParse { get; set; }
        public bool IsFinalize { set; get; }
    }

    public class Shop : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
