using System;

namespace SHA_256
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please input your PS VITA AID Key value : ");

            string inputAID = Console.ReadLine();

            string returnString = VitaAIDConvert.To64Digits(inputAID);
            
            Console.WriteLine(returnString);
        }
    }
}
