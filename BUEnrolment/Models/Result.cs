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
            HighDistinction = 4,
            Distinction = 3,
            Credit = 2,
            Pass = 1,
            Fail = 0
        }

        [Key]
        public int Id { get; set; }
        public Subject Subject { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage="Mark has to be within 0 to 100")]
        public double Mark { get; set; }

        public ResultGrade Grade {
            get
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                ResultGrade grade;

                if (Mark >= Convert.ToInt32(appSettings["HighDistinction"]))
                {
                    grade = ResultGrade.HighDistinction;
                }
                else if (Mark >= Convert.ToInt32(appSettings["Distinction"]))
                {
                    grade = ResultGrade.Distinction;
                }
                else if (Mark >= Convert.ToInt32(appSettings["Credit"]))
                {
                    grade = ResultGrade.Credit;
                }
                else if (Mark >= Convert.ToInt32(appSettings["Pass"]))
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
