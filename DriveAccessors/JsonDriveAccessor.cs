using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DriveAccessors
{
    class JsonDriveAccessor<T> : IDriveAccessor<T> where T : class
    {
        private Stream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private bool disposed;
        private IIndexedStorage<long> addressStorage;

        public JsonDriveAccessor(string path, IIndexedStorage<long> storage)
        {
            disposed = false;

            if (!File.Exists(path))
                throw new ArgumentException($"File \"{path}\" doesn't exist.");

            stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            addressStorage = storage;

            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
        }

        public T this[int index] => throw new NotImplementedException();

        public void AddRecord(T instance)
        {
            long currentPosition = stream.Position;
            long lastRecordAddress = stream.Seek(0, SeekOrigin.End);
            string jsonData = JsonConvert.SerializeObject(instance);

            writer.WriteLine(jsonData);
            writer.Flush();

            stream.Position = currentPosition;

            addressStorage.Add(lastRecordAddress);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public T GetNextRecord()
        {
            T nextRecord = null;

            try
            {
                string jsonData = reader.ReadLine();

                nextRecord = JsonConvert.DeserializeObject<T>(jsonData);
            }
            catch (SerializationException)
            {
                throw new InvalidDataException("Couldn't retrieve next record");
            }

            return nextRecord;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
