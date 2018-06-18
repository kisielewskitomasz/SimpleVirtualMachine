using System;

namespace SimpleVirtualMachine.Models
{
    interface ICommand
    {
        string Description { get; }
        void Execute(Menu menu);
    }
}
