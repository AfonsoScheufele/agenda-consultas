using System.Diagnostics;
using AgendaConsultas.Data;
using AgendaConsultas.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendaConsultas.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var consultas = RepositorioMemoria.ListarConsultas();
        var agora = DateTime.Now;

        var model = new DashboardViewModel
        {
            TotalMedicos = RepositorioMemoria.ListarMedicos().Count,
            TotalConsultas = consultas.Count,
            ConsultasAgendadas = consultas.Count(c => c.Status == StatusConsulta.Agendada),
            ConsultasRealizadas = consultas.Count(c => c.Status == StatusConsulta.Realizada),
            ConsultasCanceladas = consultas.Count(c => c.Status == StatusConsulta.Cancelada),
            ProximasConsultas = consultas
                .Where(c => c.Status == StatusConsulta.Agendada && c.DataHora >= agora)
                .OrderBy(c => c.DataHora)
                .Take(5)
                .ToList(),
            UltimasConsultas = consultas
                .OrderByDescending(c => c.Id)
                .Take(5)
                .ToList()
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
