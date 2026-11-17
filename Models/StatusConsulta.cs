using System.ComponentModel.DataAnnotations;

namespace AgendaConsultas.Models;

public enum StatusConsulta
{
    [Display(Name = "Agendada")]
    Agendada = 0,

    [Display(Name = "Realizada")]
    Realizada = 1,

    [Display(Name = "Cancelada")]
    Cancelada = 2
}
