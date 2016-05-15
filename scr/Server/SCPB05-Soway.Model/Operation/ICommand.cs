using System;
using System.Runtime.Serialization;
namespace Soway.Model
{
 
    public interface ICommand
    {
 
        Model ArgModel { get; set; }
         
        string ArgPropertyExp { get; set; }
        string ArgSourceIdExp { get; set; }
        CommandsType CommandsType { get; set; }
        string Exp { get; set; }
        Property Property { get; set; }

        string PropertyExp { get; set; }
        string TempResultValue { get; set; }

        int Index { get; set; }
    }
}
