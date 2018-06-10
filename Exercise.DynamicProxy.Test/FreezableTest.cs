using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exercise.DynamicProxy.Test
{
    [TestClass]
    public class FreezableTest
    {
        [TestMethod]
        public void FreezablePerson_Test()
        {
            Person per = Freezable.MakeFreezable<Person>();
            per.FirstName = "Foo";
            per.LastName = "Bar";

            Assert.IsTrue(Freezable.IsFreezable(per));
        }

        [TestMethod]
        public void NotFreezablePerson_Test()
        {
            Person per = new Person
            {
                FirstName = "Foo",
                LastName = "Bar"
            };

            Assert.IsFalse(Freezable.IsFreezable(per));
        }

        [TestMethod]
        public void NotFreezablePersonRegister_Test()
        {
            Person per = new ProxyGenerator().CreateClassProxy<Person>(new FreezableInterceptor());
            per.FirstName = "Foo";
            per.LastName = "Bar";

            // by the current implementation the object have to be registered befoce it can be recognized as a freezable!
            Assert.IsFalse(Freezable.IsFreezable(per));

            Freezable.RegisterFreezable(per);

            Assert.IsTrue(Freezable.IsFreezable(per));
        }
    }
}
