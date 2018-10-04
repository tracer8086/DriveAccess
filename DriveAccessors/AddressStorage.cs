using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

namespace DriveAccessors
{
    class AddressStorage : IIndexedStorage<int>
    {
        private Stream stream;
        private BinaryFormatter serializer;
        private int? lastIndex;

        public int this[int index]
        {
            get
            {
                if (lastIndex == null || index > lastIndex)
                    throw new IndexOutOfRangeException($"There are no elements stored by index {index}");

                if (index < 0)
                    throw new IndexOutOfRangeException($"Index value must be non-negative");

                long currentPosition = stream.Position;
                int result;

                stream.Seek(0, SeekOrigin.Begin);

                do
                {
                    result = (int)serializer.Deserialize(stream);
                    index--;
                }
                while (index >= 0);

                stream.Position = currentPosition;

                return result;
            }
        }

        public AddressStorage(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException($"File \"{path}\" doesn't exist.");

            stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            serializer = new BinaryFormatter();
        }

        public void Add(int item)
        {
            serializer.Serialize(stream, item);

            lastIndex = lastIndex == null ? 0 : lastIndex + 1;
        }

        public IEnumerator<int> GetEnumerator()
        {
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
