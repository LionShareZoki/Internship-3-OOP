        public void PrintAllCalls()
        {
            Console.Clear();
            Console.WriteLine("Svi pozivi u imeniku:");

            foreach (var entry in phoneBook)
            {
                var contact = entry.Key;
                var calls = entry.Value;

                Console.WriteLine($"Pozivi za kontakt {contact.FullName}:");

                if (calls.Any())
                {
                    foreach (var call in calls)
                    {
                        Console.WriteLine($"Vrijeme poziva: {call.CallTime}, Status: {call.Status}");
                    }
                }
                else
                {
                    Console.WriteLine("Nema poziva za prikaz za odabranog kontakta.");
                }

                Console.WriteLine();
            }

            Console.ReadKey();
        }

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
