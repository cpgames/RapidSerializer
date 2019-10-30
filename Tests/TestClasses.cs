using System;
using System.Collections.Generic;

namespace cpGames.RapidSerializer.Tests
{
    public class TestClassA
    {
        #region Fields
        private string _a;
        protected string _b;
        public string c;
        public int d;
        public Guid guid;
        public List<string> listOfStrings;
        public List<float> listOfFloats;
        public List<object> listOfObjects;
        public float[] arrayOfFloats;
        public string[] arrayOfStrings;
        public TestClassB nestedClass;
        public List<AbstractClass> abstractClasses;
        public List<Interface> interfaces;
        public List<List<int>> listOfLists;
        [CpSerializationIgnore] public string ignoreMe;
        [SerializationMask(SerializationMaskType.Public)]
        public string onlyForPrivileged;
        [SerializationMask(SerializationMaskType.Private)]
        public string onlyForSuperPrivileged;
        [SerializationMask(SerializationMaskType.Public | SerializationMaskType.Private)]
        public string forBoth;
        #endregion

        #region Properties
        public string A => _a;
        public string B => _b;
        public string C => c;
        public int D => d;
        public Guid Guid => guid;
        public List<string> ListOfStrings => listOfStrings;
        public List<float> ListOfFloats => listOfFloats;
        public List<object> ListOfObjects => listOfObjects;
        public float[] ArrayOfFloats => arrayOfFloats;
        public string[] ArrayOfStrings => arrayOfStrings;
        public List<List<int>> ListOfLists => listOfLists;
        public string IgnoreMe => ignoreMe;
        public string OnlyForPrivileged => onlyForPrivileged;
        public string OnlyForSuperPrivileged => onlyForSuperPrivileged;
        public string ForBoth => forBoth;
        #endregion

        #region Methods
        public void SetValues()
        {
            guid = Guid.NewGuid();
            _a = "private string";
            c = "this is nestedClass string";
            _b = "this is nestedClass protected string";
            d = 3;
            listOfStrings = new List<string> { "aaa", "bbb", "ccc" };
            listOfFloats = new List<float> { 0.1f, 0.2f, 32.2f };
            listOfObjects = new List<object> { 1, "two", 3f, 4L };
            arrayOfFloats = new[] { 1, 2, 3.5f };
            arrayOfStrings = new[] { "abc", "def" };

            nestedClass = new TestClassB();
            nestedClass.SetValues();

            abstractClasses = new List<AbstractClass>();
            var da = new DerivedA();
            da.SetValues();
            abstractClasses.Add(da);
            var db = new DerivedB();
            db.SetValues();
            abstractClasses.Add(db);

            interfaces = new List<Interface>();
            var ia = new DerivedA();
            ia.SetValues();
            interfaces.Add(ia);
            var ib = new DerivedB();
            ib.SetValues();
            interfaces.Add(ib);

            listOfLists = new List<List<int>>
            {
                new List<int> { 1, 2, 3 },
                new List<int> { 5, 6, 7 }
            };

            ignoreMe = "this should be ignored";
            onlyForPrivileged = "can you read that?";
            onlyForSuperPrivileged = "how about that?";
            forBoth = "or that?";
        }
        #endregion
    }

    public class TestClassB
    {
        #region Fields
        public string a;
        public TestClassC nestedClass;
        #endregion

        #region Properties
        public string A => a;
        public TestClassC NestedClass => nestedClass;
        #endregion

        #region Methods
        public void SetValues()
        {
            a = "I am a nestedClass!";
            nestedClass = new TestClassC();
            nestedClass.SetValues();
        }
        #endregion
    }

    public class TestClassC
    {
        #region Fields
        public string a;
        #endregion

        #region Properties
        public string A => a;
        #endregion

        #region Methods
        public void SetValues()
        {
            a = "I am even more nested!";
        }
        #endregion
    }

    public abstract class AbstractClass { }

    public interface Interface { }

    public class DerivedA : AbstractClass, Interface
    {
        #region Fields
        public string a;
        #endregion

        #region Properties
        public string A => a;
        #endregion

        #region Methods
        public void SetValues()
        {
            a = "I am derived A";
        }
        #endregion
    }

    public class DerivedB : AbstractClass, Interface
    {
        #region Fields
        public string a;
        #endregion

        #region Properties
        public string A => a;
        #endregion

        #region Methods
        public void SetValues()
        {
            a = "I am derived B";
        }
        #endregion
    }

    public class DictionaryClass
    {
        #region Fields
        public Dictionary<string, float> dict;
        public Dictionary<string, object> dict2;
        #endregion

        #region Methods
        public void SetValues()
        {
            dict = new Dictionary<string, float>
            {
                { "nestedClass", 1 },
                { "nestedClass", 2 },
                { "d", 3 }
            };

            dict2 = new Dictionary<string, object>
            {
                { "nestedClass", 1 },
                { "nestedClass", "two" },
                { "d", new TestStruct { a = 5.3f, b = "three" } }
            };
        }
        #endregion
    }

    public struct TestStruct
    {
        public float a;
        public string b;
    }

    public class ListClass : List<string>
    {
        #region Constructors
        public ListClass() { }
        public ListClass(int capacity) : base(capacity) { }
        #endregion
    }

    public class ListContainerClass
    {
        #region Fields
        public List<string> list;
        #endregion
    }

    public class ListContainerClassReadonly
    {
        #region Fields
        public const string CONSTSTR = "ABC";
        public readonly List<string> list = new List<string>();
        public readonly string str = "abs";
        #endregion
    }
}