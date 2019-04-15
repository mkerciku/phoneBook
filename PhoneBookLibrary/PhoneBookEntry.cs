using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBookLibrary
{
    /// <summary>
    /// Class that represents the phone book entries
    /// </summary>
    [Serializable]
    public class PhoneBookEntry
    {
        public PhoneBookEntry()
        {
            EntryDetails = new List<PhoneBookEntryDetails>();
        }
        public  string Name { get; set; }

        public List<PhoneBookEntryDetails> EntryDetails { get; set; }

    }

    /// <summary>
    /// Class that represent the phone book entry details
    /// </summary>
    [Serializable]
    public class PhoneBookEntryDetails
    {
        public PhoneBookEntryDetails()
        {
        }
        public string NumberType { get; set; }
        public string Number { get; set; }

    }
}
