using System;

namespace iPractice.Api.Models.Exception
{
    public class PsychologistAbsentException : System.Exception
    {
        string message = "Psychologist ID doesn't exist in our database.";
    }
}
