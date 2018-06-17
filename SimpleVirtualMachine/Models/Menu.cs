using System;
namespace SimpleVirtualMachine.Models
{
    public class Menu
    {
        internal static ICommand[] mainCommands =
        {
            new Quit.ExitCommand(),
            new ShowDescription.Command(),
            new OpenEditor.Command(),
            new RunLinear.Command(),
            new RunStep.Command(),
            new ShowExample.Command()
        };

        public Menu()
        {
            Print.Show.Info();
            MakeMenu(mainCommands);
        }

        internal void MakeMenu(ICommand[] commands)
        {
            while (true)
            {
                PrintMenu(commands);
                int commandIndex = GetUserChoice(commands);
                commands[commandIndex].Execute(this);
            }
        }

        void PrintMenu(ICommand[] commands)
        {
            for (int i = 1; i < commands.Length; i++)
            {
                Console.WriteLine($"{i}. {commands[i].Description}");
            }
            Console.WriteLine($"{0}. {commands[0].Description}");
        }

        int GetUserChoice(ICommand[] commands)
        {
            string userChoice = string.Empty;
            int commandIndex = -1;
            do
            {
                Console.Write("> ");
                userChoice = Console.ReadLine();
            }
            while (!int.TryParse(userChoice, out commandIndex) || commandIndex >= commands.Length);
            return commandIndex;
        }
    }
}
