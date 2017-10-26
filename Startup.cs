using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemaControleAlunos.Startup))]
namespace SistemaControleAlunos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
