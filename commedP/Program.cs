using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace commedP
{

    class Program
    {
        public static directory currentDirectory;
        public static string currentPath;

        class commandArg
        {
           // public static int lastValue = 0;
            public void Command(String input, String directory)
            {
                string[] textSplit = input.Split(" ");
                textSplit[0] = textSplit[0].ToUpper();

                if (textSplit[0].Length == 0)
                {
                    Console.WriteLine(directory);
                }
                else if (textSplit[0] == "CD")
                {
                    int index = Program.currentDirectory.searchDirectory(textSplit[1]);
                    if (index != -1)
                    {
                        byte attr = Program.currentDirectory.directoryTable[index].fileAtterbute;
                        if (attr == 0)
                        {
                            int f_c = Program.currentDirectory.directoryTable[index].firstCluster;
                            directory d = new directory(textSplit[1].ToCharArray(), 0, f_c, 0, Program.currentDirectory);
                            Program.currentDirectory = d;
                            Program.currentPath = "\\" + Program.currentDirectory.fileName;
                            d.readDirectory();
                        }
                        Console.WriteLine(directory + '>');
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                        Console.WriteLine(directory + '>');
                    }

                }
                else if (textSplit[0] == "HELP")
                {
                    if (textSplit[0] != input.ToUpper())
                    {
                        textSplit[1] = textSplit[1].ToUpper();
                    }
                    if (input.ToUpper() == textSplit[0])
                    {
                        Console.WriteLine(
                            "CD             Displays the name of or changes the current directory.\n" +
                            "CLR            Clears the screen.\n" +
                            "COPY           Copies one or more files to another location.\n" +
                            "MD             Creates a directory.\n" +
                            "QUIT           Quits the CMD.EXE program (command interpreter).\n" +
                            "DIR            Displays a list of files and subdirectories in a directory.\n" +
                            "RD             Removes a directory.\n" +
                            "TYPE           Displays the contents of a text file.\n" +
                            "DEL            Deletes one or more files.\n" +
                            "RENAME         Renames a file or files.\n" +
                            "EXPORT         Export text file(s) to your computer.\n" +
                            "IMPORT         Import text file(s) from your computer.\n" +
                            directory + '>'
                            );

                    }
                    else if (textSplit[1] == "CD")
                    {
                        Console.WriteLine($"CD             Displays the name of or changes the current directory.\n{directory}>");
                    }
                    else if (textSplit[1] == "CLR")
                    {
                        Console.WriteLine($"CLR            Clears the screen.\n{directory}>");
                    }
                    else if (textSplit[1] == "COPY")
                    {
                        Console.WriteLine($"COPY           Copies one or more files to another location.\n{directory}>");
                    }
                    else if (textSplit[1] == "MD")
                    {
                        Console.WriteLine($"MD             Creates a directory.\n{directory}>");
                    }
                    else if (textSplit[1] == "QUIT")
                    {
                        Console.WriteLine($"QUIT           Quits the CMD.EXE program (command interpreter).\n{directory}>");
                    }
                    else if (textSplit[1] == "DIR")
                    {
                        Console.WriteLine($"DIR            Displays a list of files and subdirectories in a directory.\n{directory}>");
                    }
                    else if (textSplit[1] == "RD")
                    {
                        Console.WriteLine($"RD             Removes a directory.\n{directory}>");
                    }
                    else if (textSplit[1] == "TYPE")
                    {
                        Console.WriteLine($"TYPE           Displays the contents of a text file.\n{directory}>");
                    }
                    else if (textSplit[1] == "DEL")
                    {
                        Console.WriteLine($"DEL            Deletes one or more files.\n{directory}>");
                    }
                    else if (textSplit[1] == "RENAME")
                    {
                        Console.WriteLine($"RENAME            Renames a file or files.\n{directory}>");
                    }
                    else if (textSplit[1] == "EXPORT")
                    {
                        Console.WriteLine($"EXPORT            Export text file(s) to your computer.\n{directory}>");
                    }
                    else if (textSplit[1] == "IMPORT")
                    {
                        Console.WriteLine($"IMPORT            Import text file(s) from your computer.\n{directory}>");
                    }
                    else
                    {
                        Console.WriteLine($"This command is not supported by the help utility.  Try \"{textSplit[1]} /? \".\n{directory}>");
                    }
                }
                else if (textSplit[0] == "CLR")
                {
                    Console.Clear();
                    Console.WriteLine(directory + '>');
                }
                else if (textSplit[0] == "QUIT")
                {
                    System.Environment.Exit(0);
                    Console.WriteLine(directory + '>');
                }
                else if (textSplit[0] == "MD")
                {
                    if (Program.currentDirectory.searchDirectory(textSplit[1]) == -1)
                    {
                        directoryEntry d = new directoryEntry(textSplit[1].ToCharArray(), 0, 0, 0);
                        Program.currentDirectory.directoryTable.Add(d);
                        Program.currentDirectory.writeDirectory();
                        if (Program.currentDirectory.parent != null)
                        {
                            Program.currentDirectory.parent.updateDirectory(Program.currentDirectory.parent);
                            Program.currentDirectory.parent.writeDirectory();
                        }
                        Console.WriteLine(directory + '>');
                    }
                    else
                    {
                        Console.WriteLine("A subdirectory or file already exist");
                        Console.WriteLine(directory + '>');
                    }
                }
                else if (textSplit[0] == "RD")
                {
                    int index = Program.currentDirectory.searchDirectory(textSplit[1]);
                    if (index != -1)
                    {
                        int f_cluster = Program.currentDirectory.directoryTable[index].firstCluster;
                        directory d = new directory(textSplit[1].ToCharArray(), 0, f_cluster, 0, Program.currentDirectory);
                        d.deleteDirectory();
                        Program.currentPath = new string(Program.currentDirectory.fileName).Trim();
                        Console.WriteLine(directory + '>');
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified");
                        Console.WriteLine(directory + '>');

                    }
                }
                else if (textSplit[0] == "TYPE")
                {
                    int index = Program.currentDirectory.searchDirectory(textSplit[1]);
                    if (index != -1)
                    {
                        int fC = Program.currentDirectory.directoryTable[index].firstCluster;
                        int size = Program.currentDirectory.directoryTable[index].fileSize;
                        string content = null;
                        fileEntry f = new fileEntry(content.ToCharArray(), 0, fC, size, content, Program.currentDirectory);
                        f.readFileEntry();
                        Console.WriteLine(f.content);
                        Console.WriteLine(directory + '>');
                    }
                    else
                    {
                        Console.WriteLine("system can't find file");
                        Console.WriteLine(directory + '>');
                    }


                    //if (textSplit[0] == input.ToUpper())
                    //{
                    //    Console.WriteLine("The syntax of the command is incorrect.");
                    //    Console.WriteLine(directory + '>');
                    //}
                    //else
                    //{
                    //    if(File.Exists(directory+"\\"+textSplit[1]))
                    //    {
                    //        string text = System.IO.File.ReadAllText($"C:\\My file\\FCI\\3-2nd tearm\\os\\{textSplit[1]}");
                    //        Console.WriteLine(text);
                    //        Console.WriteLine(directory + '>');
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("this file is not exists.");
                    //        Console.WriteLine(directory + '>');
                    //    }  

                    //}
                }
                else if (textSplit[0] == "COPY")
                {
                    int index = Program.currentDirectory.searchDirectory(textSplit[1]);
                    if (index != -1)
                    {
                        int fC = Program.currentDirectory.directoryTable[index].firstCluster;
                        int size = Program.currentDirectory.directoryTable[index].fileSize;
                        string content = null;
                        fileEntry f = new fileEntry(textSplit[1].ToCharArray(), 0, size, fC, content, Program.currentDirectory);
                        f.readFileEntry();
                        Console.WriteLine(f.content);
                        Console.WriteLine(directory + '>');
                    }
                    else
                    {
                        Console.WriteLine("system can't find file");
                        Console.WriteLine(directory + '>');
                    }
                }
                else if (textSplit[0] == "RENAME")
                {
                    int index = Program.currentDirectory.searchDirectory(textSplit[1]);
                    if (index != -1)
                    {
                        int n = Program.currentDirectory.searchDirectory(textSplit[2]);
                        if (n == -1)
                        {
                            directoryEntry f = Program.currentDirectory.directoryTable[index];
                            f.fileName = textSplit[2].ToCharArray();
                            Program.currentDirectory.directoryTable.RemoveAt(index);
                            Program.currentDirectory.directoryTable.Insert(n, f);
                        }
                        else
                        {
                            Console.WriteLine("dublicate file name");
                        }
                        Console.WriteLine(directory + '>');

                    }
                    else
                    {
                        Console.WriteLine("system cannot find the file specified");
                        Console.WriteLine(directory + '>');
                    }




                    ////rename zoz.txt ahmed.txt
                    //if (textSplit[0] == input.ToUpper() || textSplit[0].Length + textSplit[1].Length == input.Length - 1)
                    //{
                    //    Console.WriteLine("The syntax of the command is incorrect.");
                    //    Console.WriteLine(directory + '>');
                    //}
                    //else
                    //{
                    //    System.IO.File.Move($"C:\\My file\\FCI\\3-2nd tearm\\os\\{textSplit[1]}", $"C:\\My file\\FCI\\3-2nd tearm\\os\\{textSplit[2]}");
                    //    Console.WriteLine(directory + '>');
                    //}
                }
                else if (textSplit[0] == "EXPORT")
                {
                    int index = Program.currentDirectory.searchDirectory(textSplit[1]);
                    if (index != -1)
                    {
                        if (System.IO.Directory.Exists(textSplit[2]))
                        {
                            int fC = Program.currentDirectory.directoryTable[index].firstCluster;
                            int size = Program.currentDirectory.directoryTable[index].fileSize;
                            string content = null;
                            fileEntry f = new fileEntry(textSplit[1].ToCharArray(), 0, fC, size, content, Program.currentDirectory);
                            f.readFileEntry();
                            StreamWriter s = new StreamWriter(textSplit[2] + "\\" + textSplit[1]);
                            s.Write(f.content);
                            s.Flush();
                            s.Close();
                        }
                        else
                        {
                            Console.WriteLine("system cannot find the file specified");
                        }
                        Console.WriteLine(directory + '>');
                    }
                    else
                    {
                        Console.WriteLine("File Not exist");
                        Console.WriteLine(directory + '>');
                    }


                }
                else if (textSplit[0] == "IMPORT")
                {
                    if (File.Exists(textSplit[1]))
                    {
                        miniFAT fat = new miniFAT();
                        string content = File.ReadAllText(textSplit[1]);
                        int size = content.Length;
                        int name_start = textSplit[1].LastIndexOf("\\");
                        string name;
                        name = textSplit[1].Substring(name_start + 1);
                        int index = Program.currentDirectory.searchDirectory(name);
                        if (index == -1)
                        {
                            int fC;
                            if (size > 0)
                            {
                                fC = fat.getAvailableIndex();
                            }
                            else
                            {
                                fC = 0;
                            }
                            fileEntry f = new fileEntry(name.ToCharArray(), 0, 0, size, content, Program.currentDirectory);
                            f.writeFileEntry();
                            directoryEntry d = new directoryEntry(name.ToCharArray(), 0, 0, size);
                            Program.currentDirectory.directoryTable.Add(d);
                            Program.currentDirectory.writeDirectory();
                            Console.WriteLine(directory + '>');
                        }
                        else
                        {
                            Console.WriteLine("already exist in root");
                            Console.WriteLine(directory + '>');
                        }

                    }

                    Console.WriteLine(directory + '>');
                }
                else if (textSplit[0] == "DEL")
                {
                    int index = Program.currentDirectory.searchDirectory(textSplit[1]);
                    if (index != -1)
                    {
                        int f = Program.currentDirectory.directoryTable[index].fileAtterbute;
                        if (f == 0x0)
                        {
                            int first_cluster = Program.currentDirectory.directoryTable[index].firstCluster;
                            int file_size = Program.currentDirectory.directoryTable[index].fileSize;
                            fileEntry d = new fileEntry(textSplit[1].ToCharArray(), 0x0, first_cluster, 0, null, Program.currentDirectory);
                            d.deleteDirectory();
                        }
                        else
                        {
                            Console.WriteLine(" The system cannot find the file specified. ");
                        }
                        Console.WriteLine(directory + '>');
                    }
                    else
                    {
                        Console.WriteLine(" The system cannot find the file specified. ");
                        Console.WriteLine(directory + '>');
                    }
                }
                else if (textSplit[0] == "DIR")
                {
                    int countFileNumber = 0;
                    int countDireNum = 0;
                    int countFileSize = 0;
                    miniFAT fat = new miniFAT();

                    Console.WriteLine("Directory of " + Program.currentPath);
                    for (int i = 0; i < Program.currentDirectory.directoryTable.Count(); i++)
                    {
                        if (Program.currentDirectory.directoryTable[i].fileAtterbute == 0)
                        {
                            Console.WriteLine(Program.currentDirectory.directoryTable[i].fileSize + " " + Program.currentDirectory.directoryTable[i].fileName);
                            countFileNumber++;
                            countFileSize += Program.currentDirectory.directoryTable[i].fileSize;
                            countDireNum++;
                        }

                        else if (Program.currentDirectory.directoryTable[i].fileAtterbute == 1)
                        {
                            Console.WriteLine("<DIR> " + Program.currentDirectory.directoryTable[i].fileName);
                            countDireNum++;

                        }
                    }
                    Console.WriteLine(countFileNumber + " File(s)" + "  " + countFileSize + " bytes");
                    Console.WriteLine(countDireNum + " Dir(s)" + "  " + fat.getFreeSpace() + " bytes free");
                    Console.WriteLine(directory + '>');
                }
                else
                {
                    Console.WriteLine($"'{textSplit[0]}' is not recognized as an internal or external command,\noperable program or batch file.\n");
                    Console.WriteLine(directory + '>');
                }


            }
        }
        public class virtualDisk
        {
            public static miniFAT fat = new miniFAT();
            public static FileStream Disk;
            public static void createDisk(string path)
            {
                Disk = new FileStream(@"C:\My file\FCI\3-2nd tearm\os\commedP\File1.txt", FileMode.Create, FileAccess.ReadWrite);
                Disk.Close();
            }
            public void initialize(string path)
            {
                if (!File.Exists(path))
                {
                    createDisk(path);
                    byte[] b = new byte[1024];
                    for (int i = 0; i < b.Length; i++)
                    {
                        b[i] = 0;
                    }
                    writeCluster(b, 0);
                    fat.intial();
                    char[] s = { 'C', ':' };
                    directory root = new directory(s, 0, 5, 0, null);
                    root.writeDirectory();
                    fat.setFatTable(5, -1);
                    Program.currentDirectory = root;
                    fat.writeFat();
                }
                else
                {

                    fat.readFat();
                    char[] s = { 'C', ':' };

                    directory root = new directory(s, 0, 5, 0, null);
                    root.readDirectory();
                    Program.currentDirectory = root;
                }
            }
            public void writeCluster(byte[] block, int cluster_index)
            {
                block = new byte[1024];
                Disk.Seek(cluster_index * 1024, 0);//from start 0 mean that seekorigeal=0 in the begin of the file
                Disk.Write(block);
                Disk.Flush();
            }
            public byte[] readCluster(int cluster_index)
            {
                byte[] block = new byte[1024];
                Disk.Seek(cluster_index * 1024, 0);//from start
                Disk.Read(block);
                return block;
            }
        }
        public class miniFAT
        {
            int[] fatTable = new int[1024];
            int AvailableIndex;
            static FileStream myFile = new FileStream(@"C:\My file\FCI\3-2nd tearm\os\commedP\file1.txt", FileMode.Create, FileAccess.ReadWrite);
            static StreamWriter writeFile = new StreamWriter(myFile);
            public void preFile()
            {
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0)
                    {
                        //super block
                        for (int j = 0; j < 1024; j++)
                            writeFile.Write('0');
                    }
                    else if (i == 1)
                    {
                        //meta data & fat table
                        for (int j = 0; j < 4 * 1024; j++)
                        {
                            writeFile.Write('*');
                        }
                    }
                    else
                    {
                        //data file avalible to write in it 
                        for (int j = 0; j < 1019 * 1024; j++)
                        {
                            writeFile.Write('#');
                        }
                    }
                }

                myFile.Close();
            }
            public void intial()
            {
                for (int i = 0; i < fatTable.Length; i++)
                {
                    if (i < 5) fatTable[i] = -1;//0 => 4 (-1) file system
                    else fatTable[i] = 0;//5 => ..... (0)
                }
            }
            public void writeFat()
            {
                myFile.Seek(1024, SeekOrigin.Begin);
                byte[] b = new byte[4096];
                Buffer.BlockCopy(fatTable, 0, b, 0, b.Length);
                myFile.Write(b, 0, b.Length);
                myFile.Close();
            }
            public int[] readFat()
            {
                myFile.Seek(1024, SeekOrigin.Begin);
                byte[] b = new byte[4096];
                myFile.Read(b, 0, b.Length);
                Buffer.BlockCopy(b, 0, fatTable, 0, b.Length);
                return fatTable;
                myFile.Close();
            }
            public int getAvailableIndex()
            {
                for (int i = 5; i < 1024; i++)
                {
                    if (fatTable[i] == 0)
                    {
                        AvailableIndex = i;
                        return AvailableIndex;
                        break;
                    }
                }
                return -1;
            }
            public void setFatTable(int index, int value)
            {
                fatTable[index] = value;
            }
            public int getFatTable(int index)
            {
                return fatTable[index];
            }
            public byte[] gettingCluster(int index)
            {
                byte[] block = new byte[1024];
                myFile.Seek(1024 * index, SeekOrigin.Begin);
                myFile.Read(block, 0/*start*/, 1024/*count*/);
                myFile.Close();
                return block;
            }
            public void settingCluster(byte[] block, int index)
            {
                myFile.Seek(1024 * index, SeekOrigin.Begin);
                for (int i = 0; i < 1024; i++)
                {
                    myFile.WriteByte(block[i]);
                }
                myFile.Close();
            }
            public int getFreeSpace()
            {
                return getAvailableIndex() * 1024;
            }



        }
        public class directoryEntry
        {
            public char[] fileName = new char[11];
            public byte fileAtterbute;
            int[] fileEmpty = new int[3];
            public int fileSize;
            public int firstCluster;
            public directoryEntry(char[] fileName, byte fileAtterbute, int firstCluster, int fileSize)
            {
                this.fileName = fileName;
                this.fileAtterbute = fileAtterbute;
                this.firstCluster = firstCluster;
                this.fileSize = fileSize;
            }
            public directoryEntry()
            {

            }

            public byte[] toByte = new byte[32];
            public byte[] getBytes()
            {
                byte[] ss = Encoding.ASCII.GetBytes(fileName);
                for (int i = 0; i < 11; i++)
                {
                    if (i < ss.Length)
                        toByte[i] = ss[i];
                    else
                        toByte[i] = (byte)' ';

                }
                toByte[11] = fileAtterbute;

                for (int i = 12; i < 24; i++)
                {
                    toByte[i] = 0;
                }

                byte[] tobyte2 = BitConverter.GetBytes(firstCluster);

                for (int i = 24, c = 0; i < 28; i++, c++)
                {
                    toByte[i] = tobyte2[c];
                }

                byte[] tobyte3 = BitConverter.GetBytes(fileSize);

                for (int i = 28, c = 0; i < 32; i++, c++)
                {
                    toByte[i] = tobyte3[c];
                }

                return toByte;
            }
            public directoryEntry getDirectoryEntry(byte[] b)
            {
                directoryEntry de = new directoryEntry();
                for (int i = 0; i < 11; i++)
                {
                    de.fileName[i] = (char)b[i];
                }

                de.fileAtterbute = b[11];

                for (int i = 0; i < 12; i++)
                {
                    de.fileEmpty[i] = 0;
                }

                byte[] fc = new byte[4];
                for (int i = 24; i < 28; i++)
                {
                    fc[i % 24] = b[i];

                }
                de.firstCluster = BitConverter.ToInt32(fc, 0);

                byte[] fs = new byte[4];
                for (int i = 28; i < 32; i++)
                {
                    fc[i % 28] = b[i];
                }
                de.fileSize = BitConverter.ToInt32(fs, 0);

                return de;
            }


        }
        public class directory : directoryEntry
        {
            public static miniFAT fat = new miniFAT();
            public static virtualDisk virtualDisk = new virtualDisk();
            public List<directoryEntry> directoryTable;
            public directory parent;
            public directory(char[] fileName, byte fileAtterbute, int firstCluster, int fileSize, directory parent) : base(fileName, fileAtterbute, firstCluster, fileSize)
            {
                if (parent != null)
                {
                    this.parent = parent;
                }
                directoryTable = new List<directoryEntry>();
            }
            public void writeDirectory()
            {
                byte[] directoryTableB = new byte[32 * directoryTable.Count()];
                for (int i = 0; i < directoryTable.Count(); i++)
                {
                    byte[] directoryEntryB = directoryTable[i].getBytes();
                    for (int j = 0, x = i * 32; j < 32 && x < directoryTable.Count() * 32; j++, x++)
                    {
                        directoryTableB[x] = directoryEntryB[i];
                    }
                }

                double numOfBlocks = directoryTableB.Length / 1024;
                int numOfRequiredBlock = Convert.ToInt32(Math.Ceiling(numOfBlocks));
                int numOfFullSizeBlock = Convert.ToInt32(Math.Floor(numOfBlocks));
                double reminder = directoryTableB.Length % 1024;

                List<byte[]> allBytes = new List<byte[]>(numOfRequiredBlock);
                byte[] b = new byte[1024];

                for (int x = 0; x < numOfRequiredBlock; x++)
                {
                    for (int i = 0, j = x * 1024; i < 1024 && j < 1024; i++, j++)
                    {
                        b[i] = directoryTableB[j];
                    }
                    allBytes.Add(b);
                }

                int fatIndex;

                if (firstCluster != 0)
                {
                    fatIndex = firstCluster;
                }
                else
                {
                    fatIndex = fat.getAvailableIndex();
                    firstCluster = fatIndex;

                }

                int lastIndex = -1;

                for (int i = 0; i < allBytes.Count; i++)
                {
                    if (fatIndex != -1)
                    {
                        virtualDisk.writeCluster(allBytes[i], fatIndex);
                        fat.setFatTable(fatIndex, -1);
                        if (lastIndex != -1)
                        {
                            lastIndex = fatIndex;
                            fat.setFatTable(lastIndex, fatIndex);
                        }
                        fatIndex = fat.getAvailableIndex();
                        fat.writeFat();
                    }

                }



            }
            public void readDirectory()
            {
                if (this.firstCluster != 0 && fat.getFatTable(firstCluster) != 0)
                {
                    int fatIndex = this.firstCluster;

                    int next = fat.getFatTable(fatIndex);
                    List<byte> ls = new List<byte>();
                    List<directoryEntry> dt = new List<directoryEntry>();

                    do
                    {
                        ls.AddRange(virtualDisk.readCluster(fatIndex));
                        fatIndex = next;
                        if (fatIndex != -1)
                        {
                            next = fat.getFatTable(fatIndex);
                        }
                    } while (next != -1);

                    for (int i = 0; i < ls.Count; i++)
                    {
                        byte[] b = new byte[32];
                        for (int k = i * 32, m = 0; m < b.Length && k < ls.Count; m++, k++)
                        {
                            b[m] = ls[k];
                        }
                        if (b[0] == 0)
                            break;
                        dt.Add(getDirectoryEntry(b));
                    }
                }
            }
            public int searchDirectory(string name)
            {
                if (name.Length < 11)
                {
                    name += "\0";
                    for (int i = name.Length + 1; i < 12; i++)
                        name += " ";
                }
                else
                {
                    name = name.Substring(0, 11);
                }

                for (int i = 0; i < directoryTable.Count; i++)
                {
                    string n = new string(directoryTable[i].fileName);

                    if (n.Equals(name))
                        return i;
                }
                return -1;




            }
            public void updateDirectory(directoryEntry d)
            {
                readDirectory();
                string s = d.fileName.ToString();
                int index = searchDirectory(s);
                if (index != -1)
                {
                    directoryTable.RemoveAt(index);
                    directoryTable.Insert(index, d);
                    writeDirectory();
                }
            }
            public void deleteDirectory()
            {
                if (firstCluster != 0)
                {
                    int index = firstCluster;
                    int next = fat.getFatTable(index);
                    do
                    {
                        fat.setFatTable(index, 0);
                        if (index != -1)
                        {
                            index = next;
                            next = fat.getFatTable(index);
                        }

                    } while (index != -1);
                    if (parent != null)
                    {
                        parent.readDirectory();
                        string s = fileName.ToString();
                        int I = parent.searchDirectory(s);
                        if (I != -1)
                        {
                            parent.directoryTable.RemoveAt(I);
                            parent.writeDirectory();
                            fat.writeFat();
                        }
                    }
                }

            }
        }
        public class fileEntry : directoryEntry
        {
            public static miniFAT fat = new miniFAT();
            public static virtualDisk virtualDisk = new virtualDisk();

            public directory parent;
            public string content;

            public fileEntry(char[] fileName, byte fileAtterbute, int firstCluster, int fileSize, string content, directory parent) : base(fileName, fileAtterbute, firstCluster, fileSize)
            {

                this.parent = parent;

                this.content = content;

            }

            public void writeFileEntry()
            {

                byte[] contentArr = Encoding.ASCII.GetBytes(content);

                int numFullSize = contentArr.Length / 1024;
                double num_of_required_blocks = Math.Ceiling(contentArr.Length / 1024.0);
                //decimal num_of_full_blocks = Math.Floor(num);
                int reminder = contentArr.Length % 1024;
                if (num_of_required_blocks <= fat.getAvailableIndex())
                {
                    int fat_index;
                    int last_index = -1;
                    if (firstCluster != 0)
                    {
                        fat_index = firstCluster;
                    }
                    else
                    {
                        fat_index = fat.getAvailableIndex();
                        fat_index = firstCluster;

                    }
                    List<byte[]> ls = new List<byte[]>();
                    for (int i = 0; i < numFullSize; i++)
                    {
                        byte[] b = new byte[1024];
                        for (int j = 0; j < contentArr.Length; i++)
                        {
                            b[j % 1024] = contentArr[j];
                            if ((j + 1) % 1024 == 0)
                                ls.Add(b);
                        }

                    }
                    if (reminder > 0)
                    {
                        byte[] b = new byte[1024];
                        int start = numFullSize * 1024;
                        for (int j = start; j < (start + reminder); j++)
                            b[j % 1024] = contentArr[j];
                        ls.Add(b);


                    }

                    for (int i = 0; i < ls.Count; i++)
                    {
                        virtualDisk.writeCluster(ls[i], fat_index);
                        fat.setFatTable(fat_index, -1);
                        if (fat_index != -1)
                            fat.setFatTable(last_index, fat_index);
                        last_index = fat_index;
                        fat_index = fat.getAvailableIndex();
                    }
                    fat.writeFat();

                }

            }
            public void readFileEntry()
            {

                List<byte> ls = new List<byte>();
                int fat_index = 0;
                int next = fat.getFatTable(fat_index); ;
                fat_index = firstCluster;
                if (firstCluster != 0)
                {
                    do
                    {
                        ls.AddRange(virtualDisk.readCluster(fat_index));
                        fat_index = next;
                        if (fat_index != -1)
                            next = fat.getFatTable(fat_index);
                    } while (next != -1);



                }

            }
            public void deleteDirectory()
            {
                if (firstCluster != 0)
                {
                    int index = firstCluster;
                    int next = fat.getFatTable(index);
                    do
                    {
                        fat.setFatTable(index, 0);
                        if (index != -1)
                        {
                            index = next;
                            next = fat.getFatTable(index);
                        }

                    } while (index != -1);
                    if (parent != null)
                    {
                        parent.readDirectory();
                        string s = fileName.ToString();
                        int I = parent.searchDirectory(s);
                        if (I != -1)
                        {
                            parent.directoryTable.RemoveAt(I);
                            parent.writeDirectory();
                            fat.writeFat();
                        }
                    }
                }

            }
        }


        static void Main(string[] args)
        {
            commandArg obj1 = new commandArg();
            String directory = "C:\\My file\\FCI\\3-2nd tearm\\os\\file1.txt";
            virtualDisk obj = new virtualDisk();
            obj.initialize(@"C:\My file\FCI\3-2nd tearm\os\file1.txt");
            directory = new string(currentDirectory.fileName);
            currentPath = new string(currentDirectory.fileName);
            Console.WriteLine(directory);
            while (true)
            {
                String user = Console.ReadLine();
                obj1.Command(user, directory);
            }

        }
    }

}
