using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaControleAlunos.Models
{
    //11º step
    //cria a classe UsuarioView que pega todas as informações da classe usuario e o metodo HttpPostedFileBase para ser consumido
    public class UsuarioView
    {
        public Usuario Usuario { get; set; }
        public HttpPostedFileBase Foto { get; set; }
    }
}