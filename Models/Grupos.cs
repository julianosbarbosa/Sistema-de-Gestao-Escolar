using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaControleAlunos.Models
{
    public class Grupos
    {
        //6º step
        // cria class grupos que vai monta a tabela grupos no banco, passa validações atravez do DataAnnotations
        // passa UserId para associar a tabela de usuario
        // a proprieda virtual vai relacionar tabelas atravez de uma chave estrageira
        [Key]
        public int GrupoId { get; set; }

        [Required(ErrorMessage = " O Campo {0} é Obrigatório!")]
        [StringLength(50, ErrorMessage = " O Campo {0} pode ter no máximo {1} e minimo {2} caracteres ", MinimumLength = 3)]
        [Index("GrupoDescricaoIndex", IsUnique = true)]
        public string Descricao { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public virtual Usuario Professor { get; set; }
        [JsonIgnore]
        public virtual ICollection<GruposDetalhes> GroupoDetalhes { get; set; }

    }
}