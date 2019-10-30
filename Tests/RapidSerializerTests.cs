using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cpGames.RapidSerializer.Tests
{
    [TestClass]
    public class RapidSerializerTests
    {
        #region Methods
        private static bool ListsEqual<T>(List<T> a, List<T> b, Func<T, T, bool> compareFunc)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null)
            {
                return false;
            }
            if (a.Count != b.Count)
            {
                return false;
            }
            return !a.Where((t, i) => !compareFunc(t, b[i])).Any();
        }

        [TestMethod]
        public void ToFile_Test()
        {
            var filename = "test.txt";
            var t = new TestClassA();
            t.SetValues();
            RapidSerializer.SerializeToFile(t, filename);
            var tRes = RapidSerializer.DeserializeFromFile<TestClassA>(filename);
        }

        [TestMethod]
        public void DictionaryTest1()
        {
            var t = new TestClassA();
            t.SetValues();

            var serializedData_noMask = RapidSerializer.Serialize(t);
            var t_NoMask = RapidSerializer.Deserialize<TestClassA>(serializedData_noMask);
            Assert.IsNotNull(t_NoMask);
            Assert.IsTrue(t.A == t_NoMask.A);
            Assert.IsTrue(t.B == t_NoMask.B);
            Assert.IsTrue(t.C == t_NoMask.C);
            Assert.IsTrue(t.D == t_NoMask.D);
            Assert.IsTrue(t.Guid.Equals(t_NoMask.Guid));
            Assert.IsTrue(ListsEqual(t.ListOfStrings, t_NoMask.ListOfStrings, string.Equals));

            var serializedData_Mask_1 = RapidSerializer.Serialize(t, SerializationMaskType.Public);
            var t_Mask_1 = RapidSerializer.Deserialize<TestClassA>(serializedData_Mask_1);

            var serializedData_Mask_2 = RapidSerializer.Serialize(t, SerializationMaskType.Private);
            var t_Mask_2 = RapidSerializer.Deserialize<TestClassA>(serializedData_Mask_2);

            var serializedData_Mask_1_2 = RapidSerializer.Serialize(t, SerializationMaskType.Public | SerializationMaskType.Private);
            var t_Mask_1_2 = RapidSerializer.Deserialize<TestClassA>(serializedData_Mask_1_2);
        }

        [TestMethod]
        public void DictionaryTest2()
        {
            var a = new DerivedA();
            a.SetValues();

            var serializedData = RapidSerializer.Serialize(a);
            var a1 = RapidSerializer.Deserialize<Interface>(serializedData);
        }

        [TestMethod]
        public void DictionaryTest3()
        {
            var a = new TestStruct { a = 3, b = "testString" };
            var serializedData = RapidSerializer.Serialize(a);
            var a_deserialized = RapidSerializer.Deserialize<TestStruct>(serializedData);
        }

        [TestMethod]
        public void ListTest()
        {
            //var lIn = new ListClass();
            var lIn = new ListContainerClass { list = new List<string>() };
            lIn.list.Add("Hello");
            lIn.list.Add("My name is");
            lIn.list.Add("Bob!");
            var serializedData = RapidSerializer.Serialize(lIn);
            RapidSerializer.DeserializeList<ListClass>(new object[] { serializedData });
            //var lOut = RapidSerializer.Deserialize<ListClass>(serializedData);
            var lOut = RapidSerializer.Deserialize<ListContainerClass>(serializedData);
        }

        [TestMethod]
        public void DictionaryDictionaryTest()
        {
            var a = new DictionaryClass();
            a.SetValues();

            var serializedData = RapidSerializer.Serialize(a);
            var a1 = RapidSerializer.Deserialize<DictionaryClass>(serializedData);
        }

        [TestMethod]
        public void ReadonlyListTest()
        {
            //var lIn = new ListClass();
            var lIn = new ListContainerClassReadonly();
            lIn.list.Add("Hello");
            lIn.list.Add("My name is");
            lIn.list.Add("Bob!");
            var serializedData = RapidSerializer.Serialize(lIn);
            var lOut = RapidSerializer.Deserialize<ListContainerClassReadonly>(serializedData);
        }

        [TestMethod]
        public void InterfaceTest()
        {
            var interfaceList = new List<Interface>();
            interfaceList.Add(new DerivedA { a = "a1" });
            interfaceList.Add(new DerivedA { a = "a2" });
            interfaceList.Add(new DerivedB { a = "b1" });
            var serializedData = RapidSerializer.Serialize(interfaceList);
            var interfaceListOut = RapidSerializer.Deserialize<List<Interface>>(serializedData);
            Assert.AreEqual(interfaceList.Count, interfaceListOut.Count);
        }
        #endregion
    }
}