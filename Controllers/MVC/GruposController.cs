using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaControleAlunos.Models;

namespace SistemaControleAlunos.Controllers.MVC
{

    //30º step
    // cria o controller de Grupos no scaffolding seleciona model grupos e o context padrao e na classe usuario habilita a propriedade de grupos
    // autorize somente professor  ter acesso 
    //
    [Authorize(Roles = "Professor")]
    public class GruposController : Controller
    {
        private ControleContext db = new ControleContext();

        //Excluir estudantes
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gruposDetalhes = db.GruposDetalheses.Find(id);
            if (gruposDetalhes == null)
            {
                return HttpNotFound();
            }

            db.GruposDetalheses.Remove(gruposDetalhes);
            db.SaveChanges();

            return RedirectToAction(string.Format("Details/{0}", gruposDetalhes.GrupoId));
        }

        //Add estudantes
            public ActionResult AddEstudante(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }


            var gruposDetalhes = new GruposDetalhes
            {
                GrupoId = id.Value,
            };

            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Estudante).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto");
            return View(gruposDetalhes);
        }

        // POST para Add alunos
        [HttpPost]
        public ActionResult AddEstudante(GruposDetalhes gruposDetalhes)
        {

            if (ModelState.IsValid)
            {
                var existe = db.GruposDetalheses.Where(gd => gd.GrupoId == gruposDetalhes.GrupoId && gd.UserId == gruposDetalhes.UserId).FirstOrDefault();
                if (existe == null)
                {
                    db.GruposDetalheses.Add(gruposDetalhes);
                    db.SaveChanges();
                    return RedirectToAction(string.Format("Details/{0}", gruposDetalhes.GrupoId));
                }
                ModelState.AddModelError(String.Empty, "Aluno já matriculado");
            }

            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Estudante).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto", gruposDetalhes.UserId);

            return View(gruposDetalhes);


        }

        // GET: Grupos
        public ActionResult Index()
        {
            var grupos = db.Grupos.Include(g => g.Professor);
            return View(grupos.ToList());
        }

        // GET: Grupos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
            return View(grupos);
        }

        // GET: Grupos/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto");
            return View();
        }

        // POST: Grupos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GrupoId,Descricao,UserId")] Grupos grupos)
        {
            if (ModelState.IsValid)
            {
                db.Grupos.Add(grupos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto");
            return View(grupos);
        }

        // GET: Grupos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto", grupos.GrupoId);
            return View(grupos);
        }

        // POST: Grupos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GrupoId,Descricao,UserId")] Grupos grupos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Usuarios, "UserId", "UserName", grupos.UserId);
            return View(grupos);
        }

        // GET: Grupos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
            return View(grupos);
        }

        // POST: Grupos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Grupos grupos = db.Grupos.Find(id);
            db.Grupos.Remove(grupos);
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
