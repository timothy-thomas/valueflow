using System;
using System.Runtime.InteropServices;
using GME.Util;
using GME.MGA;

namespace GME.CSharp
{
    
    abstract class ComponentConfig
    {
        // Set paradigm name. Provide * if you want to register it for all paradigms.
		public const string paradigmName = "ValueFlow";
		
		// Set the human readable name of the interpreter. You can use white space characters.
        public const string componentName = "ValueFlowInterpreter";
        
		// Specify an icon path
		public const string iconName = "ValueFlowInterpreter.ico";
        
		public const string tooltip = "ValueFlow Interpreter";

		// If null, updated with the assembly path + the iconName dynamically on registration
        public static string iconPath = null; 
        
		// Uncomment the flag if your component is paradigm independent.
		public static componenttype_enum componentType = componenttype_enum.COMPONENTTYPE_INTERPRETER;
				
        public const regaccessmode_enum registrationMode = regaccessmode_enum.REGACCESS_SYSTEM;
        public const string progID = "MGA.Interpreter.ValueFlowInterpreter";
        public const string guid = "3816D242-9E8C-41A7-BE67-6DACE76441BC";
    }
}
