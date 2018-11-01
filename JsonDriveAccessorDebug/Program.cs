using System;
using DriveAccessors;
using System.IO;
using System.Collections.Generic;

namespace JsonDriveAccessorDebug
{
    class Program
    {
        public const string Path = "file.dat";
        public const string AddressStoragePath = "addresses.dat";
        public const string TestPath = "test.dat";

        private static JsonDriveAccessor<Person> dataManager;
        private static Person[] people;

        static void Main(string[] args)
        {
            File.Create(Path).Close();
            File.Create(AddressStoragePath).Close();

            IIndexedStorage<long> addressStorage = new AddressStorage(AddressStoragePath);

            dataManager = new JsonDriveAccessor<Person>(Path, addressStorage);

            people = new Person[]
            {
                new Person("John", "Doh", 37),
                new Person("Rick", "Morgan", 25),
                new Person("Joe", "Dash", 17)
            };

            dataManager.AddRecord(people[0]);
            dataManager.AddRecord(people[1]);
            dataManager.AddRecord(people[2]);

            Person extracted = dataManager.GetNextRecord();

            Console.WriteLine(extracted);
            Console.WriteLine(extracted.Equals(people[0]));
            Console.WriteLine();

            extracted = dataManager.GetNextRecord();

            Console.WriteLine(extracted);
            Console.WriteLine(extracted.Equals(people[1]));
            Console.WriteLine();

            int i = 0;

            foreach (Person person in dataManager)
                Console.WriteLine(person/*.Equals(people[i++])*/);

            Console.WriteLine(dataManager.GetNextRecord().Equals(people[2]));
            Console.WriteLine();

            try
            {
                Person guy = dataManager.GetNextRecord();
            }
            catch (InvalidDataException)
            {
                Console.WriteLine("End of file");
            }

            dataManager.Reset();
            Person man = dataManager.GetNextRecord();

            Console.WriteLine(man);
            Console.WriteLine(man.Equals(people[0]));
            Console.WriteLine();

            List<Person> personList = new List<Person>();

            for (int j = 0; j < 3; j++)
                Console.WriteLine(dataManager[j]);
        }
    }
}
