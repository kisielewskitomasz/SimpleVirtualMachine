using System;

namespace SimpleVirtualMachine.Models
{
    public class ShowDescription
    {
        public class Command : ICommand
        {
            readonly ICommand[] commands =
            {
                new Quit.ReturnCommand(),
                new ShowAvabileOpcodes()
            };

            public string Description => "Szczegolowy opis Maszyny Wirtualnej.";
            public void Execute(Menu menu) 
            {
                Print.Show.Description();
                menu.MakeMenu(commands); 
            }
        }

        public class ShowAvabileOpcodes : ICommand
        {
            readonly ICommand[] commands = { new Quit.ReturnCommand() };
            public string Description => "Wyswietl dostepne operacje.";
            public void Execute(Menu menu)
            {
                Print.Show.AvabileOpcodes();
                menu.MakeMenu(commands); 
            }
        }
    }

    class OpenEditor
    {
        public class Command : ICommand
        {
            public string Description => "Tworzenie pliku programu Maszyny Wirtualnej.";
            public void Execute(Menu menu)
            {
                Editor editor = new Editor();
            }
        }
    }

    class RunLinear
    {
        public class Command : ICommand
        {
            public string Description => "Wykonywanie liniowe programu Maszyny Wirtualnej.";
            public void Execute(Menu menu)
            {
                // add input file
                VirtualMachine VM = new VirtualMachine();
            }
        }
    }

    class RunStep
    {
        public class Command : ICommand
        {
            public string Description => "Wykonywanie krokowe programu Maszyny Wirtualnej.";
            public void Execute(Menu menu)
            {
                // add input file
                VirtualMachine VM = new VirtualMachine();
            }
        }
    }

    class ShowExample
    {
        public class Command : ICommand
        {
            readonly ICommand[] commands =
            {
                new Quit.ReturnCommand(),
                new ShowProjectExampleCommand(),
                new ShowAdding2IntCommand(),
                new ShowDividing2IntCommand(),
                new ShowBigestFrom3IntsCommand()
            };

            public string Description => "Przykladowe programy Maszyny Wirtualnej.";
            public void Execute(Menu menu) { menu.MakeMenu(commands); }
        }

        public class ShowProjectExampleCommand : ICommand
        {
            readonly ICommand[] commands = { new Quit.ReturnCommand() };
            public string Description => "Przyklad z zadania projektowego.";
            public void Execute(Menu menu)
            {
                Print.Show.ProjectExample();
                menu.MakeMenu(commands);
            }
        }

        public class ShowAdding2IntCommand : ICommand
        {
            readonly ICommand[] commands = { new Quit.ReturnCommand() };
            public string Description => "Dodawanie dwoch liczb.";
            public void Execute(Menu menu)
            {
                Print.Show.Adding2Int();
                menu.MakeMenu(commands);
            }
        }

        public class ShowDividing2IntCommand : ICommand
        {
            readonly ICommand[] commands = { new Quit.ReturnCommand() };
            public string Description => "Dzielenie 2 liczb.";
            public void Execute(Menu menu)
            {
                Print.Show.Dividing2Int();
                menu.MakeMenu(commands);
            }
        }

        public class ShowBigestFrom3IntsCommand : ICommand
        {
            readonly ICommand[] commands = { new Quit.ReturnCommand() };
            public string Description => "Najwieksza z trzech.";
            public void Execute(Menu menu)
            {
                Print.Show.BigestFrom3Ints();
                menu.MakeMenu(commands);
            }
        }
    }

    class Quit
    {
        public class ExitCommand : ICommand
        {
            public string Description => "Zakoncz program Maszyna Wirtualna.";
            public void Execute(Menu menu) { Environment.Exit(0); }
        }

        public class ReturnCommand : ICommand
        {
            public string Description => "Powrot do MENU GLOWNEGO.";
            public void Execute(Menu menu) 
            {
                Print.Show.Info();
                menu.MakeMenu(Menu.mainCommands);
            }
        }
    }
}
