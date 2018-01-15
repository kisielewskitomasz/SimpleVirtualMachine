using System;
using System.IO;
using System.Collections.Generic;

namespace SimpleVirtualMachine
{
    public class VirtualMachine
    {
        enum Registers { r1 = 0, r2 = 1 };
        enum Flag { Negative = -1, Zero = 0, Positive = 1 };
        enum Opcode { ADD = 0, SUB = 1, MUL = 2, DIV = 3, CMP = 4, CPY = 5, JMP = 6, LOD = 7, INP = 8, OUT = 9, END = 10, SAV = 11 }; // SAV isn't a opcode of VM, it's using for simplify editor
        enum RunMode { Normal = 0, StepByStep = 1 };
        enum ErrorCode { DividedByZero = 0, EndOfFile, OpenFile };

        static readonly string[] OperationText = { "dodaj", "odejmij", "mnoz", "dziel", "porownaj", "kopiuj", "skocz", "wczytaj", "pobierz", "wyswietl", "zakoncz", "zapisz" };

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

        class Instruction
        {
            public int Op { get => _op; set => _op = value; }
            public int R1 { get => _r1; set => _r1 = value; }
            public int R2 { get => _r2; set => _r2 = value; }

            int _op;
            int _r1;
            int _r2;

            public Instruction(byte[] operand)
            {
                short operation = BitConverter.ToInt16(operand, 0);

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

        public VirtualMachine(string path)
        {
            Rom = new ROM();
            Ram = new RAM();
            ReadVMFile(path);
            Run();
        }

        bool Run()
        {
            while(!(Rom.InstructionList[Ram.InstructionRegister].Op == (int)Opcode.END))
            {
                Execute(Rom.InstructionList[Ram.InstructionRegister]);
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
                    byte[] nextOperand = new byte[2];
                    byte[] nextInt = new byte[4];

                    bytesRead = fs.Read(nextOperand, 0, 2);
                    if (bytesRead == 0)
                    {
                        Console.WriteLine("reading nextOperand: EOF");
                        break;
                    }

                    Console.WriteLine($"i: {i} value: {nextOperand[0]:X2}{nextOperand[1]:X2}");

                    Instruction nextInstruction = new Instruction(nextOperand);
                    Console.WriteLine($"R2: {nextInstruction.R2} R1: {nextInstruction.R1} Op: {nextInstruction.Op} ({OperationText[nextInstruction.Op]})");

                    if (nextInstruction.Op == (int)Opcode.JMP)
                    {
                        bytesRead = +fs.Read(nextInt, 0, 4);
                        if (bytesRead == 0)
                            Console.WriteLine("reading nextInt(jump) EOF");
                        else
                        {
                            nextInstruction.R2 = Rom.JumpList.Count;
                            Console.WriteLine("Reading file, OP JUMP, R2: " + nextInstruction.R2);
                            Rom.JumpList.Add(BitConverter.ToInt32(nextInt, 0));
                        }
                    }
                    else if (nextInstruction.Op == (int)Opcode.LOD)
                    {
                        bytesRead = +fs.Read(nextInt, 0, 4);
                        if (bytesRead == 0)
                            Console.WriteLine("reading nextInt(load) EOF");
                        else
                        {
                            nextInstruction.R2 = Rom.ConstList.Count;
                            Rom.ConstList.Add(BitConverter.ToInt32(nextInt, 0));
                        }
                    }

                    Rom.InstructionList.Add(nextInstruction);
                }
            }
            return true;
        }

        int Execute(Instruction instruction)
        {
            switch (instruction.Op)
            {
                case (int)Opcode.ADD: // dodaj
                    {
                        Ram.ValueRegister[instruction.R1] += Ram.ValueRegister[instruction.R2];
                        if (Ram.ValueRegister[instruction.R1] > 0)
                            Ram.FlagRegister = (int)Flag.Positive;
                        else if (Ram.ValueRegister[instruction.R1] < 0)
                            Ram.FlagRegister = (int)Flag.Negative;
                        else
                            Ram.FlagRegister = (int)Flag.Zero;
                        break;
                    }
                case (int)Opcode.SUB: // odejmij
                    {
                        Ram.ValueRegister[instruction.R1] -= Ram.ValueRegister[instruction.R2];
                        if (Ram.ValueRegister[instruction.R1] > 0)
                            Ram.FlagRegister = (int)Flag.Positive;
                        else if (Ram.ValueRegister[instruction.R1] < 0)
                            Ram.FlagRegister = (int)Flag.Negative;
                        else
                            Ram.FlagRegister = (int)Flag.Zero;
                        break;
                    }
                case (int)Opcode.MUL: // mnoz
                    {
                        Ram.ValueRegister[instruction.R1] *= Ram.ValueRegister[instruction.R2];
                        if (Ram.ValueRegister[instruction.R1] > 0)
                            Ram.FlagRegister = (int)Flag.Positive;
                        else if (Ram.ValueRegister[instruction.R1] < 0)
                            Ram.FlagRegister = (int)Flag.Negative;
                        else
                            Ram.FlagRegister = (int)Flag.Zero;
                        break;
                    }
                case (int)Opcode.DIV:
                    {
                        int temp = Ram.ValueRegister[instruction.R1];

                        if (Ram.ValueRegister[instruction.R2] != 0)
                        {
                            Ram.ValueRegister[instruction.R1] /= Ram.ValueRegister[instruction.R2];
                            Ram.ValueRegister[instruction.R2] = temp % Ram.ValueRegister[instruction.R2];

                            if (Ram.ValueRegister[instruction.R1] > 0)
                                Ram.FlagRegister = (int)Flag.Positive;
                            else if (Ram.ValueRegister[instruction.R1] < 0)
                                Ram.FlagRegister = (int)Flag.Negative;
                            else
                                Ram.FlagRegister = (int)Flag.Zero;
                        }
                        //else
                            //wyswietlBlad(bladDziel);
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
                                {
                                    Ram.InstructionRegister += instruction.R2;
                                    break;
                                }
                            case 1:
                                {
                                    if (Ram.FlagRegister == (int)Flag.Zero)
                                        Ram.InstructionRegister += instruction.R2;
                                    break;
                                }
                            case 2:
                                {
                                    if (Ram.FlagRegister != (int)Flag.Zero)
                                        Ram.InstructionRegister += instruction.R2;
                                    break;
                                }
                            case 3:
                                {
                                    if (Ram.FlagRegister == (int)Flag.Positive)
                                        Ram.InstructionRegister += instruction.R2;
                                    break;
                                }
                            case 4:
                                {
                                    if (Ram.FlagRegister == (int)Flag.Negative)
                                        Ram.InstructionRegister += instruction.R2;
                                    break;
                                }
                            case 5:
                                {
                                    if (Ram.FlagRegister != (int)Flag.Negative)
                                        Ram.InstructionRegister += instruction.R2;
                                    break;
                                }
                            case 6:
                                {
                                    if (Ram.FlagRegister != (int)Flag.Positive)
                                        Ram.InstructionRegister += instruction.R2;
                                    break;
                                }
                            default: break;
                        }
                        break;
                    }
                case (int)Opcode.LOD:
                    {
                        int constInt = Rom.JumpList[Rom.InstructionList[Rom.InstructionList.Count].R2];
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
    }
}
