using System;

namespace interpreter
{
    public class Exceptions
    {
        //public static Exception  = new Exception("Unsupported file type of source code!\n Only .txt files a supported.");
        public static Exception UnsupportedCommand = new Exception("Unsupported command in source code!");
        public static Exception InconsistentSourceCode = new Exception("Syntax error: Inconsistent soure code!");
        public static Exception BlankSourceCode = new Exception("Source code doesn't contain any executable commands!");
        public static Exception MissingJumpForwardCommand = new Exception("Inconsistent soure code - missing jump forward command!");
        public static Exception MissingJumpBackwardCommand = new Exception("Inconsistent soure code - missing jump backward command!");
        public static Exception UnknownCommand = new Exception("Unknown command!");
        public static Exception UnableToReadData = new Exception("Unable to read next byte from input (reached end of input file)!");
        public static Exception MissingCommand = new Exception("Syntax error:\nCommands 1000 and 1001 must be followed by another command!");
        public static Exception CantExecute = new Exception("Programm can't be executed. One or more errors occured during interpreting source code!");

        public static Exception FileNotFound(string path)
        {
            return new Exception("File not found (" + path + ")!");
        }

        public static Exception UnsupportedFileType(string path)
        {
            return new Exception("Unsupported file type (" + path + ")!\n Only .txt or .bin files a supported.");
        }

        public static Exception UnsupportedCommandDetail(string command)
        {
            return new Exception("Unsupported command in source code [ " + command + " ]!");
        }
    }
}
