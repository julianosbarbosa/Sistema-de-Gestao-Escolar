using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using SistemaControleAlunos.Classes;
using SistemaControleAlunos.Models;

namespace SistemaControleAlunos.Controllers.API
{
    //18º step
    //cria o controller de usuarios api modelo de classe usuario, e o data context controleContext atraves do scafoldinng vai gerar as classes e as views e banco

    [RoutePrefix("API/Usuarios")]
    //19º step
    // passa um pre fixo de url "API/Usuarios" para chamar a classe UsuariosController
    public class UsuariosController : ApiController
    {
        private ControleContext db = new ControleContext();
        //20º step
        // metodo login vem por post que vai ser passado email e senha q vai ser atribuido em obj json, e faz uma verificação de login
        //se tiver tudo correto ele inicia o db context, chama a classe usuario, e verifica se exite o ususario com oscampos se ele fo nulo vai falar q a senha ou usuario ta incorreto
        //se ele existir recupera as informações da tabela do bd e verifica se esta correto
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(JObject form)
        {
            string email = string.Empty;
            string password = string.Empty;
            dynamic jsonObject = form;

            try
            {
                email = jsonObject.Email.Value;
                password = jsonObject.Senha.Value;
            }
            catch
            {
                return this.BadRequest("Chamada Incorreta");
            }

            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.Find(email, password);

            if (userASP == null)
            {
                return this.BadRequest("Usuário ou Senha incorretos");
            }

            var user = db.Usuarios
                .Where(u => u.UserName == email)
                .FirstOrDefault();

            if (user == null)
            {
                return this.BadRequest("Usuário ou Senha incorretos ");
            }

            return this.Ok(user);
        }


        // GET: api/Usuarios
        public List<Usuario> GetUsuarios()
        {
            var usuarios = db.Usuarios.ToList();
            return usuarios;
        }

        // GET: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult GetUsuario(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuario(int id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.UserId)
            {
                return BadRequest();
            }
            //29º step
            //copie db2 da controller mvc para funcionar na api  e as validações try
        
            var db2 = new ControleContext();
            var oldUser = db2.Usuarios.Find(usuario.UserId);
            db2.Dispose();

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                if (oldUser != null && oldUser.UserName != usuario.UserName)
                {
                    Utilidades.ChangeEmailUserASP(oldUser.UserName, usuario.UserName);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.Ok(usuario);
        }

        //21º step
        // apago a anotação respose type, envolve as linhas com try e ctch que adiciona no banco igual a controler usuario do mvc
        // cadastro feito pela web api vai ser sempre como estudante
        public IHttpActionResult PostUsuario(UsuarioSenha usuarioSenha)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //27º step
            //insira todos os campos classe usuario e recupera o id e mude os parametros de usuario p usuarioSenha e defina as 
            var usuario = new Usuario
            {
                Endereco = usuarioSenha.Endereco,
                Professor = false,
                Estudante = true,
                Sobrenome = usuarioSenha.Sobrenome,
                Telefone = usuarioSenha.Telefone,
                UserName = usuarioSenha.UserName
            };

            try
            {
                db.Usuarios.Add(usuarioSenha);
                db.SaveChanges();
                Utilidades.CreateUserASP(usuarioSenha.UserName ,usuarioSenha.Senha);
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            usuarioSenha.UserId = usuario.UserId;
            usuarioSenha.Professor = false;
            usuarioSenha.Estudante = true;

            return this.Ok(usuarioSenha);
        }

        // DELETE: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult DeleteUsuario(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuarios.Remove(usuario);
            db.SaveChanges();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuarios.Count(e => e.UserId == id) > 0;
        }
    }
}