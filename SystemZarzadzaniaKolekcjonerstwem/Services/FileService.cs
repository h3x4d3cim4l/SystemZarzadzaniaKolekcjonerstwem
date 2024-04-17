using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemZarzadzaniaKolekcjonerstwem.Models;

namespace SystemZarzadzaniaKolekcjonerstwem.Services
{
    public class FileService
    {

        static readonly string filepath = FileSystem.AppDataDirectory;

        public static Boolean CheckAvailable(string filename)
        {
            return !File.Exists(Path.Combine(filepath,filename+".txt"));
        }

        public static void WriteCollection(string filename)
        {
            File.Create($"{filepath}/{filename}.txt");
        }

        public static void DeleteCollection(string filename)
        {
            File.Delete($"{filepath}/{filename}.txt");
        }

        public static List<string> ReadCollectionState()
        {
            List<string> list = new List<string>();
            foreach(var file in Directory.GetFiles(filepath)){
                string filestr = file.Replace(filepath + "\\", "").Replace(".txt","");
                list.Add(filestr);
            }
            return list;

        }

        public static void WriteColItems(string colname, List<Item> items)
        {
            string toSave = String.Empty;
            foreach (var item in items)
            {
                toSave += $"{item.Name},";
            }
            File.WriteAllText(Path.Combine(filepath, colname + ".txt"),toSave);
        }

        public static List<Item> ReadColItems(string colname)
        {
            List<Item> items = new List<Item>();
            string fileoutput;
            try
            {
                fileoutput = File.ReadAllText(Path.Combine(filepath, colname + ".txt"));
            }
            catch (Exception e)
            {
               return items;
            }
            foreach (var itemname in fileoutput.Split(new char[] { ',' }))
            {
                if(itemname != "")
                {
                    items.Add(new Item(itemname));
                }
            }
            return items;
        }

        public static string GetFilepath()
        {
            return filepath;
        }
    }
}
