using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class FindQuarter
    {
        public string GetQuarter(string strquarter)
        {
            // string strquarter = Session[Constants.QUARTER] + "";

            if (strquarter.ToLower() == "current")
            {

                DateTime todaydate = DateTime.Now;
                int year = todaydate.Year - 2000;
                int nextyear = year + 1;
                if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                    strquarter = "Q4'" + year;
                else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                    strquarter = "Q1'" + nextyear;
                else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                    strquarter = "Q2'" + nextyear;
                else
                    strquarter = "Q3'" + nextyear;
            }

            else if (strquarter.ToLower() == "next")
            {

                DateTime todaydate = DateTime.Now.AddMonths(3);
                int year = todaydate.Year - 2000;
                int nextyear = year + 1;
                if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                    strquarter = "Q4'" + year;
                else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                    strquarter = "Q1'" + nextyear;
                else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                    strquarter = "Q2'" + nextyear;
                else
                    strquarter = "Q3'" + nextyear;
            }

            else if (strquarter.ToLower() == "next1")
            {

                DateTime todaydate = DateTime.Now.AddMonths(6);
                int year = todaydate.Year - 2000;
                int nextyear = year + 1;
                if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                    strquarter = "Q4'" + year;
                else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                    strquarter = "Q1'" + nextyear;
                else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                    strquarter = "Q2'" + nextyear;
                else
                    strquarter = "Q3'" + nextyear;
            }

            else if (strquarter.ToLower() == "next2")
            {

                DateTime todaydate = DateTime.Now.AddMonths(9);
                int year = todaydate.Year - 2000;
                int nextyear = year + 1;
                if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                    strquarter = "Q4'" + year;
                else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                    strquarter = "Q1'" + nextyear;
                else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                    strquarter = "Q2'" + nextyear;
                else
                    strquarter = "Q3'" + nextyear;
            }

            else if (strquarter.ToLower() == "prev")
            {

                DateTime todaydate = DateTime.Now.AddMonths(-3);
                int year = todaydate.Year - 2000;
                int nextyear = year + 1;
                if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                    strquarter = "Q4'" + year;
                else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                    strquarter = "Q1'" + nextyear;
                else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                    strquarter = "Q2'" + nextyear;
                else
                    strquarter = "Q3'" + nextyear;
            }

            return strquarter;
        }
    }
