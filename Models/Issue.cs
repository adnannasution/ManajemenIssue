using System;
using System.ComponentModel.DataAnnotations;

namespace NamaProyekAnda.Models
{
    public class Issue
    {

        [Key]
        public int Id { get; set; }  // Primary Key

        public int Kode { get; set; }


        public string Tgl { get; set; } = string.Empty;


        public string Jenis { get; set; } = string.Empty;


        public string DetailIssue { get; set; } = string.Empty;


        public string NamaBrand { get; set; } = string.Empty;


        public string NamaVendor { get; set; } = string.Empty;


        public string JenisVendor { get; set; } = string.Empty;


        public string Status { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;


        public string? ImageData { get; set; }

         public string Tindakan { get; set; } = string.Empty;




    }
}
