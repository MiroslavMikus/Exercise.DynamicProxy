namespace Exercise.DynamicProxy
{
    public class Person
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public override string ToString() => $"{FirstName} {LastName}";
    }
}
