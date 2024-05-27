using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    /*
     * clsCountry:
     *   Reads from a fixid list of countries 
     *   Read only 
    */ 
    public class clsCountry
    {       
        public int ID {  get; set; }
        public string Name { get; set; }

        public clsCountry() 
        {
            ID = -1;
            Name = "";
        }

        public  clsCountry(int CountryID, string CountryName)
        {
            this.ID = CountryID;
            this.Name = CountryName;
        }

        public static clsCountry Find(int CountryID)
        {
            string CountryName =  "";

            if (clsCountryData.GetCountry(CountryID, ref CountryName))
                return new clsCountry(CountryID, CountryName);

            return null;
        }

        public static clsCountry Find(string CountryName)
        {
            int CountryID = -1;

            if (clsCountryData.GetCountry(CountryName, ref CountryID))
                return new clsCountry(CountryID, CountryName);

            return null;
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }   
    }
}
