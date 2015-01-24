using System;

namespace AbstractClasses
{
    class Program
    {
        static void Main(string[] args)
        {
            Dog dog = new Dog();
            Console.WriteLine(dog.Describe());
            Console.ReadKey();
        }
    }

    abstract class FourLeggedAnimal
    {
        public virtual string Describe()
        {
            return "This animal has four legs.";
        }
    }
    
    class Dog : FourLeggedAnimal
    {
        public override string Describe()
        {
            string result = base.Describe();
            result += " In fact, it's a dog!";
            return result;
        }
    }
}

