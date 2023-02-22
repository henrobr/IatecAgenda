namespace api.Models.Dbase
{
    public class AgendasCompartilhadas
    {
        [Key, Column(Order = 1)]
        public int IdAgenda { get; set; }
        [Key, Column(Order = 2)]
        public string IdUsuario { get; set; }

        //Agendas
        [ForeignKey(nameof(IdAgenda))]
        public Agendas IdAgendaNavigation { get; set; }

        //Usuarios
        [ForeignKey(nameof(IdUsuario))]
        public Usuarios IdUsuarioNavigation { get; set; }

    }
}
