using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;

namespace Server
{
    /// <summary>
    /// Summary description for Marksheet
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Marksheet : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string Calculate()
        {
            string subjectsStr = HttpContext.Current.Request.Params["request"];
            List<SubjectModel> subjects = JsonConvert.DeserializeObject<List<SubjectModel>>(subjectsStr);

            double totalMarks = 0;
            int minMarks = subjects[0].ObtainedMarks;
            int maxMarks = subjects[0].ObtainedMarks;
            string minMarksSubjext = subjects[0].Name;   
            string maxMarksSubjext = subjects[0].Name;
            for (int i = 0; i < subjects.Count; i++)
            {
                totalMarks += subjects[i].ObtainedMarks;
                if (maxMarks < subjects[i].ObtainedMarks)
                {
                    maxMarks = subjects[i].ObtainedMarks;
                    maxMarksSubjext = subjects[i].Name;
                }
                else if (minMarks > subjects[i].ObtainedMarks)
                {
                    minMarks = subjects[i].ObtainedMarks;
                    minMarksSubjext = subjects[i].Name;
                }              
            }
            double percent = (totalMarks / (subjects.Count * 100)) * 100;

            MarksheetModel marksheetModel = new MarksheetModel
            {
                Percentage = percent,
                MinMarks = minMarks,
                MaxMarks = maxMarks,
                MinSubjectMarks = minMarksSubjext,
                MaxSubjectMarks = maxMarksSubjext
            };

            string str = JsonConvert.SerializeObject(marksheetModel);
            return str;

        }
        public class SubjectModel
        {
            public string Name { get; set; }
            public int ObtainedMarks { get; set; }
        }

        public class MarksheetModel
        {
            public int MinMarks { get; set; }
            public int MaxMarks { get; set; }
            public string MinSubjectMarks { get; set; }
            public string MaxSubjectMarks { get; set; }
            public double Percentage { get; set; }
        }

    }
}
