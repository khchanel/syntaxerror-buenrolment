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
        public double Mark {
            get
            {
                return mark;
            }
            set
            {
                mark = value;
                Grade = MarkToGrade(mark);
            }
        }
        public ResultGrade Grade { get; private set; }
        public Dictionary<ResultGrade, double> GradeMap { get; private set; }


        public Result()
        {
            SetupGradeMap();
        }


        private void SetupGradeMap()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            GradeMap = new Dictionary<ResultGrade, double>();
            GradeMap.Add(ResultGrade.HighDistinction, Convert.ToInt32(appSettings["HighDistinction"]));
            GradeMap.Add(ResultGrade.Distinction, Convert.ToInt32(appSettings["Distinction"]));
            GradeMap.Add(ResultGrade.Credit, Convert.ToInt32(appSettings["Credit"]));
            GradeMap.Add(ResultGrade.Pass, Convert.ToInt32(appSettings["Pass"]));
            GradeMap.Add(ResultGrade.Fail, Convert.ToInt32(appSettings["Fail"]));

        }

        private ResultGrade MarkToGrade(double mark)
        {
            ResultGrade grade;

            if (mark >= GradeMap[ResultGrade.HighDistinction])
            {
                grade = ResultGrade.HighDistinction;
            }
            else if (mark >= GradeMap[ResultGrade.Distinction])
            {
                grade = ResultGrade.Distinction;
            }
            else if (mark >= GradeMap[ResultGrade.Credit])
            {
                grade = ResultGrade.Credit;
            }
            else if (mark >= GradeMap[ResultGrade.Pass])
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
}
