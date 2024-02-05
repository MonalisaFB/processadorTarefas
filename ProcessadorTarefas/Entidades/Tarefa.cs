using ProcessadorTarefas.Servicos;
using static ProcessadorTarefas.Entidades.Tarefa;
using System.Linq;
using System.Collections.Generic;

namespace ProcessadorTarefas.Entidades
{
    public interface ITarefa
    {
        int Id { get; }
        EstadoTarefa Estado { get; }
        DateTime IniciadaEm { get; }
        DateTime EncerradaEm { get; }
        List<Subtarefa> SubtarefasPendentes { get; }
        List<Subtarefa> SubtarefasExecutadas { get; }
    }

    public class Tarefa : ITarefa
    {
        public int Id { get; set; }
        public EstadoTarefa Estado { get; set; }
        public DateTime IniciadaEm { get; set; }
        public DateTime EncerradaEm { get; set; }
        public List<Subtarefa> SubtarefasPendentes { get; set; }
        public List<Subtarefa> SubtarefasExecutadas { get; set; }

       
        public Tarefa()
        {
            Estado = EstadoTarefa.Criada;
            SubtarefasPendentes = new List<Subtarefa>();
            SubtarefasExecutadas = new List<Subtarefa>();
        }

        public void Iniciar()
        {
            Estado = EstadoTarefa.EmExecucao;
            IniciadaEm = DateTime.Now;
        }

        public void Encerrar()
        {
            Estado = EstadoTarefa.Concluida;
            EncerradaEm = DateTime.Now;
        }

        public void AdicionarSubtarefasAleatorias()
        {
            var random = new Random();
            var quantidadeSubtarefas = random.Next(10, 101);

            for (var i = 0; i < quantidadeSubtarefas; i++)
            {
                var duracao = TimeSpan.FromSeconds(random.Next(3, 61));
                SubtarefasPendentes.Add(new Subtarefa { Duracao = duracao });
            }
        }

        public bool TodasSubtarefasConcluidas()
        {
            return SubtarefasPendentes.Count == 0;
        }

        public Subtarefa ObterProximaSubtarefa()
        {
            if (SubtarefasPendentes.Any())
            {
                var proximaSubtarefa = SubtarefasPendentes.First();
                SubtarefasPendentes.Remove(proximaSubtarefa);
                SubtarefasExecutadas.Add(proximaSubtarefa);
                return proximaSubtarefa;
            }

            return new Subtarefa();

        }

        public void Agendar()
        {
            if (Estado == EstadoTarefa.Criada)
            {
                Estado = EstadoTarefa.Agendada;
            }
            else
            {
                throw new InvalidOperationException("A tarefa não de ser agendada no momento!");
            }
        }

        public void Empausa()
        {
            if(Estado == EstadoTarefa.EmExecucao)
            {
                Estado = EstadoTarefa.EmPausa;
            }
            else 
            {
                throw new InvalidOperationException("A tarefa não de ser colocada em pausa no momento!");
                    
            }
        }

        public void Cancelar()
        {
            if(Estado != EstadoTarefa.Concluida && Estado != EstadoTarefa.Cancelada)
            {
                Estado = EstadoTarefa.Cancelada;
            }
            else
            {
                throw new InvalidOperationException("A tarefa não pode ser cancelada no momento!");
            }
        }
    }

    

}
