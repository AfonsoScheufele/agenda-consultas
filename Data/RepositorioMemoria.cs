using AgendaConsultas.Models;

namespace AgendaConsultas.Data;

/// <summary>
/// Armazena médicos e consultas em listas estáticas (em memória).
/// Os dados são perdidos ao reiniciar a aplicação.
/// </summary>
public static class RepositorioMemoria
{
    private static int _proximoMedicoId = 1;
    private static int _proximoConsultaId = 1;
    private static readonly object _lock = new();

    public static List<Medico> Medicos { get; } = new();
    public static List<Consulta> Consultas { get; } = new();

    public static List<Medico> ListarMedicos()
    {
        lock (_lock)
            return Medicos.Select(ClonarMedico).OrderBy(m => m.Nome).ToList();
    }

    public static Medico? ObterMedico(int id)
    {
        lock (_lock)
        {
            var medico = Medicos.FirstOrDefault(m => m.Id == id);
            return medico is null ? null : ClonarMedico(medico);
        }
    }

    public static Medico AdicionarMedico(Medico medico)
    {
        lock (_lock)
        {
            medico.Id = _proximoMedicoId++;
            Medicos.Add(ClonarMedico(medico));
            return ClonarMedico(medico);
        }
    }

    public static bool AtualizarMedico(Medico medico)
    {
        lock (_lock)
        {
            var existente = Medicos.FirstOrDefault(m => m.Id == medico.Id);
            if (existente is null) return false;

            existente.Nome = medico.Nome;
            existente.CRM = medico.CRM;
            existente.Especialidade = medico.Especialidade;
            existente.Telefone = medico.Telefone;
            existente.Email = medico.Email;
            return true;
        }
    }

    public static bool ExcluirMedico(int id)
    {
        lock (_lock)
        {
            if (Consultas.Any(c => c.MedicoId == id))
                return false;

            var medico = Medicos.FirstOrDefault(m => m.Id == id);
            if (medico is null) return false;

            Medicos.Remove(medico);
            return true;
        }
    }

    public static bool MedicoTemConsultas(int id)
    {
        lock (_lock)
            return Consultas.Any(c => c.MedicoId == id);
    }

    public static List<Consulta> ListarConsultas()
    {
        lock (_lock)
        {
            return Consultas
                .Select(ClonarConsulta)
                .OrderBy(c => c.DataHora)
                .ToList();
        }
    }

    public static Consulta? ObterConsulta(int id)
    {
        lock (_lock)
        {
            var consulta = Consultas.FirstOrDefault(c => c.Id == id);
            return consulta is null ? null : ClonarConsulta(consulta);
        }
    }

    public static Consulta AdicionarConsulta(Consulta consulta)
    {
        lock (_lock)
        {
            consulta.Id = _proximoConsultaId++;
            Consultas.Add(ClonarConsulta(consulta));
            return ClonarConsulta(consulta);
        }
    }

    public static bool AtualizarConsulta(Consulta consulta)
    {
        lock (_lock)
        {
            var existente = Consultas.FirstOrDefault(c => c.Id == consulta.Id);
            if (existente is null) return false;

            existente.MedicoId = consulta.MedicoId;
            existente.NomePaciente = consulta.NomePaciente;
            existente.DataHora = consulta.DataHora;
            existente.Motivo = consulta.Motivo;
            existente.Status = consulta.Status;
            return true;
        }
    }

    public static bool ExcluirConsulta(int id)
    {
        lock (_lock)
        {
            var consulta = Consultas.FirstOrDefault(c => c.Id == id);
            if (consulta is null) return false;

            Consultas.Remove(consulta);
            return true;
        }
    }

    private static Medico ClonarMedico(Medico m) => new()
    {
        Id = m.Id,
        Nome = m.Nome,
        CRM = m.CRM,
        Especialidade = m.Especialidade,
        Telefone = m.Telefone,
        Email = m.Email
    };

    private static Consulta ClonarConsulta(Consulta c)
    {
        var medico = Medicos.FirstOrDefault(m => m.Id == c.MedicoId);
        return new Consulta
        {
            Id = c.Id,
            MedicoId = c.MedicoId,
            NomePaciente = c.NomePaciente,
            DataHora = c.DataHora,
            Motivo = c.Motivo,
            Status = c.Status,
            Medico = medico is null ? null : ClonarMedico(medico)
        };
    }
}
