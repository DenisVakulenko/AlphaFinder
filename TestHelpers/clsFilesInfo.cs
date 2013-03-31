using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestHelpers {
    class clsFilesInfo {
        enum DirType {
            Folder,
            Drive,
            Flash,
            CD,
            MusicAlboom,
            PhotoAlboom,
            Films
        }
        enum FileType {
            Simple,
            Text,
            Note,
            Music,
            Image,
            Film,
            ConfigInfo
        }

        struct DirInfo {
            public string Name;
            public DirType Type;
            public int NFileIDs;
            public int[] FileIDs;
        }
        struct FileInfo {
            public string Name;
            public FileType Type;
        }

        class Drive {
            public char Name;
            public int NDirs;
            public int NFiles;
            public DirInfo[] Dirs;
            public FileInfo[] Files;

            public int FindDir(string DirectoryName) {
                for (int i = 0; i < NDirs; i++) {
                    if (Dirs[i].Name == DirectoryName) return i;
                }
                return -1;
            }
            public int AddDir(string DirectoryName) {
                Dirs[NDirs].Name = DirectoryName;
                return NDirs++;
            }

            public int FindFile(string Path) {
                string DirName = System.IO.Path.GetDirectoryName(Path);
                string FileName = System.IO.Path.GetFileName(Path);

                int DirID = FindDir(DirName);
                if (DirID == -1) return -1;
                for (int i = 0; i < Dirs[DirID].NFileIDs; i++) {
                    if (Files[Dirs[DirID].FileIDs[i]].Name == FileName) return i;
                }
                return -1;
            }
            public int AddFile(string Path) {
                string DirName = System.IO.Path.GetDirectoryName(Path);
                int DirID = FindDir(DirName);
                if (DirID == -1) DirID = AddDir(DirName);

                Files[NFiles].Name = System.IO.Path.GetFileName(Path);

                Dirs[DirID].FileIDs[Dirs[DirID].NFileIDs++] = NFiles;

                return NFiles++; 
            }
        }

        int NDrives;
        Drive[] Drives;

        public bool AddRecord(string Path) {
            if (System.IO.File.Exists(Path)) {
                return AddFileRecord(Path);
            }
            return false;
        }

        public bool AddFileRecord(string Path) {
            for (int i = 0; i < NDrives; i++) {
                if (Path[0] == Drives[i].Name) {
                    return true;
                }
            }
            return false;
        }

    }
}
