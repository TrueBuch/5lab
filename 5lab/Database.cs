using System.Collections;
using System.Diagnostics;

public class Database
{
    private readonly Dictionary<Type, IList> _tables;

    public Database()
    {
        _tables = [];
    }

    public static implicit operator bool(Database db)
    {
        return db._tables.Count > 0;
    }

    public void Register<T>(List<T> table) where T : DatabaseEntry
    {
        _tables[typeof(T)] = table;
    }

    public List<T> Table<T>()
    {
        return (List<T>) _tables[typeof(T)];
    }

    public bool Get<T>(int id, out T item) where T : DatabaseEntry
    {
        if (!_tables.ContainsKey(typeof(T)))
        {
            item = default;
            return false;
        }

        item = Table<T>().FirstOrDefault(a => a.ID == id);
        return true;
    }

    public bool Has<T>(int id) where T : DatabaseEntry
    {
        return Table<T>().Any(e => e.ID == id);
    }

    public bool Remove<T>(int id) where T : DatabaseEntry
    {
        if (!_tables.ContainsKey(typeof(T)))
        {
            return false;
        }

        List<T> table = Table<T>();
        
        T item = table.FirstOrDefault(a => a.ID == id);

        return table.Remove(item);
    }

    public void Add<T>(T entry) where T : DatabaseEntry
    {
        if (!_tables.ContainsKey(typeof(T)))
        {
            return;
        }

        entry.ID = GenerateId<T>();
        Table<T>().Add(entry);

        Console.WriteLine($"Запись добавлена: ID = {entry.ID}");
    }   

    public int GenerateId<T>() where T : DatabaseEntry
    {
        var table = Table<T>();

        return table.Count == 0 ? 1 : table.Max(e => e.ID) + 1;
    }
}