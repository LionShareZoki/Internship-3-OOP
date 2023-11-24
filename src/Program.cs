using System;
using System.Collections.Generic;
using System.Linq;

using phoneBook.Classes;

namespace phoneBook
{
    public class Program
    {
        private readonly Dictionary<Contact, List<Call>> phoneBook = new Dictionary<Contact, List<Call>>();

        static void Main()
        {
            var program = new Program();

            while (true)
            {
                Console.WriteLine("1. Ispiši sve kontakte");
                Console.WriteLine("2. Dodaj novi kontakt");
                Console.WriteLine("3. Izbriši kontakt ");
                Console.WriteLine("4. Uredi preferencu kontakta");
                Console.WriteLine("5. Upravljaj kontaktom");
                Console.WriteLine("6. Ispis svih poziva");
                Console.WriteLine("7. Exit");

                int choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        program.PrintAllContacts();
                        break;

                    case 2:
                        Console.Clear();
                        Contact newContact = GetNewContactFromUser();
                        program.AddContact(newContact);
                        break;
                    case 3:
                        Console.Clear();
                        Contact contactToRemove = GetContactToRemoveFromUser(program);
                        program.RemoveContact(contactToRemove);
                        break;

                    case 4:
                        Console.Clear();
                        Contact contactToUpdate = GetContactToUpdatePreference(program);
                        Console.WriteLine("Unesite novu preferencu kontakta (Favorit, Normalan, Blokiran): ");
                        if (Enum.TryParse(Console.ReadLine(), out Contact.ContactPreference newPreference))
                        {
                            program.EditContactPreference(contactToUpdate, newPreference);
                        }
                        else
                        {
                            Console.WriteLine("Neispravna vrijednost preference. Promjena nije izvršena.");
                        }
                        break;
                    case 5:
                        Console.Clear();
                        program.SubMenu();
                        break;
                    case 6:
                        Console.Clear();
                        program.PrintAllCalls();
                        break;
                    case 7:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Neispravan unos. Molim vas pokušajte ponovo.");
                        break;
                }
            }
        }

        public void PrintAllContacts()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Svi kontakti u imeniku:");

                foreach (var contact in phoneBook.Keys)
                {
                    Console.WriteLine($"Ime: {contact.FullName}, Broj mobitela: {contact.PhoneNumber}, Preferenca: {contact.Preference}");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }


        static int GetUserChoice()
        {
            int choice;
            try
            {
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Neispravan unos. Unesite broj.");
                }
                return choice;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                Console.Clear();
                return 7;
            }
        }

        static Contact GetNewContactFromUser()
        {
            try
            {
                Console.WriteLine("Unesite puno ime: ");
                string fullName = Console.ReadLine();

                Console.WriteLine("Unesite broj mobitela: ");
                string phoneNumber = Console.ReadLine();

                Console.WriteLine("Unesite kontakt preferencu (Favorit, Normalan, Blokiran): ");
                if (Enum.TryParse(Console.ReadLine(), out Contact.ContactPreference preference))
                {
                    return new Contact(fullName, phoneNumber, preference);
                }
                else
                {
                    Console.WriteLine("Neispravna vrijednost preference. Unesena preferenca 'Normalan'.");
                    return new Contact(fullName, phoneNumber, Contact.ContactPreference.Normalan);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                return null;
            }
        }


        static Contact GetContactToRemoveFromUser(Program program)
        {
            Console.WriteLine("Unesite broj mobitela kontakta koje želite ukloniti: ");
            try
            {


                string phoneNumber = Console.ReadLine();

                Contact contactToRemove = program.GetContacts().FirstOrDefault(c => c.PhoneNumber == phoneNumber);

                if (contactToRemove == null)
                {
                    Console.WriteLine("Ne postoji korisnik s tim brojem mobitela.");
                    Console.ReadKey();
                    Console.Clear();
                }

                return contactToRemove;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                return null;
            }
        }



        static Contact GetContactToUpdatePreference(Program program)
        {
            Console.WriteLine("Unesite broj mobitela kontakta kojem želite promijeniti preferencu: ");
            try
            {
                string phoneNumber = Console.ReadLine();

                Contact existingContact = program.GetContacts().FirstOrDefault(c => c.PhoneNumber == phoneNumber);

                if (existingContact == null)
                {
                    Console.WriteLine("Kontakt ne postoji u imeniku. Promjena nije izvršena.");
                    Console.ReadKey();
                    Console.Clear();
                }

                return existingContact;
            }
            catch( Exception e)
            {
                return null;
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
            }
        }

        void PrintDescendingCalls()
        {
            try
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
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                Console.Clear();
            }

        }


        void SubMenu()
        {
            Console.WriteLine("1. Ispis svih poziva s tim kontaktom poredan od vremenski najnovijeg");
            Console.WriteLine("2. Kreiranje novog poziva");
            Console.WriteLine("3. Izlaz iz podmenua");
            try
            {
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
            catch( Exception e )
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                Console.Clear();
            }
        }

        int GetUserSubMenuChoice()
        {
            try
            {
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Neispravan unos. Unesite broj.");
                }
                return choice;
            }
            catch(Exception e)
            {   
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                Console.Clear();
                return -1;
            }
        }

        Contact GetContactToPrintCalls()
        {
            Console.WriteLine("Unesite broj mobitela kontakta kojem želite ispisati pozive: ");
            try
            {
                string phoneNumber = Console.ReadLine();

                Contact existingContact = GetContacts().FirstOrDefault(c => c.PhoneNumber == phoneNumber);

                if (existingContact == null)
                {
                    Console.WriteLine("Kontakt ne postoji u imeniku. Promjena nije izvršena.");
                    Console.ReadKey();
                    Console.Clear();
                }

                return existingContact;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                Console.Clear();
                return null;
            }
        }

        IEnumerable<Contact> GetContacts()
        {
            return phoneBook.Keys;
        }

        void AddContact(Contact contact)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contact.FullName) || string.IsNullOrWhiteSpace(contact.PhoneNumber))
                {
                    Console.Clear();
                    Console.WriteLine("Ime i broj mobitela ne smiju biti prazni.");
                    Console.ReadKey();
                    Console.Clear();

                    return;
                }

                if (phoneBook.Keys.Any(c => c.PhoneNumber == contact.PhoneNumber))
                {
                    Console.Clear();
                    Console.WriteLine("Kontakt sa istim brojem mobitela već postoji.");
                    Console.ReadKey();
                    Console.Clear();

                }
                else
                {
                    phoneBook.Add(contact, new List<Call>());
                    Console.Clear();
                    Console.WriteLine("Kontakt uspješno dodan.");
                    Console.ReadKey();
                    Console.Clear();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                Console.Clear();

            }
        }


        void RemoveContact(Contact contact)
        {
            try
            {
                if (!phoneBook.Keys.Any(c => c.PhoneNumber == contact.PhoneNumber))
                {
                    Console.Clear();
                    Console.WriteLine("Kontakt s tim brojem ne postoji.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    phoneBook.Remove(contact);
                    Console.Clear();
                    Console.WriteLine("Kontakt uspješno uklonjen.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch( Exception e )
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                Console.Clear();
            }
        }

        void EditContactPreference(Contact contact, Contact.ContactPreference newPreference)
        {
            try
            {
                if (!phoneBook.Keys.Contains(contact))
                {
                    Console.Clear();
                    Console.WriteLine("Ne postoji korisnik s tim brojem mobitel.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    contact.Preference = newPreference;
                    Console.Clear();
                    Console.WriteLine("Uspješno je promijenjena preferenca kontakta.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void CreateNewCall(Program program)
        {
            Console.WriteLine("Unesite broj mobitela kontakta koji želite nazvati: ");
            try
            {
                string phoneNumber = Console.ReadLine();

                Contact contact = program.GetContacts().FirstOrDefault(c => c.PhoneNumber == phoneNumber);

                if (contact == null)
                {
                    Console.Clear();
                    Console.WriteLine("Kontakt ne postoji u imeniku. Poziv nije moguć.");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }

                if (contact.Preference == Contact.ContactPreference.Blokiran)
                {
                    Console.Clear();
                    Console.WriteLine("Nije moguće nazvati blokiran kontakt.");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }

                if (IsCallInProgress(program, contact))
                {
                    Console.Clear();
                    Console.WriteLine("Već postoji poziv u tijeku s odabranim kontaktom.");
                    Console.ReadKey();
                    Console.Clear();
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
                    Console.ReadKey();
                    Console.Clear();

                    if (randomStatus == CallStatus.InProgress)
                    {
                        Timer timer = new Timer(callback =>
                        {
                            newCall.Status = CallStatus.Completed;
                            Console.Clear();
                            Console.WriteLine($"Poziv završen. Trajanje: {callDuration} sekundi.");
                            Console.ReadKey();
                            Console.Clear();
                        }, null, callDuration * 1000, Timeout.Infinite);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Poziv nije odgovoren.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("Nemoguće nazvati za odabranog kontakta.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                Console.Clear();
            }
        }


        public void PrintAllCalls()
        {
            Console.Clear();
            Console.WriteLine("Svi pozivi u imeniku:");
            try
            {
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
                        Console.ReadKey();
                        Console.Clear();
                    }
                }

                Console.ReadKey();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Dogodila se greška: ${e.Message}");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static bool IsCallInProgress(Program program, Contact contact)
        {
            try
            {
            if (program.GetPhoneBook().TryGetValue(contact, out List<Call> callsForContact))
            {
                return callsForContact.Any(call => call.Status == CallStatus.InProgress);
            }
            }
            catch( Exception e )
            {
                Console.WriteLine($"Dogodila se greška: {e.Message}");
                Console.ReadKey();
                Console.Clear();
            }
            return false;
        }

        Dictionary<Contact, List<Call>> GetPhoneBook()
        {
            return phoneBook;
        }
    }
}