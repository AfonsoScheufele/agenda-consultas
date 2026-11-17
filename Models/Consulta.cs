using System.ComponentModel.DataAnnotations;

namespace AgendaConsultas.Models;

public class Consulta
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Selecione o médico.")]
    [Display(Name = "Médico")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecione o médico.")]
    public int MedicoId { get; set; }

    [Required(ErrorMessage = "O nome do paciente é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do paciente deve ter entre {2} e {1} caracteres.")]
    [Display(Name = "Paciente")]
    public string NomePaciente { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data e hora são obrigatórias.")]
    [Display(Name = "Data e hora")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime DataHora { get; set; }

    [Required(ErrorMessage = "O motivo é obrigatório.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O motivo deve ter entre {2} e {1} caracteres.")]
    [Display(Name = "Motivo")]
    public string Motivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O status é obrigatório.")]
    [Display(Name = "Status")]
    public StatusConsulta Status { get; set; } = StatusConsulta.Agendada;

    // Propriedade de navegação preenchida pelo repositório (não persistida em banco)
    public Medico? Medico { get; set; }
}
