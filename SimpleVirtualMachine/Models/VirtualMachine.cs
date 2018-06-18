using System;
using System.IO;
using System.Collections.Generic;

namespace SimpleVirtualMachine.Models
{
    public class VirtualMachine
    {
        public enum Flag { Negative = -1, Zero = 0, Positive = 1 };
        public enum Opcode { ADD = 0, SUB = 1, MUL = 2, DIV = 3, CMP = 4, CPY = 5, JMP = 6, LOD = 7, INP = 8, OUT = 9, END = 10, SAV = 11 }; // SAV isn't a opcode of VM, it's using for simplify editor
        public enum RunMode { Normal = 0, StepByStep = 1 };
        public enum ErrorCode { DividedByZero = 0, EndOfFile, OpenFile };

        public static readonly string[] OperationText = { "dodaj", "odejmij", "mnoz", "dziel", "porownaj", "kopiuj", "skocz", "wczytaj", "pobierz", "wyswietl", "zakoncz", "zapisz" };

        ROM Rom { get => _rom; set => _rom = value; }
        ROM _rom;

        RAM Ram { get => _ram; set => _ram = value; }
        RAM _ram;

        class RAM
        {
            public int[] ValueRegister { get => _valueRegister; set => _valueRegister = value; }
            public int FlagRegister { get => _flagRegister; set => _flagRegister = value; }
            public int InstructionRegister { get => _instructionRegister; set => _instructionRegister = value; }

            int[] _valueRegister = new int[64];
            int _flagRegister;
            int _instructionRegister;

            void Clear()
            {
                for (int i = 0; i < 64; i++)
                    ValueRegister[i] = 0;

                FlagRegister = (int)Flag.Zero;
                InstructionRegister = 0;
            }
        }

        class ROM
        {            
            public List<Instruction> InstructionList { get => _instructionList; set => _instructionList = value; }
            public List<int> ConstList { get => _constList; set => _constList = value; }
            public List<int> JumpList { get => _jumpList; set => _jumpList = value; }

            List<Instruction> _instructionList;
            List<int> _constList;
            List<int> _jumpList;

            public ROM()
            {
                InstructionList = new List<Instruction>();
                ConstList = new List<int>();
                JumpList = new List<int>();
            }
        }

        public class Constant
        {
            public byte[] Bytes { get => _bytes; set => _bytes = value; }

            byte[] _bytes = new byte[4];

            public Constant() 
            {
            }

            public Constant(int integer)
            {
                Bytes = BitConverter.GetBytes(integer);
            }

            public int ToInt32()
            {
                return BitConverter.ToInt32(Bytes, 0);
            }
        }

        public class Operand
        {
            public byte[] Bytes { get => _bytes; set => _bytes = value; }

            byte[] _bytes = new byte[2];

            public Operand() 
            {
            }

            public Operand(Instruction instruction)
            {
                Bytes = BitConverter.GetBytes((instruction.Op) | (instruction.R1 << 4) | (instruction.R2 << 10));
            }

            public int ToInt32()
            {
                return BitConverter.ToInt32(Bytes, 0);
            }
        }

        public class Instruction
        {
            public int Op { get => _op; set => _op = value; }
            public int R1 { get => _r1; set => _r1 = value; }
            public int R2 { get => _r2; set => _r2 = value; }

            int _op;
            int _r1;
            int _r2;

            public Instruction()
            {
                Op = 0;
                R1 = 0;
                R2 = 0;
            }

            public Instruction(int opcode)
            {
                Op = opcode;
                R1 = 0;
                R2 = 0;
            }

            public Instruction(Operand operand)
            {
                short operation = BitConverter.ToInt16(operand.Bytes, 0);

                Op = (operation & 0b1111);
                R1 = ((operation & 0b1111110000) >> 4);
                R2 = ((operation & 0b1111110000000000) >> 10);
            }
        }

        public VirtualMachine()
        {
            Rom = new ROM();
            Ram = new RAM();
        }

        public VirtualMachine(bool debug = false)
        {
            Rom = new ROM();
            Ram = new RAM();
            ReadVMFile(InputPath());
            Run(debug);
        }

        bool Run(bool debug)
        {
            while (!(Rom.InstructionList[Ram.InstructionRegister].Op == (int)Opcode.END))
            {
                Execute(Rom.InstructionList[Ram.InstructionRegister]);
                if (debug)
                {
                    Console.WriteLine("Rom.InstructionList.count: " + Rom.InstructionList.Count);
                    Console.WriteLine("Ram.InstructionRegister: " + Ram.InstructionRegister);
                }
            }
            return true;
        }

        bool ReadVMFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Error: file do not exist!");
                return false;
            }

            using (FileStream fs = File.OpenRead(path))
            {
                int bytesRead = 0;
                for (int i = 0; i < fs.Length; i += bytesRead)
                {
                    Operand nextOperand = new Operand();
                    Constant nextConstant = new Constant();

                    bytesRead = fs.Read(nextOperand.Bytes, 0, 2);
                    if (bytesRead == 0)
                    {
                        Console.WriteLine("Warning: EOF while reading nextOperand");
                        break;
                    }

                    Instruction nextInstruction = new Instruction(nextOperand);
                    //Console.WriteLine($"i-byte: {i} byte-value: {nextOperand[0]:X2}{nextOperand[1]:X2} R2: {nextInstruction.R2} R1: {nextInstruction.R1} Op: {nextInstruction.Op} ({OperationText[nextInstruction.Op]})");

                    if (nextInstruction.Op == (int)Opcode.JMP)
                    {
                        bytesRead += fs.Read(nextConstant.Bytes, 0, 4);
                        if (bytesRead == 0)
                            Console.WriteLine("Warning: EOF while reading nextInt(jump)");
                        else
                        {
                            nextInstruction.R2 = Rom.JumpList.Count;
                            Rom.JumpList.Add(BitConverter.ToInt32(nextConstant.Bytes, 0) - 1);
                            //Console.WriteLine($"i-byte: {i} const-value: {nextInt[0]:X2}{nextInt[1]:X2}{nextInt[2]:X2}{nextInt[3]:X2} JUMP-VALUE: {Rom.JumpList[nextInstruction.R2]}");
                        }
                    }
                    else if (nextInstruction.Op == (int)Opcode.LOD)
                    {
                        bytesRead += fs.Read(nextConstant.Bytes, 0, 4);
                        if (bytesRead == 0)
                            Console.WriteLine("Warning: EOF reading nextInt(load)");
                        else
                        {
                            nextInstruction.R2 = Rom.ConstList.Count;
                            Rom.ConstList.Add(BitConverter.ToInt32(nextConstant.Bytes, 0));
                        }
                    }

                    Rom.InstructionList.Add(nextInstruction);
                }
            }
            return true;
        }

        void SetFlag(Instruction instruction)
        {
            if (Ram.ValueRegister[instruction.R1] > 0)
                Ram.FlagRegister = (int)Flag.Positive;
            else if (Ram.ValueRegister[instruction.R1] < 0)
                Ram.FlagRegister = (int)Flag.Negative;
            else
                Ram.FlagRegister = (int)Flag.Zero;
        }

        int Execute(Instruction instruction)
        {
            switch (instruction.Op)
            {
                case (int)Opcode.ADD:
                    {
                        Ram.ValueRegister[instruction.R1] += Ram.ValueRegister[instruction.R2];
                        SetFlag(instruction);
                        break;
                    }
                case (int)Opcode.SUB:
                    {
                        Ram.ValueRegister[instruction.R1] -= Ram.ValueRegister[instruction.R2];
                        SetFlag(instruction);
                        break;
                    }
                case (int)Opcode.MUL:
                    {
                        Ram.ValueRegister[instruction.R1] *= Ram.ValueRegister[instruction.R2];
                        SetFlag(instruction);
                        break;
                    }
                case (int)Opcode.DIV:
                    {
                        int temp = Ram.ValueRegister[instruction.R1];

                        if (Ram.ValueRegister[instruction.R2] != 0)
                        {
                            Ram.ValueRegister[instruction.R1] /= Ram.ValueRegister[instruction.R2];
                            Ram.ValueRegister[instruction.R2] = temp % Ram.ValueRegister[instruction.R2];

                            SetFlag(instruction);
                        }
                        else
                            Print.Error.ErrorDivisionByZero();
                        break;
                    }
                case (int)Opcode.CMP:
                    {
                        int temp = Ram.ValueRegister[instruction.R1] - Ram.ValueRegister[instruction.R2];
                        if (temp > 0)
                            Ram.FlagRegister = (int)Flag.Positive;
                        else if (temp < 0)
                            Ram.FlagRegister = (int)Flag.Negative;
                        else
                            Ram.FlagRegister = (int)Flag.Zero;
                        break;
                    }
                case (int)Opcode.CPY:
                    {
                        Ram.ValueRegister[instruction.R1] = Ram.ValueRegister[instruction.R2];
                        break;
                    }
                case (int)Opcode.JMP:
                    {
                        switch (instruction.R1)
                        {
                            case 0:
                                Ram.InstructionRegister += Rom.JumpList[instruction.R2];
                                break;
                            case 1:
                                if (Ram.FlagRegister == (int)Flag.Zero)
                                    Ram.InstructionRegister += Rom.JumpList[instruction.R2];
                                break;
                            case 2:
                                if (Ram.FlagRegister != (int)Flag.Zero)
                                    Ram.InstructionRegister += Rom.JumpList[instruction.R2];
                                break;
                            case 3:
                                if (Ram.FlagRegister == (int)Flag.Positive)
                                    Ram.InstructionRegister += Rom.JumpList[instruction.R2];
                                break;
                            case 4:
                                if (Ram.FlagRegister == (int)Flag.Negative)
                                    Ram.InstructionRegister += Rom.JumpList[instruction.R2];
                                break;
                            case 5:
                                if (Ram.FlagRegister != (int)Flag.Negative)
                                    Ram.InstructionRegister += Rom.JumpList[instruction.R2];
                                break;
                            case 6:
                                if (Ram.FlagRegister != (int)Flag.Positive)
                                    Ram.InstructionRegister += Rom.JumpList[instruction.R2];
                                break;
                        }
                        break;
                    }
                case (int)Opcode.LOD:
                    {
                        int constInt = Rom.ConstList[instruction.R2];
                        break;
                    }
                case (int)Opcode.INP:
                    {
                        Ram.ValueRegister[instruction.R1] = Int32.Parse(Console.ReadLine());
                        break;
                    }
                case (int)Opcode.OUT:
                    {
                        Console.WriteLine(Ram.ValueRegister[instruction.R1]);
                        break;
                    }
                case (int)Opcode.END:
                    {
                        //system("PAUSE");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("FATAL ERROR DRUING EXECUTE FILE: invalid value of instruction.Op: " + instruction.Op);
                        break;
                    };
            }

            Ram.InstructionRegister++;

            return 0;
        }

        static string InputPath()
        {
            bool inputOk = false;
            string path = "";
            while (!(inputOk))
            {
                Console.WriteLine("Podaj nazwe pliku (bez rozszerzenia): ");
                Console.Write("> ");
                path = Console.ReadLine().Trim();
                if (path.Length < 1)
                    Console.WriteLine("Bledna sciezka!");
                else
                    inputOk = true;
            }
            return (path + ".bin");
        }
    }
}
