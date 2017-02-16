using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace interpreter
{
    class NLProgram
    {
        private static readonly Dictionary<string, Commands> cmdDictionary = CommandsDictionary.CmdDictionary;
        private static List<Commands> commands = null;
        private static List<JumpPair> jumpPairs = null;
        private StreamReader inputStreamReader = null;
        private StreamWriter outputStreamWriter = null;
        private byte[] cellArray = null;

        public NLProgram(string prgPath, string inputPath, string outputPath)
        {
            
            cellArray = new byte[100000];
            jumpPairs = new List<JumpPair>();

            try
            {
                CreateProgram(prgPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// If there are no errors, method prepares source code to execution,
        /// </summary>
        /// <param name="prgPath">Path to file with source code.</param>
        private static void CreateProgram(string prgPath)
        {
            if (!File.Exists(prgPath))
                throw Exceptions.FileNotFound;

            if (new FileInfo(prgPath).Extension != ".txt")
                throw Exceptions.UnsupportedFileType;

            ReadProgram(prgPath);

            if (commands.Count <= 0)
                throw Exceptions.BlankSourceCode;

            var jumpCommands = commands.Where(x =>
                x == Commands.JumpForward ||
                x == Commands.JumpBackward).ToList();

            if (jumpCommands.Count % 2 == 1)
                throw Exceptions.InconsistentSourceCode;

            if (jumpCommands.Count > 0)
                CreateJumpPairs(0);

            if (jumpPairs.Count * 2 != jumpCommands.Count)
                throw Exceptions.InconsistentSourceCode;
        }

        /// <summary>
        /// Method parses the source code and checks for occurrence of unsupported commands and its inconsistences.
        /// </summary>
        /// <param name="prgPath">Path to file with source code.</param>
        private static void ReadProgram(string prgPath)
        {
            commands = new List<Commands>();
            var reader = new StreamReader(prgPath);
            var programString = Regex.Replace(reader.ReadToEnd(), @"\s+", "");
            reader.Close();

            var builder = new StringBuilder();
            foreach (var character in programString)
            {
                builder.Append((character));
                if (builder.Length == 4)
                {
                    var command = builder.ToString();
                    builder.Clear();

                    if (cmdDictionary.ContainsKey(command))
                        commands.Add(cmdDictionary[command]);
                    else 
                        throw Exceptions.UnsupportedCommand;
                }
            }

            foreach (var cmnd in commands)
            {
                Console.WriteLine(cmnd);
            }

            if (builder.Length != 0)
                throw Exceptions.InconsistentSourceCode;
        }

        private static int CreateJumpPairs(int currentIndex)
        {
            var forward = -1;
            var backward = -1;

            if (currentIndex >= commands.Count)
                return currentIndex;

            switch (commands[currentIndex])
            {
                case Commands.JumpForward:
                    forward = currentIndex;
                    backward = CreateJumpPairs(++currentIndex);

                    if (forward == -1)
                        throw Exceptions.MissingJumpForwardCommand;
                    if (backward == -1 || backward >= commands.Count)
                        throw Exceptions.MissingJumpBackwardCommand;

                    jumpPairs.Add(new JumpPair() {ForwardIndex = forward, BackwardIndex = backward});
                    return CreateJumpPairs(++backward);

                case Commands.JumpBackward:
                    return currentIndex;

                default:
                    return CreateJumpPairs(++currentIndex);
            }
        }
    }
}
