using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace PhoneBookLibrary
{
    /// <summary>
    /// Main class for the CRUD operations of the library
    /// </summary>
    public static class PhoneBook
    {
        //used readerWriterLockSlim to implement thread safety to the read and write methods to the file.
        /// <summary>
        /// ReaderWriterLockSlim object to implement thread safety to the read and write methods to the file.
        /// </summary>
        public static ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();


        //create entry method
        /// <summary>
        /// Method that creates a phone book entry
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="phoneBookEntry"></param>
        public static void CreatePhoneBookEntry(string filePath, PhoneBookEntry phoneBookEntry)
        {
            List<PhoneBookEntry> phoneBookEntries = new List<PhoneBookEntry>();

            phoneBookEntries = ReadPhoneBook(filePath);
            phoneBookEntries.Add(phoneBookEntry);

            WriteBinatyFile(filePath, phoneBookEntries);
        }




        //edit method (edit by name)
        /// <summary>
        /// Method that edits a phone book entry provided the name of the entry
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="editedEntry"></param>
        public static void EditPhoneBookEntry(string filePath, PhoneBookEntry editedEntry)
        {
            List<PhoneBookEntry> phoneBookEntries = new List<PhoneBookEntry>();
            phoneBookEntries = ReadPhoneBook(filePath);

            try
            {
                PhoneBookEntry unEditedEntry = phoneBookEntries.Find(entry => entry.Name == editedEntry.Name);
                unEditedEntry.Name = editedEntry.Name;
                unEditedEntry.EntryDetails = editedEntry.EntryDetails;

                WriteBinatyFile(filePath, phoneBookEntries);
            }
            catch (Exception)
            {
                throw new Exception("No entry found with that name");
            }

        }



        //delete method ( delete by name)
        /// <summary>
        /// Method that deletes a phone book entry provided the name of the entry
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="entryToDelete"></param>
        public static void DeletePhoneBookEntry(string filePath, PhoneBookEntry entryToDelete)
        {
            List<PhoneBookEntry> phoneBookEntries = new List<PhoneBookEntry>();
            phoneBookEntries = ReadPhoneBook(filePath);

            PhoneBookEntry entry = phoneBookEntries.Find(item => item.Name == entryToDelete.Name);
            phoneBookEntries.Remove(entry);

            WriteBinatyFile(filePath, phoneBookEntries);
        }


        //read method
        /// <summary>
        /// Method to read the binary file and access the phone book entries
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<PhoneBookEntry> ReadPhoneBook(string filePath)
        {
            lockSlim.EnterReadLock();

            try
            {
                List<PhoneBookEntry> phoneBookEntries = new List<PhoneBookEntry>();
                if (File.Exists(filePath) == true)
                {
                    using (Stream stream = File.Open(filePath, FileMode.Open))
                    {
                        if (stream.Length > 0)
                        {
                            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                            var entries = (List<PhoneBookEntry>)binaryFormatter.Deserialize(stream);
                            var sortedList = entries.OrderBy(x => x.Name).ToList();
                            return sortedList;
                        }

                    }
                }

                return phoneBookEntries;
            }
            catch (Exception)
            {

                throw new Exception("Error reading file");
            }
            finally
            {
                lockSlim.ExitReadLock();
            }

        }


        //write binary file method
        /// <summary>
        /// Method to write to binary file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="phoneBookEntries"></param>
        public static void WriteBinatyFile(string filePath, List<PhoneBookEntry> phoneBookEntries)
        {
            lockSlim.EnterWriteLock();
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    stream.Position = 0;
                    binaryFormatter.Serialize(stream, phoneBookEntries);
                }
            }
            catch (Exception)
            {

                throw new Exception("Error writing to file.");
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }

        }
    }
}
