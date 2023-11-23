public class Contact
{
    public string FullName { get; }
    public string PhoneNumber { get; }
    public ContactPreference Preference { get; set; }

    public Contact(string fullName, string phoneNumber, ContactPreference preference)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Preference = preference;
    }

    public override bool Equals(object obj)
    {
        if (obj is Contact otherContact)
        {
            return PhoneNumber == otherContact.PhoneNumber;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return PhoneNumber.GetHashCode();
    }
    public enum ContactPreference
    {
        Favorit,
        Normalan,
        Blokiran
    }
}

