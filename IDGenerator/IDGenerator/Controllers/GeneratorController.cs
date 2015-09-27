using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IDGenerator.Models;

namespace IDGenerator.Controllers
{
    public class GeneratorController : ApiController
    {
        private static SaIDValidator.SAIDValidatorSoapClient soapClient = new SaIDValidator.SAIDValidatorSoapClient();

        [HttpPost]
        public SaIDValidator.saidType WebServiceResponse(string idNumberToCheck)
        {
            return soapClient.ValidateIdString(null, idNumberToCheck);
        }

        [HttpGet]
        public IDnumber[] generateNewId()
        {
            
            Random rand = new Random();

            //Date of Birth. 1st 6 digits
            string dob = getDateSeq();

            //Get gender sequence
            string gender = getGenderSeq();

            //Country is SA
            string countryind = "0";

            //Racial indicator
            string racialind;

            if (rand.Next(0, 2) == 0)
                racialind = "8";
            else
                racialind = "9";

            //Build string, remove quotes
            string finalID = dob.Trim('"') + gender.Trim('"') + countryind.ToString() + racialind.Trim('"');

            int checkdigit = GetCheckDigit(finalID);

            return new IDnumber[]
            {
                new IDnumber
                {
                    dob = dob,
                    gender = gender,
                    countryind = countryind,
                    racialind = racialind,
                    checkdigit = checkdigit
                }
            };
        }

        private string getDateSeq()
        {
            //Date of Birth. 1st 6 digits
            DateTime now = DateTime.Now;
            string date = now.GetDateTimeFormats('d')[0];

            string yy = date.Substring(2, 2);
            string mm = date.Substring(5, 2);
            string dd = date.Substring(8, 2);
            string dob = yy + mm + dd;

            return dob;
        }

        private string getGenderSeq()
        {
            string genderdigit, b;
            Random rand = new Random();

            //Get random gender indicator. 5 and above is male, below 5 is female
            if (rand.Next(0, 2) == 0)
                genderdigit = Convert.ToString(rand.Next(5, 9));
            else
                genderdigit = Convert.ToString(rand.Next(0, 4));

            for (int i = 0; i < 3; i++)
            {
                b = Convert.ToString(rand.Next(0, 9));
                genderdigit = genderdigit.Trim('"') + b.Trim('"');
            }

            return genderdigit;
        }

        public int GetCheckDigit(string idnum)
        {
            int sumodd = 0, sumeven = 0, n;
            int[] evenarray = new int[6];

            //Odd elements
            for (int i = 0; i < 12; i++)
            {

                if (i % 2 == 0)
                {
                    n = (int)(idnum[i] - '0');
                    sumodd += n;
                }
            }

              string evenfield = "";
              //Even elements into field
              for (int i = 0; i < 12; i++)
              {
                  if (i % 2 != 0)
                  {
                      evenfield = evenfield + idnum[i];
                  }
              }

            
            int num = int.Parse(evenfield);
            int num2 = num * 2;

              //Add up values of even elements
              sumeven = 0;
              while (num2 != 0)
              {
                  sumeven += num2 % 10;
                  num2 /= 10;
              }


            int totalsum = sumodd + sumeven;
            int lastdigit = (totalsum % 10);
            int checkdigit = 10 - lastdigit;

            //Checkdigit cant be 10
            if (lastdigit == 0)
            {
                checkdigit = 0;
            }

            return checkdigit;
        }

    }

}
