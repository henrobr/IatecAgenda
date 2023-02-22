namespace api.Models.Dbase
{
    public class Agendas
    {
        public Agendas()
        {
            UsuariosCompartilhados = new HashSet<AgendasCompartilhadas>();
        }
        [Key]
        public int Id { get; set; }
        public string IdUsuario { get; set; }
        public bool Particular { get; set; } = true;
        [Column(TypeName = "nvarchar(100)")]
        public string Nome { get; set; }
        [Column(TypeName = "ntext")]
        public string? Descricao { get; set; }
        [Column(TypeName = "date")]
        public DateTime DtEvento { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? HrEvento { get; set; }
        [Column(TypeName = "ntext")]
        public string? Local { get; set; }
        [Column(TypeName = "ntext")]
        public string? Participantes { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DtInsert { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DtEdit { get; set; }

        //Usuario
        [ForeignKey(nameof(IdUsuario))]
        public Usuarios IdUsuarioNavigation { get; set; }

        //Compartilhamentos
        [InverseProperty(nameof(AgendasCompartilhadas.IdAgendaNavigation))]
        public ICollection<AgendasCompartilhadas> UsuariosCompartilhados { get; set; }
    }
}
