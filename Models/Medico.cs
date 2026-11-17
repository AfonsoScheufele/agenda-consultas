using System.ComponentModel.DataAnnotations;

namespace AgendaConsultas.Models;

public class Medico
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre {2} e {1} caracteres.")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CRM é obrigatório.")]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "O CRM deve ter entre {2} e {1} caracteres.")]
    [Display(Name = "CRM")]
    public string CRM { get; set; } = string.Empty;

    [Required(ErrorMessage = "A especialidade é obrigatória.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "A especialidade deve ter entre {2} e {1} caracteres.")]
    [Display(Name = "Especialidade")]
    public string Especialidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "O telefone deve ter entre {2} e {1} caracteres.")]
    [Phone(ErrorMessage = "Informe um telefone válido.")]
    [Display(Name = "Telefone")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(120, ErrorMessage = "O e-mail deve ter no máximo {1} caracteres.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;
}
