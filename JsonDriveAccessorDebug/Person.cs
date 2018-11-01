using System;
using System.Collections.Generic;
using System.Text;

namespace JsonDriveAccessorDebug
{
    [Serializable]
    public class Person
    {
        public string Name { get; }
        public string Surname { get; }
        public int Age { get; }

        public Person(string name, string surname, int age)
        {
            Name = name;
            Surname = surname;
            Age = age;
        }

        public override string ToString() => $"{Name} {Surname}, {Age} y.o.";

        public override bool Equals(object obj)
        {
            if (!(obj is Person other))
                return false;

            return (Name, Surname, Age) == (other.Name, other.Surname, other.Age);
        }

        public override int GetHashCode() => (Name, Surname, Age).GetHashCode();
    }
}
