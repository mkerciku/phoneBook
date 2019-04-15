using NUnit.Framework;
using PhoneBookLibrary;
using System.Linq;

namespace Tests
{
    public class Tests
    {
        private PhoneBookEntry pbEntry;
        [SetUp]
        public void Setup()
        {
            pbEntry = new PhoneBookEntry();
        }

        [Test]
        public void CreateTest()
        {
            pbEntry.Name = "Paul";
            pbEntry.EntryDetails.Add(new PhoneBookEntryDetails{NumberType = "Home",Number = "0684040440" });
            pbEntry.EntryDetails.Add(new PhoneBookEntryDetails{NumberType = "Work",Number = "0695050550" });
            PhoneBook.CreatePhoneBookEntry(@"test.bin",pbEntry);
            var entries = PhoneBook.ReadPhoneBook(@"test.bin");

            Assert.IsTrue(entries.Any(x =>x.Name == pbEntry.Name));
        }

        [Test]
        public void UpdateTest()
        {
            pbEntry.Name = "Luigi";
            pbEntry.EntryDetails.Add(new PhoneBookEntryDetails { NumberType = "Home", Number = "0684040440" });
            pbEntry.EntryDetails.Add(new PhoneBookEntryDetails { NumberType = "Work", Number = "0695050550" });
            PhoneBook.CreatePhoneBookEntry(@"test.bin", pbEntry);
            var entries = PhoneBook.ReadPhoneBook(@"test.bin");
            var entryOriginal = entries.Find(x=>x.Name == pbEntry.Name);

            pbEntry.EntryDetails = new System.Collections.Generic.List<PhoneBookEntryDetails> {
                new PhoneBookEntryDetails { NumberType = "Mobile",Number="060606606606"},
                new PhoneBookEntryDetails {NumberType = "Work",Number = "0695050550"}
            };

            PhoneBook.EditPhoneBookEntry(@"test.bin", pbEntry);
            entries = PhoneBook.ReadPhoneBook(@"test.bin");
            var entryUpdated = entries.Find(x => x.Name == pbEntry.Name);

            Assert.AreNotEqual(entryOriginal, entryUpdated);
        }


        [Test]
        public void DeleteTest()
        {
            pbEntry.Name = "Jane";
            pbEntry.EntryDetails.Add(new PhoneBookEntryDetails { NumberType = "Home", Number = "0684040440" });
            pbEntry.EntryDetails.Add(new PhoneBookEntryDetails { NumberType = "Work", Number = "0695050550" });
            PhoneBook.CreatePhoneBookEntry(@"test.bin", pbEntry);
            PhoneBook.DeletePhoneBookEntry(@"test.bin", pbEntry);
            var entries = PhoneBook.ReadPhoneBook(@"test.bin");

            Assert.IsFalse(entries.Any(x => x.Name == pbEntry.Name));
        }
    }
}