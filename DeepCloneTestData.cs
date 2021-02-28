using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NetCorePerformanceTest.DeepClone
{
    [Serializable] 
    public class DerivedDictionary<T1, T2> : Dictionary<T1, T2>
    {
        public DerivedDictionary()
        { }

        public DerivedDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
    [Serializable]
    public class Struct
    {
        private int Item1;

        public SimpleClass Item23 { get; set; }

        public SimpleClass Item32 { get; set; }

        public SimpleClass Item4 { get; }

        public Struct(int item1, SimpleClass item23, SimpleClass item4)
        {
            Item1 = item1;
            Item23 = item23;
            Item32 = item23;
            Item4 = item4;
        }

        public int GetItem1()
        {
            return Item1;
        }

        public void IncrementItem1()
        {
            Item1++;
        }

        public void DecrementItem1()
        {
            Item1--;
        }
    }

    [Serializable]
    public struct DeeperStruct
    {
        [Serializable]
        private struct SubStruct
        {
            public int Item1;

            public SimpleClass Item2;
        }

        private SubStruct SubStructItem;

        public DeeperStruct(int item1, SimpleClass item2)
        {
            SubStructItem = new SubStruct() { Item1 = item1, Item2 = item2 };
        }

        public int GetItem1()
        {
            return SubStructItem.Item1;
        }

        public void IncrementItem1()
        {
            SubStructItem.Item1++;
        }

        public void DecrementItem1()
        {
            SubStructItem.Item1--;
        }

        public SimpleClass GetItem2()
        {
            return SubStructItem.Item2;
        }
    }
    [Serializable]
    public class SimpleClass
    {
        public string PropertyPublic { get; set; }

        protected bool PropertyProtected { get; set; }

        private int PropertyPrivate { get; set; }

        public int FieldPublic;

        private string FieldPrivate;

        public readonly string ReadOnlyField;
        public SimpleClass() { }
        public SimpleClass(int propertyPrivate, bool propertyProtected, string fieldPrivate)
        {
            PropertyPrivate = propertyPrivate;
            PropertyProtected = propertyProtected;
            FieldPrivate = fieldPrivate + "_" + typeof(SimpleClass);
            ReadOnlyField = FieldPrivate + "_readonly";
        }

        public static SimpleClass CreateForTests(int seed)
        {
            return new SimpleClass(seed, seed % 2 == 1, "seed_" + seed)
            {
                FieldPublic = -seed,
                PropertyPublic = "seed_" + seed + "_public"
            };
        }

        public int GetPrivateProperty()
        {
            return PropertyPrivate;
        }

        public string GetPrivateField()
        {
            return FieldPrivate;
        }
    }

    [Serializable]
    public class ModerateClass : SimpleClass
    {
        public string PropertyPublic2 { get; set; }

        protected bool PropertyProtected2 { get; set; }

        public int FieldPublic2 { get; set; }

        private int PropertyPrivate { get; set; }

        private string FieldPrivate { get; set; }

        //public Struct StructField;

        public DeeperStruct DeeperStructField;

        public GenericClass<SimpleClass> GenericClassField;

        public SimpleClass SimpleClassProperty { get; set; }

        public SimpleClass ReadonlySimpleClassField;

        public SimpleClass[] SimpleClassArray { get; set; }

        public Object ObjectTextProperty { get; set; }
        public ModerateClass()
        {

        }
        public ModerateClass(int propertyPrivate, bool propertyProtected, string fieldPrivate)
            : base(propertyPrivate, propertyProtected, fieldPrivate)
        {
            PropertyPrivate = propertyPrivate + 1;
            FieldPrivate = fieldPrivate + "_" + typeof(ModerateClass);
            ObjectTextProperty = "Test";
        }

        public static ModerateClass CreateForTests(int seed)
        {
            var moderateClass = new ModerateClass(seed, seed % 2 == 1, "seed_" + seed);

            moderateClass.FieldPublic = seed;
            moderateClass.FieldPublic2 = seed + 1;

            //moderateClass.StructField = new Struct(seed, moderateClass, SimpleClass.CreateForTests(seed));
            moderateClass.DeeperStructField = new DeeperStruct(seed, SimpleClass.CreateForTests(seed));

            moderateClass.GenericClassField = new GenericClass<SimpleClass>(moderateClass, SimpleClass.CreateForTests(seed));

            var seedSimple = seed + 1000;

            moderateClass.SimpleClassProperty = new SimpleClass(seedSimple, seed % 2 == 1, "seed_" + seedSimple);

            moderateClass.ReadonlySimpleClassField = new SimpleClass(seedSimple + 1, seed % 2 == 1, "seed_" + (seedSimple + 1));

            moderateClass.SimpleClassArray = new SimpleClass[10];

            for (int i = 1; i <= 10; i++)
            {
                moderateClass.SimpleClassArray[i - 1] = new SimpleClass(seedSimple + i, seed % 2 == 1, "seed_" + (seedSimple + i));
            }

            return moderateClass;
        }

        public int GetPrivateProperty2()
        {
            return PropertyPrivate;
        }

        public string GetPrivateField2()
        {
            return FieldPrivate;
        }
    }

    [Serializable]
    public class GenericClass<T>
    {
        public T Item1;

        public T Item2 { get; }

        public GenericClass(T item1, T item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    [Serializable]
    public class ComplexClass : ModerateClass
    {
        //public ComplexClass ThisComplexClass { get; set; }

        //public Tuple<ComplexClass, ModerateClass, SimpleClass> TupleOfThis { get; protected set; }

        public Dictionary<string, SimpleClass> SampleDictionary = new DerivedDictionary<string, SimpleClass>();

        public DerivedDictionary<string, SimpleClass> ISampleDictionary { get; private set; } = new DerivedDictionary<string, SimpleClass>();

        public SimpleClass[,,] ISimpleMultiDimArray;

        public SimpleClass[][][] SimpleMultiDimArray;

        //public Struct[] StructArray;



        //public bool IsJustEventNull { get { return JustEvent == null; } }

        public ComplexClass()
            : base(propertyPrivate: -1, propertyProtected: true, fieldPrivate: "fieldPrivate_" + typeof(ComplexClass))
        {

            //JustEvent += new DelegateType(() => CreateForTests());
        }


        public static ComplexClass CreateForTests()
        {
            var complexClass = new ComplexClass();

            var dict1 = new DerivedDictionary<string, SimpleClass>();
            complexClass.SampleDictionary = dict1;

            dict1[typeof(ComplexClass).ToString()] = new ComplexClass();
            dict1[typeof(ModerateClass).ToString()] = new ModerateClass(1, true, "madeInComplexClass");
            dict1[typeof(SimpleClass).ToString()] = new SimpleClass(2, false, "madeInComplexClass");

            var dict2 = complexClass.ISampleDictionary;

            dict2[typeof(ComplexClass).ToString()] = dict1[typeof(ComplexClass).ToString()];
            dict2[typeof(ModerateClass).ToString()] = dict1[typeof(ModerateClass).ToString()];
            dict2[typeof(SimpleClass).ToString()] = new SimpleClass(2, false, "madeInComplexClass");

            var array1 = new SimpleClass[2, 1, 1];
            array1[0, 0, 0] = new SimpleClass(4, false, "madeForMultiDimArray");
            array1[1, 0, 0] = new ComplexClass();
            complexClass.ISimpleMultiDimArray = array1;

            var array2 = new SimpleClass[2][][];
            array2[1] = new SimpleClass[2][];
            array2[1][1] = new SimpleClass[2];
            array2[1][1][1] = (SimpleClass)array1[0, 0, 0];
            complexClass.SimpleMultiDimArray = array2;

            //complexClass.StructArray = new Struct[2];
            //complexClass.StructArray[0] = new Struct(1, complexClass, SimpleClass.CreateForTests(5));
            //complexClass.StructArray[1] = new Struct(3, new SimpleClass(3, false, "inStruct"), SimpleClass.CreateForTests(6));

            return complexClass;
        }
    }
}
