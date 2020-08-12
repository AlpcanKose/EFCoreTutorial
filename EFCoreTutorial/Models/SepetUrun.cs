using System;
using System.Collections.Generic;

namespace EFCoreTutorial.Models
{
    public partial class SepetUrun
    {
        public int Id { get; set; }
        public int? SepetId { get; set; }
        public int? Tutar { get; set; }
        public string Aciklama { get; set; }
    }
}
