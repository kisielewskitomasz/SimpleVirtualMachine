using System;
using System.Collections.Generic;
using System.IO;
using static SimpleVirtualMachine.Models.VirtualMachine;

namespace SimpleVirtualMachine.Models
{
    public class Editor
    {
        public List<Operand> OperandList { get => _operandList; set => _operandList = value; }
        public List<Constant> ConstantList { get => _constantList; set => _constantList = value; }
        List<Operand> _operandList;
        List<Constant> _constantList;

        public Editor()
        {
            OperandList = new List<Operand>();
            ConstantList = new List<Constant>();
            Edit();
        }

        bool WriteVMFile(string path)
        {
            using (FileStream fs = File.OpenWrite(path))
            {
                for (int i = 0, j = 0; i < OperandList.Count; i++)
                {
                    fs.Write(OperandList[i].Bytes, 0, 2);
                    Instruction instruction = new Instruction(OperandList[i]);
                    if (instruction.Op == (int)Opcode.JMP || instruction.Op == (int)Opcode.LOD)
                        fs.Write(ConstantList[j++].Bytes, 0, 4);
                }
                return true;
            }
        }

        public void Edit()
        {
            Print.Show.AvabileOpcodes();
            int inputOpcode = 0;
            bool inputOk;

            Operand operand;
            Constant constant;
            while (!(inputOpcode == (int)Opcode.SAV))
            {
                inputOk = false;
                while (!(inputOk))
                {
                    Console.WriteLine("Podaj operacje: ");
                    Console.Write("> ");
                    inputOk = Int32.TryParse(Console.ReadLine(), out inputOpcode);
                }
                switch (inputOpcode)
                {
                    case (int)Opcode.ADD:
                    case (int)Opcode.SUB:
                    case (int)Opcode.MUL:
                    case (int)Opcode.DIV:
                    case (int)Opcode.CMP:
                        {
                            Instruction instruction = new Instruction(inputOpcode);
                            InputFirstRegister(instruction);
                            InputSecondRegister(instruction);
                            operand = new Operand(instruction);
                            OperandList.Add(operand);
                            Console.WriteLine($"Operacja {OperationText[instruction.Op]}: {instruction.R2}*2^10 + {instruction.R1}*2^4 + {instruction.Op} = {operand.ToInt32()}");
                            break;
                        }
                    case (int)Opcode.INP:
                    case (int)Opcode.OUT:
                        {
                            Instruction instruction = new Instruction(inputOpcode);
                            InputFirstRegister(instruction);
                            operand = new Operand(instruction);
                            OperandList.Add(operand);
                            Console.WriteLine($"Operacja {OperationText[instruction.Op]}: {instruction.R1}*2^4 + {instruction.Op} = {operand.ToInt32()}");
                            break;
                        }
                    case (int)Opcode.JMP:
                        {
                            Instruction instruction = new Instruction(inputOpcode);
                            InputJumpCondition(instruction);
                            operand = new Operand(instruction);
                            constant = new Constant(InputJumpValue());
                            OperandList.Add(operand);
                            ConstantList.Add(constant);
                            Console.WriteLine($"Operacja {OperationText[instruction.Op]}: {instruction.R2}*2^10 + {instruction.R1}*2^4 + {instruction.Op} = {operand.ToInt32()}, Skok: {constant.ToInt32()}");
                            break;
                        }
                    case (int)Opcode.LOD:
                        {
                            Instruction instruction = new Instruction(inputOpcode);
                            InputFirstRegister(instruction);
                            operand = new Operand(instruction);
                            OperandList.Add(operand);
                            constant = new Constant(InputConstantValue());
                            ConstantList.Add(constant);
                            Console.WriteLine($"Operacja {OperationText[instruction.Op]}: {instruction.R2}*2^10 + {instruction.R1}*2^4 + {instruction.Op} = {operand.ToInt32()}, Stala: {constant.ToInt32()}");
                            break;
                        }
                    case (int)Opcode.END:
                        {
                            Instruction instruction = new Instruction(inputOpcode);
                            operand = new Operand(instruction);
                            OperandList.Add(operand);
                            break;
                        }
                    case (int)Opcode.SAV:
                        break;
                    default:
                        {
                            Console.WriteLine("Nieprawidlowa operacja!");
                            Console.Write("> ");
                            break;
                        }
                }
            }
            WriteVMFile(InputPath());
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

        static bool InputJumpCondition(Instruction instruction)
        {
            bool inputOk = false;
            while (!(inputOk))
            {
                Console.WriteLine("Podaj warunek skoku (0 - 6): ");
                Console.Write("> ");
                int inputJumpCondition = -1;
                if ((Int32.TryParse(Console.ReadLine(), out inputJumpCondition)) && (inputJumpCondition >= 0) && (inputJumpCondition <= 6))
                {
                    instruction.R1 = inputJumpCondition;
                    inputOk = true;
                }
                else
                    Console.WriteLine("Bledny warunek skoku!");
            }

            return inputOk;
        }

        static int InputConstantValue()
        {
            bool inputOk = false;
            int inputConstatnt = 0;
            while (!(inputOk))
            {
                Console.WriteLine("Podaj wartosc stalej: ");
                Console.Write("> ");
                if (Int32.TryParse(Console.ReadLine(), out inputConstatnt))
                    inputOk = true;
                else
                    Console.WriteLine("Bledna wartosc stalej!");
            }
            return inputConstatnt;
        }

        static int InputJumpValue()
        {
            bool inputOk = false;
            int inputJump = 0;
            while (!(inputOk))
            {
                Console.WriteLine("Podaj wartosc skoku: ");
                Console.Write("> ");
                if (Int32.TryParse(Console.ReadLine(), out inputJump))
                    inputOk = true;
                else
                    Console.WriteLine("Bledna wartosc skoku!");
            }
            return inputJump;
        }

        static bool InputFirstRegister(Instruction instruction)
        {
            bool inputOk = false;
            while (!(inputOk))
            {
                Console.WriteLine("Podaj indeks pierwszego rejestru (0 - 63): ");
                Console.Write("> ");
                int inputRegister = -1;
                if ((Int32.TryParse(Console.ReadLine(), out inputRegister)) && (inputRegister >= 0) && (inputRegister <= 63))
                {
                    instruction.R1 = inputRegister;
                    inputOk = true;
                }
                else
                    Console.WriteLine("Bledny indeks pierwszego rejestru!");
            }

            return inputOk;
        }

        static bool InputSecondRegister(Instruction instruction)
        {
            bool inputOk = false;
            while (!(inputOk))
            {
                Console.WriteLine("Podaj indeks drugiego rejestru (0 - 63): ");
                Console.Write("> ");
                int inputRegister = -1;
                if (Int32.TryParse(Console.ReadLine(), out inputRegister) && (inputRegister >= 0) && (inputRegister <= 63))
                {
                    instruction.R2 = inputRegister;
                    inputOk = true;
                }
                else
                    Console.WriteLine("Bledny indeks drugiego rejestru!");
            }

            return inputOk;
        }
    }
}
