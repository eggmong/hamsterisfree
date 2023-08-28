using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DataTable
{
    public class Table_Base : ScriptableObject
    {
        [Serializable]
        public class SerializableTuple<T1, T2>
        {
            public T1 Type;
            [FormerlySerializedAs("Count")] public T2 Value;

            public SerializableTuple(T1 item1, T2 item2)
            {
                Type = item1;
                Value = item2;
            }
        }
    }
}