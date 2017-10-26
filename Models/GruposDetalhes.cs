using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SistemaControleAlunos.Models
{
    public class GruposDetalhes
    {
        //7º step
        // cria class GruposDetalhes que vai monta a tabela grupos no banco, passa validações atravez do DataAnnotations
        // a proprieda virtual vai relacionar tabelas atravez de uma chave estrageira
        [Key]
        public int GrupoDetalhesId { get; set; }

        public int GrupoId { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual Grupos Grupo { get; set; }

        [JsonIgnore]
        public virtual Usuario Estudante { get; set; }

        public string GrupoEstudante { get { return string.Format("{0} / {1}", Grupo.Descricao, Estudante.NomeCompleto); } }

        public virtual ICollection<Notas> Notas { get; set; }

    }
}