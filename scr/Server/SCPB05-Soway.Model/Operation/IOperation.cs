using System;
namespace Soway.Model
{
    public interface IOperation 
    {
        string ArgFilter { get; set; }
        Model ArgModel { get; set; }
        BaseOperationType BaseOperationType { get; set; }
        System.Collections.Generic.List<ICommand> GetCommands();
         string InvokeDLL { get; set; }
         string InvokeClass { get; set; }
         string InvokeMethod { get; set; }

         


    }
}
