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
            public static void Command(String input, String directory)
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
                        int firstCluster = Program.currentDirectory.directoryTable[index].firstCluster;
                        directory d = new directory(textSplit[1].ToCharArray(), 0x10, firstCluster, 0, Program.currentDirectory);
                        Program.currentDirectory = d;
                        Program.currentPath += (textSplit[1]);
                        Program.currentDirectory.readDirectory();
                        currentPath = "\\" + Program.currentDirectory.fileName;
                        d.readDirectory();
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
                        dirctoryEntry d = new dirctoryEntry(textSplit[1].ToCharArray(), 0, 0, 0);
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
                       // directory d = new directory(textSplit[1].ToCharArray(), 0, f_cluster, 0, Program.currentDirectory);
                        directory d = new directory(textSplit[1].ToCharArray(), 0x10, f_cluster, 0, Program.currentDirectory);
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
                    int index = Program.currentDirectory.searchDirectory(new string(textSplit[1]));
                    if (index != -1)
                    {
                        if (Program.currentDirectory.directoryTable[index].fileAttribute == 0x0)
                        {
                            int FirstCluster = Program.currentDirectory.directoryTable[index].firstCluster;
                            int FileSize = Program.currentDirectory.directoryTable[index].fileSize;
                            string Content = string.Empty;
                            fileEntry file = new fileEntry(textSplit[1].ToCharArray(), 0x0, FirstCluster, FileSize, Program.currentDirectory, Content);
                            file.readFile();
                            Console.WriteLine(file.content);
                            Console.WriteLine(directory + '>');
                        }
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
                        dirctoryEntry d_entry = new dirctoryEntry(textSplit[1].ToCharArray(), 0x0, fC, size);
                        //f.readFile();
                        //Console.WriteLine(f.content);
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
                            dirctoryEntry f = Program.currentDirectory.directoryTable[index];
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
                            fileEntry f = new fileEntry(textSplit[1].ToCharArray(), 0x0, fC, size, Program.currentDirectory, content);
                            f.readFile();
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
                                fC = miniFat.getAvailableBlocks();
                            }
                            else
                            {
                                fC = 0;
                            }
                            fileEntry f = new fileEntry(textSplit[1].ToCharArray(), 0x0, fC, size, Program.currentDirectory, content);
                     
                            f.writeFile();
                            dirctoryEntry d = new dirctoryEntry(name.ToCharArray(), 0, 0, size);
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
                        int f = Program.currentDirectory.directoryTable[index].fileAttribute;
                        if (f == 0x0)
                        {
                            int first_cluster = Program.currentDirectory.directoryTable[index].firstCluster;
                            int file_size = Program.currentDirectory.directoryTable[index].fileSize;
                            fileEntry d = new fileEntry(textSplit[1].ToCharArray(), 0x0, first_cluster, file_size, Program.currentDirectory, null);
                            d.deleteFile();

 
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
                    //int countFileNumber = 0;
                    //int countDireNum = 0;
                    //int countFileSize = 0;
                    Console.WriteLine("Directory of " + Program.currentPath);
                    int counterDirectory = 0, counterfiles = 0, counterSizeFiles = 0;
                    for (int i = 0; i < Program.currentDirectory.directoryTable.Count; i++)
                    {
                        if (Program.currentDirectory.directoryTable[i].fileAttribute == 0x0)
                        {
                            Console.WriteLine(Program.currentDirectory.directoryTable[i].fileSize +
                                "  " + new string(Program.currentDirectory.directoryTable[i].fileName));
                            counterfiles++;
                            counterSizeFiles += Program.currentDirectory.directoryTable[i].fileSize;
                        }
                        else
                        {
                            Console.Write("<Dir>" + "      ");
                            Console.WriteLine(Program.currentDirectory.directoryTable[i].fileName);
                            counterDirectory++;
                        }
                    }
                    Console.WriteLine(counterfiles + " File(s)       " + counterSizeFiles + " bytes");
                    Console.WriteLine(counterDirectory + " Dir(s)   " + miniFat.get_free_space() + "  bytes Free");
                    //Console.WriteLine(directory + '>');
                }
                else
                {
                    Console.WriteLine($"'{textSplit[0]}' is not recognized as an internal or external command,\noperable program or batch file.\n");
                    Console.WriteLine(directory + '>');
                }
            }
        }
        public static class miniFat
        {
            public static int[] fatTable;
            public static void intialize()
            {
                fatTable = new int[1024];
                int i = 0;
                while (i < 5)
                {
                    fatTable[i] = -1;    
                    i++;
                }
            }
            static public void writeFatTable(String p)
            {
                String path = @"C:\My file\FCI\3-2nd tearm\os\commedP\text.txt";     //
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Seek(1024, SeekOrigin.Begin);
                byte[] b = new byte[4096];
                Buffer.BlockCopy(fatTable, 0, b, 0, b.Length);
                fs.Write(b, 0, b.Length);
                fs.Close();
            }
            public static void GetFatTable(String p = @"C:\My file\FCI\3-2nd tearm\os\commedP\text.txt")
            {
                miniFat.intialize();
                int[] arr = new int[fatTable.Length];
                byte[] f = new byte[fatTable.Length];

                String path = p;     //
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Seek(1024, SeekOrigin.Begin);
                fs.Read(f, 0, f.Length);
                Buffer.BlockCopy(f, 0, fatTable, 0, f.Length);
                fs.Close();


            }
            static public int getAvailableBlock()
            {
                for (int i = 0; i < fatTable.Length; i++)
                    if (fatTable[i] == 0)
                        return i;
                return 0;
            }
            static public int getNext(int i)
            {
                return fatTable[i];
            }
            static public void setNext(int index, int value)
            {
                fatTable[index] = value;
            }
            static public int getAvailableBlocks()
            {
                int count = 0;
                for (int i = 0; i < 1024; i++)
                {
                    if (fatTable[i] == 0)
                        count++;
                }
                return count;
            }
            static public int get_free_space()
            {
                return getAvailableBlocks() * 1024;
            }
        }
        public static class virtualDisk
        {
            public static void initialize(string path)
            {
                if (!File.Exists(path))
                {
                    FileStream file = new FileStream(@"C:\My file\FCI\3-2nd tearm\os\commedP\text.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    for (int i = 0; i < 1024; i++)      //super block
                    {
                        file.WriteByte(0);
                    }
                    for (int i = 0; i < 4 * 1024; i++)        //FAT-Table
                    {
                        file.WriteByte((byte)'*');
                    }
                    for (int i = 0; i < 1019 * 1024; i++)
                    {
                        file.WriteByte((byte)'#');
                    }
                    file.Flush();
                    file.Close();
                    miniFat.intialize();
                    directory root = new directory("#: ".ToCharArray(), 0x10, 5, 0, null);
                    root.writeDirectory();
                    Program.currentDirectory = root;


                }
                else
                {
                    miniFat.GetFatTable(@"C:\My file\FCI\3-2nd tearm\os\commedP\text.txt");
                    directory root = new directory("#: ".ToCharArray(), 0x10, 5, 0, null);
                    root.readDirectory();
                    Program.currentDirectory = root;


                }

            }
            public static void writeBlock(byte[] data, int f_index)
            {
                FileStream f = new FileStream(@"C:\My file\FCI\3-2nd tearm\os\commedP\text.txt", FileMode.Open, FileAccess.Write);

                f.Seek(f_index * 1024, SeekOrigin.Begin);

                f.Write(data, 0, 1024);

                f.Close();
            }
            static public byte[] getBlock(int index)
            {
                byte[] f = new byte[1024];
                String path = @"C:\My file\FCI\3-2nd tearm\os\commedP\text.txt";
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Seek(1024 * index, SeekOrigin.Begin);
                fs.Read(f, 0, f.Length);
                fs.Close();
                return f;
            }
        }
        public class dirctoryEntry
        {
            public char[] fileName = new char[11];
            public byte fileAttribute;
            public int fileSize;
            public byte[] fileEmpty = new byte[12];
            public int firstCluster;
            public dirctoryEntry()
            {

            }
            public dirctoryEntry(char[] fileName, byte fileAttribute, int firstCluster, int fileSize)
            {
                this.fileName = fileName;
                this.fileAttribute = fileAttribute;
                this.firstCluster = firstCluster;
                this.fileSize = fileSize;
            }
            public byte[] GetBytes()
            {
                byte[] b = new byte[32];
                byte[] name = new byte[11];
                name = Encoding.ASCII.GetBytes(fileName);

                for (int i = 0; i < 11; i++)
                {
                    if (i < name.Length)
                        b[i] = name[i];
                    else
                        b[i] = (byte)' ';
                }

                b[11] = fileAttribute;

                for (int i = 0; i < 12; i++)
                {
                    b[i + 12] = 0;
                }

                byte[] bb = new byte[4];
                bb = BitConverter.GetBytes(firstCluster);
                for (int i = 0; i < 4; i++)
                {
                    b[i + 24] = bb[i];
                }

                byte[] f = new byte[4];
                f = BitConverter.GetBytes(fileSize);
                for (int i = 0; i < 4; i++)
                {
                    b[i + 28] = f[i];
                }

                return b;
            }
            public dirctoryEntry getDirectoryEntry(byte[] b)
            {
                dirctoryEntry d = new dirctoryEntry();
                for (int i = 0; i < 11; i++)
                {
                    d.fileName[i] = Convert.ToChar(b[i]);
                }

                d.fileAttribute = b[11];

                for (int i = 12, j = 0; i < 24; j++, i++)
                {
                    d.fileEmpty[j] = b[i];
                }

                byte[] p = new byte[4];
                for (int i = 24; i < 28; i++)
                {
                    p[i - 24] = b[i];
                }
                d.firstCluster = BitConverter.ToInt32(p, 0);


                byte[] pp = new byte[4];
                for (int i = 28; i < 32; i++)
                {
                    pp[i - 28] = b[i];
                }
                d.fileSize = BitConverter.ToInt32(pp, 0);

                return d;
            }
            public dirctoryEntry getDirectoryEntry()
            {
                dirctoryEntry d = new dirctoryEntry();
                d.fileName = this.fileName;
                d.fileSize = this.fileSize;
                d.firstCluster = this.firstCluster;
                d.fileAttribute = this.fileAttribute;
                return d;

            }
        }
        public class directory : dirctoryEntry
        {
            public directory parent;
            public List<dirctoryEntry> directoryTable;
            public directory(char[] file_name, byte file_attribute, int first_Cluster, int file_size, directory parent) :
            base(file_name, file_attribute, first_Cluster, file_size)
            {
                if (parent != null)
                {
                    this.parent = parent;
                }
                directoryTable = new List<dirctoryEntry>();
            }
            public void writeDirectory()
            {
                byte[] directory_table_byte = new byte[32 * directoryTable.Count];
                byte[] directory_entry_byte = new byte[32];

                for (int i = 0; i < directoryTable.Count; i++)
                {
                    directory_entry_byte = directoryTable[i].GetBytes();
                    for (int j = i * 32; j < ((i + 1) * 32); j++)
                    {
                        directory_table_byte[j] = directory_entry_byte[j % 32];
                    }
                }
                double noofreqblocks = Math.Ceiling(directory_table_byte.Length / 1024.0);
                int number_of_full_size_block = directory_table_byte.Length / 1024;
                int remainder = directory_table_byte.Length % 1024;
                if (noofreqblocks <= miniFat.getAvailableBlocks())
                {
                    List<byte[]> ls = new List<byte[]>();
                    if (directory_table_byte.Length > 0)
                    {
                        byte[] b = new byte[1024];
                        for (int i = 0; i < number_of_full_size_block; i++)
                        {
                            for (int j = i * 1024; j < ((i + 1) * 1024); i++)
                                b[j % 1024] = directory_table_byte[j];
                            ls.Add(b);
                        }
                        if (remainder > 0)
                        {
                            b = new byte[1024];
                            for (int i = number_of_full_size_block * 1024, k = 0; k < remainder; i++, k++)
                            {
                                b[k] = directory_table_byte[i];
                            }
                            ls.Add(b);
                        }
                        int fat_index;
                        int last_fat = -1;
                        if (this.firstCluster != 0)
                        {
                            fat_index = this.firstCluster;
                        }
                        else
                        {
                            fat_index = miniFat.getAvailableBlock();
                            this.firstCluster = fat_index;
                        }
                        for (int i = 0; i < ls.Count; i++)
                        {
                            virtualDisk.writeBlock(ls[i], fat_index);
                            miniFat.setNext(fat_index, -1);
                            if (last_fat != -1)
                                miniFat.setNext(last_fat, fat_index);
                            last_fat = fat_index;
                            fat_index = miniFat.getAvailableBlock();
                        }
                        if (directoryTable.Count == 0)
                        {
                            if (firstCluster != 0)
                            {
                                miniFat.setNext(firstCluster, 0);
                                firstCluster = 0;
                            }
                        }
                        miniFat.writeFatTable(@"C:\My file\FCI\3-2nd tearm\os\commedP\file1.txt");
                    }
                }
                else
                {
                    Console.WriteLine("directory size exeeds free space size");
                }
            }
            public void readDirectory()
            {
                directoryTable = new List<dirctoryEntry>();
                List<byte> ls = new List<byte>();
                if (firstCluster != 0 && miniFat.getNext(this.firstCluster) != 0)
                {
                    int fat_index = firstCluster;
                    int next = miniFat.getNext(fat_index);
                    do
                    {
                        ls.AddRange(virtualDisk.getBlock(fat_index));
                        fat_index = next;
                        if (fat_index != -1)
                        {
                            fat_index = next;
                            next = miniFat.getNext(fat_index);
                        }
                    } while (next != -1);
                    byte[] b = new byte[32];
                    for (int i = 0; i < ls.Count; i++)
                    {
                        b[i % 32] = ls[i];
                        if ((i + 1) % 32 == 0)
                        {
                            dirctoryEntry d = this.getDirectoryEntry(b);
                            if (d.fileName[0] != '\0')
                            {
                                directoryTable.Add(d);
                            }
                        }
                    }
                }
            }
            public int searchDirectory(string name)
            {
                readDirectory();
                if (name.Length < 11)
                    for (int i = name.Length; i < 11; i++)
                    {
                        name += " ";

                    }
                for (int i = 0; i < directoryTable.Count; i++)
                {
                    string ss = new string(directoryTable[i].fileName);
                    if (ss.Equals(name))
                    {
                        return i;
                    }
                }
                return -1;
            }
            public void updateDirectory(dirctoryEntry d)
            {
                readDirectory();
                int index = searchDirectory(new string(d.fileName));
                if (index != -1)
                {
                    directoryTable.RemoveAt(index);
                    directoryTable.Insert(index, d);
                }

            }
            public void deleteDirectory()
            {
                int index;
                int next;
                if (firstCluster != 0)
                {
                    index = firstCluster;
                    next = miniFat.getNext(index);
                    do
                    {
                        miniFat.setNext(index, 0);
                        index = next;
                        if (index != -1)
                        {
                            next = miniFat.getNext(index);
                        }
                    } while (index != -1);
                }

                if (parent != null)
                {
                    int i;
                    parent.readDirectory();
                    i = parent.searchDirectory(new string(fileName));
                    if (i != -1)
                    {
                        parent.directoryTable.RemoveAt(i);
                        parent.writeDirectory();
                    }
                }
                miniFat.writeFatTable(@"C:\My file\FCI\3-2nd tearm\os\commedP\file1.txt");
            }
        }
        public class fileEntry : dirctoryEntry
        {

            directory parent;
            public string content;

            public fileEntry(char[] file_name, byte file_attribute, int first_Cluster, int file_size, directory parent, string content) : base(file_name, file_attribute, first_Cluster, file_size)

            {
                this.parent = parent;
                this.content = content;
            }
            public void writeFile()
            {
                byte[] n = new byte[content.Length];
                for (int i = 0; i < content.Length; i++)
                {
                    n[i] = (byte)content[i];

                }
                decimal noofreqblocks = Math.Ceiling((decimal)n.Length / 1024);
                decimal remainder = n.Length % 1024;
                int fat_index;
                int last_index = -1;
                List<byte[]> blocks = new List<byte[]>();

                if (noofreqblocks <= miniFat.getAvailableBlocks())
                {
                    if (firstCluster != 0)
                    {
                        fat_index = firstCluster;
                    }
                    else
                    {
                        fat_index = miniFat.getAvailableBlock();
                    }



                    for (int i = 0; i < blocks.Count; i++)
                    {
                        virtualDisk.writeBlock(blocks[i], fat_index);

                        miniFat.setNext(fat_index, -1);
                        if (last_index != -1)
                        {
                            miniFat.setNext(last_index, fat_index);
                            last_index = fat_index;
                            fat_index = miniFat.getAvailableBlock();
                        }
                    }


                }

                miniFat.writeFatTable(@"C:\My file\FCI\3-2nd tearm\os\commedP\file1.txt");
            }
            public void readFile()
            {

                List<byte> ls = new List<byte>();

                int fat_index = 0;
                int next = 0;
                if (firstCluster != 0)
                {
                    fat_index = firstCluster;
                    next = miniFat.getNext(fat_index);
                }
                do
                {
                    ls.AddRange(virtualDisk.getBlock(fat_index));
                    fat_index = next;
                    if (fat_index != -1)
                    {
                        next = miniFat.getNext(fat_index);
                    }

                } while (next != 0);
                content = Encoding.ASCII.GetString(ls.ToArray());


            }
            public void deleteFile()
            {
                int index;
                int next;
                if (firstCluster != 0)
                {
                    index = firstCluster;
                    next = miniFat.getNext(index);
                    do
                    {
                        miniFat.setNext(index, 0);
                        index = next;
                        if (index != -1)
                        {
                            next = miniFat.getNext(index);
                        }

                    } while (index != -1);
                }

                if (parent != null)
                {
                    int i;
                    parent.readDirectory();
                    i = parent.searchDirectory(new string(fileName));
                    if (i != -1)
                    {
                        parent.directoryTable.RemoveAt(i);
                        parent.writeDirectory();
                    }

                }
                miniFat.writeFatTable(@"C:\My file\FCI\3-2nd tearm\os\commedP\file1.txt");


            }

        }
        static void Main(string[] args)
        {
            String directory = "C:\\My file\\FCI\\3-2nd tearm\\os\\file1.txt";
            virtualDisk.initialize(@"C:\My file\FCI\3-2nd tearm\os\file1.txt");
            currentPath = new string(currentDirectory.fileName).Trim();
           // directory = new string(currentDirectory.fileName);
           // currentPath = new string(currentDirectory.fileName);
            Console.WriteLine(directory+">");
            while (true)
            {
                String user = Console.ReadLine();
                commandArg.Command(user, directory);
            }

        }
    }

}
