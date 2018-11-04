using System;
using DriveAccessors;
using System.IO;

namespace AddressStorageDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            File.Create("addresses.dat").Close();

            AddressStorage addressStorage = new AddressStorage("addresses.dat");

            addressStorage.Add(13);
            addressStorage.Add(32);
            addressStorage.Add(6);

            Console.WriteLine($"Addres #2: {addressStorage[1]}");
            Console.WriteLine();

            foreach (long address in addressStorage)
                Console.WriteLine($"Addres: {address}");
        }
    }
}
