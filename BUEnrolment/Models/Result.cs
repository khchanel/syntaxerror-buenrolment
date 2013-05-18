using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BUEnrolment.Models
{
    public class Result
    {
        [Key]
        public int Id { get; set; }
        public int Mark { get; set; }
        public string Grade
        {
            get
            {
                return null;
            }
            set
            {
                //convert Mark to Grade
                throw new NotImplementedException();
            }

        }

        public Result(int mark)
        {
            Mark = mark;
            //Set Grade
        }
    }
}