using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;

namespace BUEnrolment.Models
{
    public class Result
    {

        public enum ResultGrade
        {
            HighDistinction,
            Distinction,
            Credit,
            Pass,
            Fail
        }

        private double mark;

        [Key]
        public int Id { get; set; }
        public Subject subject { get; set; }
        public double Mark { get; set; }
        public ResultGrade Grade {
            get
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                ResultGrade grade;

                if (mark >= Convert.ToInt32(appSettings["HighDistinction"]))
                {
                    grade = ResultGrade.HighDistinction;
                }
                else if (mark >= Convert.ToInt32(appSettings["Distinction"]))
                {
                    grade = ResultGrade.Distinction;
                }
                else if (mark >= Convert.ToInt32(appSettings["Credit"]))
                {
                    grade = ResultGrade.Credit;
                }
                else if (mark >= Convert.ToInt32(appSettings["Pass"]))
                {
                    grade = ResultGrade.Pass;
                }
                else
                {
                    grade = ResultGrade.Fail;
                }

                return grade;
            }
        }


        public Result()
        {

        }
    }
}
