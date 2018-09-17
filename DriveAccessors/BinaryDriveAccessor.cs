using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DriveAccessors
{
    public class BinaryDriveAccessor<T> : IEnumerable<T>, IDisposable where T : class
    {
        private Stream stream;
        private BinaryFormatter serializer;
        private bool disposed;

        public BinaryDriveAccessor(string path)
        {
            disposed = false;

            if (!File.Exists(path))
                throw new ArgumentException($"File \"{path}\" doesn't exist.");

            stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            serializer = new BinaryFormatter();
        }

        public T GetNextRecord()
        {
            T nextRecord;

            try
            {
                nextRecord = (T)serializer.Deserialize(stream);
            }
            catch (SerializationException)
            {
                nextRecord = null;
            }

            return nextRecord;
        }

        public void AddRecord(T instance)
        {
            long currentPosition = stream.Position;

            stream.Seek(0, SeekOrigin.End);

            serializer.Serialize(stream, instance);

            stream.Position = currentPosition;
        }

        public void Reset() => stream.Seek(0, SeekOrigin.Begin);

        public IEnumerator<T> GetEnumerator()
        {
            long enumPosition = 0;
            long currentPosition = stream.Position;

            while (true)
            {
                stream.Position = enumPosition;

                T nextRecord = GetNextRecord();

                if (nextRecord == null)
                {
                    stream.Position = currentPosition;
                    break;
                }

                enumPosition = stream.Position;
                stream.Position = currentPosition;

                yield return nextRecord;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    stream.Close();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
