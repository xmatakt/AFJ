using System;

namespace interpreter
{
    public static class Exceptions
    {
        public static Exception FileNotFound = new Exception("File not found!");
        public static Exception UnsupportedFileType = new Exception("Unsupported file type of source code!\n Only .txt files a supported.");
        public static Exception UnsupportedCommand = new Exception("Unsupported command in source code!");
        public static Exception InconsistentSourceCode = new Exception("Inconsistent soure code!");
        public static Exception BlankSourceCode = new Exception("Source code doesn't contain any executable commands!");
        public static Exception MissingJumpForwardCommand = new Exception("Inconsistent soure code - missing jump forward command!");
        public static Exception MissingJumpBackwardCommand = new Exception("Inconsistent soure code - missing jump backward command!");
        public static Exception DataOverflow = new Exception("Message");
    }
}
