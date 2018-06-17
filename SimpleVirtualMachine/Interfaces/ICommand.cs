using System;
using SimpleVirtualMachine.Models;

namespace SimpleVirtualMachine
{
    interface ICommand
    {
        string Description { get; }
        void Execute(Menu menu);
    }
}
