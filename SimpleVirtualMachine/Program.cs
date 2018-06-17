using System;
using SimpleVirtualMachine.Models;

namespace SimpleVirtualMachine
{
    class MainClass
    {
        public static int Main(string[] args)
        {
            //if(args.GetLength(0) != 1)
            //{
            //    Console.WriteLine("usage: SimpleVirtualMachine <filename>");
            //    return 1;
            //}

            //#TODO: if that check bool returned form ReadVMFile() method

            Menu menu = new Menu();

            //VirtualMachine VM = new VirtualMachine(args[0]);
            //VirtualMachine VM = new VirtualMachine("/Users/kisiel/Programowanie/Projekty/Cpp/MaszynaWirtualnaProgramy/Ja/max_3.bin");
            //VirtualMachine VM = new VirtualMachine("/Users/kisiel/Programowanie/Projekty/Cpp/MaszynaWirtualnaProgramy/Ja/dod_ab.bin");

            return 0;
        }
    }
}
