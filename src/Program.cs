        static Contact GetContactToRemoveFromUser(Program program)
        {
            Console.WriteLine("Unesite broj mobitela kontakta koje želite ukloniti: ");
            string phoneNumber = Console.ReadLine();

            Contact contactToRemove = program.GetContacts().FirstOrDefault(c => c.PhoneNumber == phoneNumber);

            if (contactToRemove == null)
            {
                Console.WriteLine("Ne postoji korisnik s tim brojem mobitela.");
            }

            return contactToRemove;
        }



        static Contact GetContactToUpdatePreference(Program program)
        {
            Console.WriteLine("Unesite broj mobitela kontakta kojem želite promijeniti preferencu: ");
            string phoneNumber = Console.ReadLine();

            Contact existingContact = program.GetContacts().FirstOrDefault(c => c.PhoneNumber == phoneNumber);

            if (existingContact == null)
            {
                Console.WriteLine("Kontakt ne postoji u imeniku. Promjena nije izvršena.");
            }

            return existingContact;
        }

        void PrintDescendingCalls()
        {
            Contact contactToPrint = GetContactToPrintCalls();

            if (contactToPrint != null)
            {
                Console.WriteLine($"Pozivi za kontakt {contactToPrint.FullName}:");

                Dictionary<Contact, List<Call>> phoneBook = GetPhoneBook();

                if (phoneBook.TryGetValue(contactToPrint, out List<Call> callsForContact))
                {
                    callsForContact = callsForContact.OrderByDescending(c => c.CallTime).ToList();

                    foreach (var call in callsForContact)
                    {
                        Console.WriteLine($"Vrijeme poziva: {call.CallTime}, Status: {call.Status}");
                    }
                }
                else
                {
                    Console.WriteLine("Nema poziva za prikaz za odabranog kontakta.");
                }
            }
        }


        void SubMenu()
        {
            Console.WriteLine("1. Ispis svih poziva s tim kontaktom poredan od vremenski najnovijeg");
            Console.WriteLine("2. Kreiranje novog poziva");
            Console.WriteLine("3. Izlaz iz podmenua");

            var subMenuChoice = GetUserSubMenuChoice();

            switch (subMenuChoice)
            {
                case 1:
                    Console.Clear();
                    PrintDescendingCalls();
                    break;
                case 2:
                    Console.Clear();
                    CreateNewCall(this);
                    break;
                case 3:
                    break;

            }
        }

        int GetUserSubMenuChoice()
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Neispravan unos. Unesite broj.");
            }
            return choice;
        }

        Contact GetContactToPrintCalls()
        {
            Console.WriteLine("Unesite broj mobitela kontakta kojem želite ispisati pozive: ");
            string phoneNumber = Console.ReadLine();

            Contact existingContact = GetContacts().FirstOrDefault(c => c.PhoneNumber == phoneNumber);

            if (existingContact == null)
            {
                Console.WriteLine("Kontakt ne postoji u imeniku. Promjena nije izvršena.");
            }

            return existingContact;
        }

        IEnumerable<Contact> GetContacts()
        {
            return phoneBook.Keys;
        }

        void AddContact(Contact contact)
        {
            if (phoneBook.Keys.Any(c => c.PhoneNumber == contact.PhoneNumber))
            {
                Console.Clear();
                Console.WriteLine("Kontakt sa istim brojem mobitela već postoji.");
            }
            else
            {
                phoneBook.Add(contact, new List<Call>());
                Console.Clear();
                Console.WriteLine("Kontakt uspješno dodan.");
            }
        }

        void RemoveContact(Contact contact)
        {
            if (!phoneBook.Keys.Any(c => c.PhoneNumber == contact.PhoneNumber))
            {
                Console.Clear();
                Console.WriteLine("Kontakt s tim brojem ne postoji.");
                Console.ReadKey();
            }
            else
            {
                phoneBook.Remove(contact);
                Console.Clear();
                Console.WriteLine("Kontakt uspješno uklonjen.");
                Console.ReadKey();
            }
        }

        void EditContactPreference(Contact contact, Contact.ContactPreference newPreference)
        {
            if (!phoneBook.Keys.Contains(contact))
            {
                Console.Clear();
                Console.WriteLine("Ne postoji korisnik s tim brojem mobitel.");
                Console.ReadKey();
            }
            else
            {
                contact.Preference = newPreference;
                Console.Clear();
                Console.WriteLine("Uspješno je promijenjena preferenca kontakta.");
                Console.ReadKey();
            }
        }

        static void CreateNewCall(Program program)
        {
            Console.WriteLine("Unesite broj mobitela kontakta koji želite nazvati: ");
            string phoneNumber = Console.ReadLine();

            Contact contact = program.GetContacts().FirstOrDefault(c => c.PhoneNumber == phoneNumber);

            if (contact == null)
            {
                Console.WriteLine("Kontakt ne postoji u imeniku. Poziv nije moguć.");
                return;
            }

            if (contact.Preference == Contact.ContactPreference.Blokiran)
            {
                Console.WriteLine("Nije moguće nazvati blokiran kontakt.");
                return;
            }

            if (IsCallInProgress(program, contact))
            {
                Console.WriteLine("Već postoji poziv u tijeku s odabranim kontaktom.");
                return;
            }

            Random random = new Random();
            DateTime callTime = DateTime.Now;
            CallStatus randomStatus = (CallStatus)random.Next(0, 2); // 0 for Missed, 1 for InProgress
            int callDuration = randomStatus == CallStatus.InProgress ? random.Next(1, 21) : 0; // Duration if InProgress, 0 if Missed

            Call newCall = new Call(callTime, randomStatus);

            if (program.GetPhoneBook().TryGetValue(contact, out List<Call> callsForContact))
            {
                callsForContact.Add(newCall);
                Console.Clear();
                Console.WriteLine("Poziv uspješno kreiran.");

                if (randomStatus == CallStatus.InProgress)
                {
                    Timer timer = new Timer(callback =>
                    {
                        newCall.Status = CallStatus.Completed;
                        Console.Clear();
                        Console.WriteLine($"Poziv završen. Trajanje: {callDuration} sekundi.");
                    }, null, callDuration * 1000, Timeout.Infinite);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Poziv nije odgovoren.");
                }
            }
            else
            {
                Console.WriteLine("Nemoguće nazvati za odabranog kontakta.");
            }
        }


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
