using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace interpreter
{
    class NLProgram
    {
        private static readonly Dictionary<string, Commands> cmdDictionary = CommandsDictionary.CmdDictionary;
        private static List<Commands> commands = null;
        private static List<JumpPair> jumpPairs = null;
        private static BinaryReader inputStreamReader = null;
        private static BinaryWriter outputStreamWriter = null;
        private static byte[] cellArray = null;
        private static bool canExecute = false;

        public NLProgram(string prgPath, string inputPath, string outputPath)
        {
            cellArray = new byte[100000];
            jumpPairs = new List<JumpPair>();

            try
            {
                CheckFile(prgPath, ".txt");
                CheckFile(inputPath, ".bin");
                //CheckFile(outputPath, ".bin");

                inputStreamReader = new BinaryReader(new FileStream(inputPath, FileMode.Open));
                outputStreamWriter = new BinaryWriter(new FileStream(outputPath, FileMode.OpenOrCreate));

                CreateProgram(prgPath);
                canExecute = true;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e.Message);
            }
        }

        public int ExecuteProgram()
        {
            if (!canExecute)
                throw Exceptions.CantExecute;

            var dataPointer = 0;

            for (var i = 0; i < commands.Count; i++)
            {
                switch(commands[i])
                {
                    case Commands.MoveRight:
                        dataPointer = MoveRight(dataPointer);
                        break;
                    case Commands.MoveLeft:
                        dataPointer = MoveLeft(dataPointer);
                        break;
                    case Commands.Increment:
                        dataPointer = Increment(dataPointer);
                        break;
                    case Commands.Decrement:
                        dataPointer = Decrement(dataPointer);
                        break;
                    case Commands.Write:
                        dataPointer = Write(dataPointer);
                        break;
                    case Commands.Read:
                        dataPointer = Read(dataPointer);
                        break;
                    case Commands.JumpForward:
                        i = JumpForward(dataPointer, i);
                        break;
                    case Commands.JumpBackward:
                        i = JumpBackward(dataPointer, i);
                        break;
                    case Commands.AddDecadic:
                        dataPointer = AddDecadic(dataPointer, i);
                        break;
                    case Commands.RemoveDecadic:
                        dataPointer = RemoveDecadic(dataPointer, i);
                        break;
                    case Commands.Nop:
                        break;
                    case Commands.Reset:
                        dataPointer = Reset(dataPointer);
                        break;
                    case Commands.JumpToBegin:
                        dataPointer = 0;
                        break;
                    default:
                        throw Exceptions.UnknownCommand;
                }

            }

            inputStreamReader.Close();
            outputStreamWriter.Flush();
            outputStreamWriter.Close();

            return 0;
        }

        #region Commands implementation
        private static int MoveRight(int dataPointer)
        {
            dataPointer++;
            if (dataPointer == cellArray.Length)
                dataPointer = 0;

            return dataPointer;
        }

        private static int MoveLeft(int dataPointer)
        {
            dataPointer--;
            if (dataPointer == -1)
                dataPointer = cellArray.Length - 1;

            return dataPointer;
        }

        private static int Increment(int dataPointer)
        {
            cellArray[dataPointer]++;
            return dataPointer;
        }

        private static int Decrement(int dataPointer)
        {
            cellArray[dataPointer]--;
            return dataPointer;
        }

        private static int Write(int dataPointer)
        {
            outputStreamWriter.Write(cellArray[dataPointer]);
            return dataPointer;
        }

        private static int Read(int dataPointer)
        {
            try
            {
                cellArray[dataPointer] = inputStreamReader.ReadByte();
                return dataPointer;
            }
            catch (EndOfStreamException)
            {
                throw Exceptions.UnableToReadData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static int JumpForward(int dataPointer, int index)
        {
            return cellArray[dataPointer] == 0x00 ? jumpPairs.First(x => x.ForwardIndex == index).BackwardIndex : index;
        }

        private static int JumpBackward(int dataPointer, int index)
        {
            return cellArray[dataPointer] == 0x00 ? index : jumpPairs.First(x => x.BackwardIndex == index).ForwardIndex;
        }

        private static int AddDecadic(int dataPointer, int index)
        {
            if (index == commands.Count - 1)
                throw Exceptions.MissingCommand;

            var tmp = Convert.ToByte(cmdDictionary.First(x => x.Value == commands[index + 1]).Key, 2);
            cellArray[dataPointer] += tmp;

            return dataPointer;
        }

        private static int RemoveDecadic(int dataPointer, int index)
        {
            if (index == commands.Count - 1)
                throw Exceptions.MissingCommand;

            cellArray[dataPointer] -= Convert.ToByte(cmdDictionary.First(x => x.Value == commands[index]).Key, 2);

            return dataPointer;
        }

        private static int Reset(int dataPointer)
        {
            cellArray[dataPointer] = 0x00;

            return dataPointer;
        }
        #endregion

        /// <summary>
        /// If there are no errors, method prepares source code to execution,
        /// </summary>
        /// <param name="prgPath">Path to file with source code.</param>
        private static void CreateProgram(string prgPath)
        {
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
                        throw Exceptions.UnsupportedCommandDetail(command) ;
                }
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

        private void CheckFile(string path, string extension)
        {
            if (!File.Exists(path))
                throw Exceptions.FileNotFound(path);

            if (new FileInfo(path).Extension != extension)
                throw Exceptions.UnsupportedFileType(path);
        }
    }
}
