using System;
using System.Collections.Generic;

namespace EFCoreTutorial.Models
{
    public partial class Sepet
    {
        public int Id { get; set; }
        public int? MusteriId { get; set; }
        public DateTime? Tarih { get; set; }
    }
}
