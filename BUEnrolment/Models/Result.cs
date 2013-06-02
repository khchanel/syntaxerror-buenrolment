using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Collections.Specialized;

namespace BUEnrolment.Models
{
    /// <summary>
    /// Model representation of Result
    /// </summary>
    public class Result
    {

        /// <summary>
        /// Representation of grade
        /// </summary>
        public enum ResultGrade
        {
            HighDistinction = 4,
            Distinction = 3,
            Credit = 2,
            Pass = 1,
            Fail = 0
        }

        /// <summary>
        /// Unique ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Reference to Subject
        /// </summary>
        public Subject Subject { get; set; }

        /// <summary>
        /// Result marks
        /// </summary>
        [Range(0, 100, ErrorMessage="Mark has to be within 0 to 100")]
        public double? Mark { get; set; }

        /// <summary>
        /// Calculate grade from Mark at runtime according to app settings
        /// </summary>
        public ResultGrade Grade {
            get
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                ResultGrade grade;

                // Conversion..
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

        /// <summary>
        /// Default constructor
        /// </summary>
        public Result()
        {
            // do nothing
        }
    }
}
