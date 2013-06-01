﻿using System;
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
        public Subject subject { get; set; }
        public double Mark { get; set; }
        public ResultGrade Grade {
            get
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                ResultGrade grade;

                if (Mark >= Convert.ToDouble(appSettings["HighDistinction"]))
                {
                    grade = ResultGrade.HighDistinction;
                }
                else if (Mark >= Convert.ToDouble(appSettings["Distinction"]))
                {
                    grade = ResultGrade.Distinction;
                }
                else if (Mark >= Convert.ToDouble(appSettings["Credit"]))
                {
                    grade = ResultGrade.Credit;
                }
                else if (Mark >= Convert.ToDouble(appSettings["Pass"]))
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
