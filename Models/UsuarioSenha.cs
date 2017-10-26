using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaControleAlunos.Models
{
    //26º step 
    //cria uma classe UsuarioSenha que herda da classe usuario e anota que nao vai ser mapeada no bd  e anote que sera campo obrigatorio
    [NotMapped]
   
    public class UsuarioSenha:Usuario
    {
        [Required(ErrorMessage = "O Campo {0} é Obrigatório!")]
        public string Senha { get; set; }
    }
}