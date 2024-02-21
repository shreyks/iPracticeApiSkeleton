using System;

namespace iPractice.Api.Models.Exception
{
    public class PsychologistUnavailableExcpetion : System.Exception
    {
        public string Message = "Psychologist isn't available at this time slot.";
    }
}
