        static bool IsCallInProgress(Program program, Contact contact)
        {
            if (program.GetPhoneBook().TryGetValue(contact, out List<Call> callsForContact))
            {
                return callsForContact.Any(call => call.Status == CallStatus.InProgress);
            }
            return false;
        }
        Dictionary<Contact, List<Call>> GetPhoneBook()
        {
            return phoneBook;
        }
