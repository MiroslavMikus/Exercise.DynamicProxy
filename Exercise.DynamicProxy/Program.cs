using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators;

namespace Exercise.DynamicProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p = Freezable.MakeFreezable<Person>();
            p.FirstName = "Foo";
            p.LastName = "Bar";
            Console.WriteLine(p);
            Freezable.Freeze(p);
            p.FirstName = "what";
            Console.WriteLine(p);
        }
    }


}
