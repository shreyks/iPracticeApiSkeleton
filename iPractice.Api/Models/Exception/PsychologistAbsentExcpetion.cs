using System;

namespace iPractice.Api.Models.Exception
{
    public class PsychologistAbsentException : System.Exception
    {
        public string Message = "Psychologist ID doesn't exist in our database.";
    }
}
