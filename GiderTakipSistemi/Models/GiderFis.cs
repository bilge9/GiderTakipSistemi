using Microsoft.AspNetCore.Identity;
using System;

namespace GiderTakipSistemi.Models
{
    public class GiderFis
    {
        public int Id { get; set; }
        public int GiderKalemId { get; set; }
        public GiderKalem? GiderKalem { get; set; }

        public int CariKayitId { get; set; }
        public CariKayit? CariKayit { get; set; }

        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; }

        public string? UserId { get; set; }
    }
}
