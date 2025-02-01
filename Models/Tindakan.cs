using System;
using System.ComponentModel.DataAnnotations;

namespace NamaProyekAnda.Models
{
    public class Tindakan
    {

        [Key]
        public int Id { get; set; }  // Primary Key

        public int Idissue { get; set; }


        public string Tgltindakan { get; set; } = string.Empty;


        public string Tindakanku { get; set; } = string.Empty;


    }
}
