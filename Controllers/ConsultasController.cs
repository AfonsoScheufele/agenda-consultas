using AgendaConsultas.Data;
using AgendaConsultas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgendaConsultas.Controllers;

public class ConsultasController : Controller
{
    public IActionResult Index(string? busca, string? status, string? ordenarPor, string? ordem)
    {
        var lista = RepositorioMemoria.ListarConsultas().AsEnumerable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var termo = busca.Trim();
            lista = lista.Where(c =>
                c.NomePaciente.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                (c.Medico?.Nome?.Contains(termo, StringComparison.OrdinalIgnoreCase) ?? false) ||
                c.Motivo.Contains(termo, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<StatusConsulta>(status, ignoreCase: true, out var statusFiltro))
        {
            lista = lista.Where(c => c.Status == statusFiltro);
        }

        var asc = !string.Equals(ordem, "desc", StringComparison.OrdinalIgnoreCase);
        lista = (ordenarPor?.ToLowerInvariant()) switch
        {
            "paciente" => asc ? lista.OrderBy(c => c.NomePaciente) : lista.OrderByDescending(c => c.NomePaciente),
            "medico" => asc ? lista.OrderBy(c => c.Medico?.Nome) : lista.OrderByDescending(c => c.Medico?.Nome),
            "status" => asc ? lista.OrderBy(c => c.Status) : lista.OrderByDescending(c => c.Status),
            "motivo" => asc ? lista.OrderBy(c => c.Motivo) : lista.OrderByDescending(c => c.Motivo),
            _ => asc ? lista.OrderBy(c => c.DataHora) : lista.OrderByDescending(c => c.DataHora)
        };

        ViewBag.Busca = busca;
        ViewBag.Status = status;
        ViewBag.OrdenarPor = ordenarPor ?? "datahora";
        ViewBag.Ordem = asc ? "asc" : "desc";
        ViewBag.StatusLista = new SelectList(Enum.GetValues<StatusConsulta>(), status);

        return View(lista.ToList());
    }

    public IActionResult Details(int id)
    {
        var consulta = RepositorioMemoria.ObterConsulta(id);
        if (consulta is null)
            return NotFound();

        return View(consulta);
    }

    public IActionResult Create()
    {
        CarregarMedicos();
        return View(new Consulta
        {
            DataHora = DateTime.Now.AddHours(1),
            Status = StatusConsulta.Agendada
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Consulta consulta)
    {
        ValidarMedicoExistente(consulta);

        if (!ModelState.IsValid)
        {
            CarregarMedicos(consulta.MedicoId);
            return View(consulta);
        }

        RepositorioMemoria.AdicionarConsulta(consulta);
        TempData["Sucesso"] = "Consulta cadastrada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var consulta = RepositorioMemoria.ObterConsulta(id);
        if (consulta is null)
            return NotFound();

        CarregarMedicos(consulta.MedicoId);
        return View(consulta);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Consulta consulta)
    {
        if (id != consulta.Id)
            return NotFound();

        ValidarMedicoExistente(consulta);

        if (!ModelState.IsValid)
        {
            CarregarMedicos(consulta.MedicoId);
            return View(consulta);
        }

        if (!RepositorioMemoria.AtualizarConsulta(consulta))
            return NotFound();

        TempData["Sucesso"] = "Consulta atualizada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var consulta = RepositorioMemoria.ObterConsulta(id);
        if (consulta is null)
            return NotFound();

        return View(consulta);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!RepositorioMemoria.ExcluirConsulta(id))
            return NotFound();

        TempData["Sucesso"] = "Consulta excluída com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    private void CarregarMedicos(int? selecionado = null)
    {
        var medicos = RepositorioMemoria.ListarMedicos();
        ViewBag.MedicoId = new SelectList(medicos, "Id", "Nome", selecionado);
    }

    private void ValidarMedicoExistente(Consulta consulta)
    {
        if (consulta.MedicoId <= 0)
            return;

        if (RepositorioMemoria.ObterMedico(consulta.MedicoId) is null)
            ModelState.AddModelError(nameof(Consulta.MedicoId), "Médico selecionado não existe.");
    }
}
