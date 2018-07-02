using System;
using System.Linq;
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

            // by the current implementation the object have to be registered before it can be recognized as a freezable!
            Assert.IsFalse(Freezable.IsFreezable(per));

            Assert.IsTrue(Freezable.RegisterFreezable(per));

            Assert.IsTrue(Freezable.IsFreezable(per));
        }

        [TestMethod]
        public void NotFreezablePersonRegister_Test_Fail()
        {
            Person per = new ProxyGenerator().CreateClassProxy<Person>();
            per.FirstName = "Foo";
            per.LastName = "Bar";

            // by the current implementation the object have to be registered before it can be recognized as a freezable!
            Assert.IsFalse(Freezable.IsFreezable(per));

            Assert.IsFalse(Freezable.RegisterFreezable(per));

            Assert.IsFalse(Freezable.IsFreezable(per));
        }

        [TestMethod]
        public void AddInterceptorAtRuntime_Test()
        {
            Person per = new ProxyGenerator().CreateClassProxy<Person>();
            per.FirstName = "Foo";
            per.LastName = "Bar";

            // by the current implementation the object have to be registered before it can be recognized as a freezable!
            Assert.IsFalse(Freezable.IsFreezable(per));

            Assert.IsFalse(Freezable.RegisterFreezable(per));

            ProxyHelper.AddInterceptor<FreezableInterceptor>(per);

            Assert.IsTrue(Freezable.RegisterFreezable(per));

            Assert.IsTrue(Freezable.IsFreezable(per));
        }

        [TestMethod]
        public void RemoveInterceptorAtRuntime_Test()
        {
            Person per = new ProxyGenerator().CreateClassProxy<Person>(new FreezableInterceptor());
            per.FirstName = "Foo";
            per.LastName = "Bar";

            Assert.IsTrue(Freezable.RegisterFreezable(per));

            Assert.IsTrue(Freezable.IsFreezable(per));

            Assert.AreEqual(1, ProxyHelper.GetInterceptorsField(per).Count());

            ProxyHelper.ExcudeInterceptors(per, typeof(FreezableInterceptor));

            Assert.AreEqual(0, ProxyHelper.GetInterceptorsField(per).Count());
        }
    }
}
