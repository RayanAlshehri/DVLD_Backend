using System;
using System.Data;
using CommonClasses;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsPerson 
    {
        private enum enMode { Add, Update}

        private enMode _Mode;

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName 
        { 
            get
            {
                string FullName = FirstName + " " + SecondName + " ";

                if (ThirdName != null)
                    FullName += ThirdName + " ";
                
                FullName += LastName;

                return FullName;
            }
        } 
        public char Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Age { get { return clsUtility.GetAge(DateOfBirth); } }
        public string NationalNumber { get; set; }

        public clsCountry Country;
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }

        public clsPerson()
        {
            _Mode = enMode.Add;

            ID = -1;
            FirstName = "";
            SecondName = "";
            ThirdName = null;
            LastName = "";
            Gender = 'N';
            DateOfBirth = DateTime.Now;
            NationalNumber = "";
            Country = new clsCountry();
            Phone = "";
            Email = null;
            Address = "";
            ImagePath = null;
        }

        public clsPerson(int ID, string FirstName, string SecondName, string ThirdName, string LastName,
            char Gender, DateTime DateOfBirth, string NationalNumber, int NationalityID,
            string Phone, string Email, string Address, string ImagePath)
        {
            _Mode = enMode.Update;

            this.ID = ID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.Gender = Gender;
            this.DateOfBirth = DateOfBirth;
            this.NationalNumber = NationalNumber;
            Country = clsCountry.Find(NationalityID);
            this.Phone = Phone;
            this.Email = Email;
            this.Address = Address;
            this.ImagePath = ImagePath;
        }      

        private bool _AddNewPerson()
        {
            this.ID = clsPersonData.AddNewPerson(FirstName, SecondName, ThirdName, LastName, Gender, DateOfBirth, NationalNumber,
                Country.ID, Phone, Email, Address, ImagePath);

            return this.ID != -1;
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(ID, FirstName, SecondName, ThirdName, LastName, Gender, DateOfBirth, NationalNumber,
                Country.ID, Phone, Email, Address, ImagePath);
        }

        public static clsPerson Find(int PersonID)
        {
            int NationalityID = -1;
            char Gender ='N';
            string FirstName = "", SecondName = "", ThirdName = null, LastName = "",
                NationalNumber = "", Phone = "", Email = null, Address = "", ImagePath = null;
            DateTime DateOfBirth = DateTime.Now;

            if (clsPersonData.GetPerson(PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName , ref Gender,
                ref DateOfBirth, ref NationalNumber, ref NationalityID, ref Phone, ref Email, ref Address, ref ImagePath))
            {
                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName, Gender, DateOfBirth, NationalNumber, NationalityID,
                    Phone, Email, Address, ImagePath);
            }
            else 
            {
                return null;
            }
        }

        public static clsPerson Find(string NationalNumber) 
        {
            int ID = -1, NationalityID = -1;
            char Gender = 'N';
            string FirstName = "", SecondName = "", ThirdName = null, LastName = ""
                , Phone = "", Email = null, Address = "", ImagePath = null;
            DateTime DateOfBirth = DateTime.Now;

            if (clsPersonData.GetPerson(NationalNumber, ref ID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref Gender,
                ref DateOfBirth, ref NationalityID, ref Phone, ref Email, ref Address, ref ImagePath))
            {
                return new clsPerson(ID, FirstName, SecondName, ThirdName, LastName, Gender, DateOfBirth, NationalNumber, NationalityID,
                    Phone, Email, Address, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        public static bool DeletePerson(string NationalNumber)
        {
            return clsPersonData.DeletePerson(NationalNumber);
        }

        public static bool DoesPersonExist(int PersonID)
        {
            return clsPersonData.DoesPersonExist(PersonID);
        }

        public static bool DoesPersonExist(string NationalNumber)
        {
            return clsPersonData.DoesPersonExist(NationalNumber);
        }

        public static DataTable GetAllPersons()
        {
            return clsPersonData.GetAllPersons();
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewPerson())
                    {
                        clsCountry.Find(Country.ID);
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    { 
                        return false;
                    }

                case enMode.Update:
                    return _UpdatePerson();
            }

            return false;
        }

        public bool HasLicenseForClass(clsLicenseClass.enLicenseClasses LicenseClass)
        {
            clsDriver Driver = clsDriver.FindByPersonID(ID);

            if (Driver == null)
                return false;

            return Driver.HasLicenseForClass(LicenseClass);
        }

        public int GetTodayUpcomingTheoryTestAppointmentIDForPerson()
        {
            return clsTestAppointmentData.GetTodayUpcomingTheoryTestAppointmentIDForPerson(ID);
        }
    }
}
