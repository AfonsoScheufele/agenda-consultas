namespace AgendaConsultas.Models;

public class DashboardViewModel
{
    public int TotalMedicos { get; set; }
    public int TotalConsultas { get; set; }
    public int ConsultasAgendadas { get; set; }
    public int ConsultasRealizadas { get; set; }
    public int ConsultasCanceladas { get; set; }
    public List<Consulta> ProximasConsultas { get; set; } = new();
    public List<Consulta> UltimasConsultas { get; set; } = new();
}
