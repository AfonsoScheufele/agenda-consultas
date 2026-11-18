using AgendaConsultas.Data;
using AgendaConsultas.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendaConsultas.Controllers;

public class MedicosController : Controller
{
    // GET: Medicos
    public IActionResult Index(string? busca, string? ordenarPor, string? ordem)
    {
        var lista = RepositorioMemoria.ListarMedicos().AsEnumerable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var termo = busca.Trim();
            lista = lista.Where(m =>
                m.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                m.Especialidade.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                m.CRM.Contains(termo, StringComparison.OrdinalIgnoreCase));
        }

        var asc = !string.Equals(ordem, "desc", StringComparison.OrdinalIgnoreCase);
        lista = (ordenarPor?.ToLowerInvariant()) switch
        {
            "crm" => asc ? lista.OrderBy(m => m.CRM) : lista.OrderByDescending(m => m.CRM),
            "especialidade" => asc ? lista.OrderBy(m => m.Especialidade) : lista.OrderByDescending(m => m.Especialidade),
            "email" => asc ? lista.OrderBy(m => m.Email) : lista.OrderByDescending(m => m.Email),
            _ => asc ? lista.OrderBy(m => m.Nome) : lista.OrderByDescending(m => m.Nome)
        };

        ViewBag.Busca = busca;
        ViewBag.OrdenarPor = ordenarPor ?? "nome";
        ViewBag.Ordem = asc ? "asc" : "desc";

        return View(lista.ToList());
    }

    // GET: Medicos/Details/5
    public IActionResult Details(int id)
    {
        var medico = RepositorioMemoria.ObterMedico(id);
        if (medico is null)
            return NotFound();

        return View(medico);
    }

    // GET: Medicos/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Medicos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Medico medico)
    {
        if (!ModelState.IsValid)
            return View(medico);

        RepositorioMemoria.AdicionarMedico(medico);
        TempData["Sucesso"] = "Médico cadastrado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Medicos/Edit/5
    public IActionResult Edit(int id)
    {
        var medico = RepositorioMemoria.ObterMedico(id);
        if (medico is null)
            return NotFound();

        return View(medico);
    }

    // POST: Medicos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Medico medico)
    {
        if (id != medico.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(medico);

        if (!RepositorioMemoria.AtualizarMedico(medico))
            return NotFound();

        TempData["Sucesso"] = "Médico atualizado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Medicos/Delete/5
    public IActionResult Delete(int id)
    {
        var medico = RepositorioMemoria.ObterMedico(id);
        if (medico is null)
            return NotFound();

        if (RepositorioMemoria.MedicoTemConsultas(id))
            ViewBag.Aviso = "Este médico possui consultas vinculadas. Exclua ou reagende as consultas antes de removê-lo.";

        return View(medico);
    }

    // POST: Medicos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (RepositorioMemoria.MedicoTemConsultas(id))
        {
            TempData["Erro"] = "Não é possível excluir: o médico possui consultas vinculadas.";
            return RedirectToAction(nameof(Index));
        }

        if (!RepositorioMemoria.ExcluirMedico(id))
            return NotFound();

        TempData["Sucesso"] = "Médico excluído com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}
