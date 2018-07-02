using System;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators;

namespace Exercise.DynamicProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Person per = Freezable.MakeFreezable<Person>();
            per.FirstName = "Foo";
            per.LastName = "Bar";

            Console.WriteLine(per);

            Freezable.Freeze(per);

            // since this person is 'Frozen' this will fail
            per.FirstName = "what";

            Console.WriteLine(per);

            Console.ReadLine();
        }
    }
}
