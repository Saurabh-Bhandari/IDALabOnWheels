﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmNet;

namespace IDALabOnWheels
{
    class FixedSizeConcurrentQueue<T> : ConcurrentQueue<T>
    {

        private readonly object syncObject = new object();

        public int Size { get; private set; }

        public FixedSizeConcurrentQueue(int size)
        {
            Size = size;
        }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            lock (syncObject)
            {
                while (base.Count > Size)
                {
                    T outObj;
                    base.TryDequeue(out outObj);
                }
            }
        }
    }


    public static class ExtensionMethods
    {
        // Syntax for extending generic classes --> http://stackoverflow.com/questions/3749386/how-to-define-extension-methods-for-generic-class
        public static T[] GetAvailableData<T>(this BlockingCollection<T> collection)
        {
            if (collection.Count == 0)
                return (null);

            int items = collection.Count;
            T[] data = new T[items];
            for (int i = 0; i < items; i++)
            {
                data[i] = collection.Take(); // will block if it's not available
            }
            return (data);
        }

    }

    public static class myGLM
    {
        public static mat4 transpose(mat4 ip)
        {
            mat4 op = mat4.identity();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    op[j,i] = ip[i,j];
                }
            }
        return (op);
        }

    }

}
