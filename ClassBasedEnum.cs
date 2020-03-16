namespace ClassBasedEnum
{
    void Main()
    {
        int a =1;
        string a1 = Status.Pending;
        a.Dump();
        a1.Dump();
        a1 = Status.Processing;
        a1.Dump();
        Status c1 = (Status)a;
        Status c2 = (Status)"Cancelled";
        var t= c1 + (Status)a1;

        ((Status)t).Dump();
    }
    
    public class Status : BaseEnum
    {
        public static readonly Status Pending = new Status(1);
        public static readonly Status Processing = new Status(2);
        public static readonly Status Cancelled = new Status(3);
        public static readonly Status Completed = new Status(4);

        private Status(int id, [System.Runtime.CompilerServices.CallerMemberName] string name = "")
            : base(id, name)
        {
        }
        public static explicit operator Status(string value)  
        {
            var result = GetAll<Status>().FirstOrDefault(v => v.Name == value);
            if (result is null)
                throw new Exception($"{value} is undefined on {nameof(Status)}");
            return result;
        }

        public static explicit operator Status(int value)  
        {
            var result = GetAll<Status>().FirstOrDefault(v => v.Id == value);
            if (result is null)
                throw new Exception($"{value} is undefined on {nameof(Status)}");
            return result;
        }
	
        public static implicit operator int(Status value)  
        {
            return value.Id;
        }
        public static implicit operator string(Status value) 
        {
            return value.Name;
        }
        public static Status  operator + (Status value1,Status value2)
        {
            return (Status)(value1.Id + value2.Id);
        }

    }
    
    public partial abstract class BaseEnum : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected BaseEnum(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : BaseEnum
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as BaseEnum;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Id.CompareTo(((BaseEnum)other).Id);

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }
    }
}
