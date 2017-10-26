using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaControleAlunos.Classes;
using SistemaControleAlunos.Models;

namespace SistemaControleAlunos.Controllers
{

    //9º step
    //cria o controller de usuarios usa modelo de classe usuario, e o data context controleContext atraves do scafoldinng vai gerar as classes e as views e banco
    public class UsuariosController : Controller
    {
        private ControleContext db = new ControleContext();

        // GET: Usuarios
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioView view)
        {
            //14º step
            // passa o parametro UsuarioView no create que ja tem o campo foto, cria um try cactch para validar se o arquivo foi salvo
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(view.Usuario);
                try
                {

                    if (view.Foto != null)
                    {
                        var pic = Utilidades.UploadPhoto(view.Foto);
                        if (!string.IsNullOrEmpty(pic))
                        {
                            view.Usuario.Photo = string.Format("~/Content/Fotos/{0}", pic);
                        }
                    }

                    db.SaveChanges();
                    //17º step
                    //pega o metod em utilidades passa como parametro view.Usuario.UserName
                    //condiciona o usario conforme aS permições atravez do metodo Utilidades.AddRoleToUser seguido dos parametros view.Usuario.UserName e passa o role name da permição que foi configurado em global.asax
                    Utilidades.CreateUserASP(view.Usuario.UserName);
                    if (view.Usuario.Estudante)
                    {
                        Utilidades.AddRoleToUser(view.Usuario.UserName, "Estudante");
                    }
                    if (view.Usuario.Professor)
                    {
                        Utilidades.AddRoleToUser(view.Usuario.UserName, "Professor");
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(view);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            //22º step
            //cria a variavel usuarioview que recebe as propriedades da classe Usuario e passa p minha variavel usuario, 
            //no metodo de ActionResult Edit deve mudar o a parametro Usuario para UsuarioView e use view. objeto do usuario
            //envolve o save chenges com try e catch igual do create
            //insere o mesmo comando do create que verifica se tem foto para poder fazer a edição
            var view = new UsuarioView
            {
                Usuario = usuario
            };

            return View(view);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioView view)
        {
            //28º step 
            //condição para editar e atualizar o usuario antigo
            if (ModelState.IsValid)

            {
                var db2 = new ControleContext();
                var oldUser = db2.Usuarios.Find(view.Usuario.UserId);
                db2.Dispose();
                if (view.Foto != null)
                {
                    var pic = Utilidades.UploadPhoto(view.Foto);
                    if (!string.IsNullOrEmpty(pic))
                    {
                        view.Usuario.Photo = string.Format("~/Content/Fotos/{0}", pic);
                    }
                }
                else
                {
                    view.Usuario.Photo = oldUser.Photo;
                }

                db.Entry(view.Usuario).State = EntityState.Modified;
                try
                {
                    if (oldUser != null && oldUser.UserName != view.Usuario.UserName)
                    {
                        Utilidades.ChangeEmailUserASP(oldUser.UserName, view.Usuario.UserName);
                    }
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(view);
                };
                return RedirectToAction("Index");
            }
            return View(view.Usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
