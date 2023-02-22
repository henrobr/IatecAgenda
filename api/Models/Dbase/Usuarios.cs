namespace api.Models.Dbase
{
    public class Usuarios : IdentityUser
    {
        public Usuarios()
        {
            Agendas = new HashSet<Agendas>();
            AgendasCompartilhadas = new HashSet<AgendasCompartilhadas>();
        }
        [Column(TypeName = "nvarchar(50)")]
        public string Nome { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DtInsert { get; set; }
        //Agendas
        [InverseProperty(nameof(Dbase.Agendas.IdUsuarioNavigation))]
        public ICollection<Agendas> Agendas { get; set; }

        //Compartilhamentos
        [InverseProperty(nameof(Dbase.AgendasCompartilhadas.IdUsuarioNavigation))]
        public ICollection<AgendasCompartilhadas> AgendasCompartilhadas { get; set; }
    }
}
