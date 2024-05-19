using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafio.Controllers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;
        // Contrutor
        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Buscar o Id no banco utilizando o EF
            var tarefa = _context.Tarefas.Find(id);            
            // Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound            
            if (tarefa == null)
                return NotFound();
            // caso contrário retornar OK com a tarefa encontrada
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            //Buscar todas as tarefas no banco utilizando o EF
            // List<Tarefa> tarefas = new List<Tarefa>();            
            // tarefas = _context.Tarefas.ToList();

            List<Tarefa> tarefas = _context.Tarefas.ToList();          
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            var tarefa = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));
                       
            return Ok(tarefa);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            _context.Add(tarefa);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            if (tarefa.Titulo != "string")            
                tarefaBanco.Titulo = tarefa.Titulo;
            if (tarefa.Descricao != "string") 
                tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            // Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();


            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent(); // nada para retornar
        }

// ----------------------------------------------------------------------------------------------------------

        //  [HttpDelete("DeletarPorTitulo")]
        // public IActionResult DeletarPorTitulo(string titulo)
        // {
        //     var tarefaBanco = _context.Tarefas.Where(x => x.Titulo == titulo ).ToList();

        //     if (!tarefaBanco.Any())
        //         return NotFound();

        //     // Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
        //     _context.Tarefas.RemoveRange(tarefaBanco);
        //     _context.SaveChanges();

        //     return NoContent(); // nada para retornar
        // }



        // [HttpPut("AtualizarPorTitulo")]
        // public async Task<IActionResult> AtualizarPorTitulo(string titulo, [FromBody] Tarefa tarefa)
        // {
        //     var tarefaBanco = await _context.Tarefas.FirstOrDefaultAsync(x => x.Titulo == titulo);

        //     if (tarefaBanco == null)
        //         return NotFound();

        //     if (tarefa.Data == DateTime.MinValue)
        //         return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

        //     // Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
        //     if (tarefa.Titulo != "string")            
        //         tarefaBanco.Titulo = tarefa.Titulo;
        //     if (tarefa.Descricao != "string") 
        //         tarefaBanco.Descricao = tarefa.Descricao;
        //     tarefaBanco.Data = tarefa.Data;
        //     tarefaBanco.Status = tarefa.Status;

        //     // Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
        //     _context.Tarefas.Update(tarefaBanco);
        //     _context.SaveChanges();


        //     return Ok();
        // }

    }
}
